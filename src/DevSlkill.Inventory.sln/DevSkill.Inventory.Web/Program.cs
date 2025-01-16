using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using System.Reflection;
using DevSkill.Inventory.Infrastructure.Extensions;

//Serilog bootstrap Configuration for application logs before application starts
#region Configure Bootstrap Logger using serilog 
var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateBootstrapLogger();
#endregion


try
{
    Log.Information("Application Starting up...");

    var defaultCulture = new CultureInfo("en-GB");
    CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
    CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    //Serilog Configuration for application logs after application starts
    #region Serilog integration for Application logs

    builder.Host.UseSerilog((services, ls) => ls
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(builder.Configuration));

    #endregion

    //builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //    options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));
    
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentity();

    builder.Services.AddControllersWithViews();


    //Autofac Configuration for dependency injection
    #region Autofac Configuration For Dependency Injection

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(
        containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly));
    }));

    #endregion

    #region AutoMapper Configuration
    builder.Services.AddAutoMapper(typeof(WebProfile).Assembly);
    #endregion

    //builder.WebHost.UseUrls("http://*:80");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    //app.MapRazorPages();

    Log.Information("Application Started Sucessfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal Error occurred while starting the application");
}
finally
{
    Log.CloseAndFlush();
}