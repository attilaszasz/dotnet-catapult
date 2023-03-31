using Autofac;
using Interfaces;
using Microsoft.Extensions.Configuration;

namespace ConfigurationService
{
    public class ConfigurationServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //NOTE: see how to work with user secrets here: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
            builder.Register(c => new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build())
                .As<IConfigurationRoot>()
                .SingleInstance();

            builder.RegisterType<ConfigurationService>()
                .As<IConfigurationService>();
        }
    }
}
