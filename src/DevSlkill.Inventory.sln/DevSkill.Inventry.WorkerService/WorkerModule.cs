using Autofac;
using Autofac.Core;
using DevSkill.Inventory.Infrastructure;

public class WorkerModule(string connectionString, string migrationAssembly) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationDbContext>().AsSelf()
               .WithParameter("connectionString", connectionString)
               .WithParameter("migrationAssembly", migrationAssembly)
               .InstancePerLifetimeScope();
    }
}