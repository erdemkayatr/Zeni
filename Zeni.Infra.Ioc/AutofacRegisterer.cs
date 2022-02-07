namespace Zeni.Infra.Ioc
{
    public static class AutofacRegisterer
    {
        public static IHostBuilder UseZeniRegisterServices(this IHostBuilder host)
        {

            //AutofacRegister

            host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            host.ConfigureContainer<ContainerBuilder>((hostBuilder, builder) =>
            {
                builder.RegisterAssemblyTypes(GetApplicationAssemblies())
                   .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerMatchingLifetimeScope();

                builder.RegisterAssemblyTypes(GetApplicationAssemblies()).Where(t => t.Namespace?.Contains("Application.Services") == true)
                    .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
            });
            return host;
        }

        private static Assembly[] GetApplicationAssemblies()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                    .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            var zenies = assemblies.Where(x =>
                x.FullName.Split(",")[0].StartsWith("Zeni.Service") &&
                x.FullName.Split(",")[0].EndsWith("Application"));
            return zenies.ToArray();
        }
    }
}
