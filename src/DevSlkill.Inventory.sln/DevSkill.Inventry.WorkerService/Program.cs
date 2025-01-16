using Autofac.Extensions.DependencyInjection;
using Autofac;
using DevSkill.Inventry.WorkerService;
using Serilog;
using Serilog.Events;
using System.Globalization;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;



var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

//Serilog bootstrap Configuration for application logs before application starts
#region Configure Bootstrap Logger using serilog 
Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();
#endregion

try
{
    Log.Information("Service Starting up...");

    var defaultCulture = new CultureInfo("en-GB");
    CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
    CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var migrationAssembly = typeof(Worker).Assembly.FullName;


    IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((services, ls) => ls
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration))
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new WorkerModule(
                        connectionString, migrationAssembly));
                })
                .ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));

                    services.AddHostedService<Worker>();
                })
                .Build();

    Log.Information("Service Started successfully!");

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal Error occurred while starting the Worker Service");
}
finally
{
    Log.CloseAndFlush();
}