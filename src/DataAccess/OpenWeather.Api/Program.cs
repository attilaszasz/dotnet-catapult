using Autofac;
using Autofac.Extensions.DependencyInjection;
using Man.Dapr.Sidekick;
using OpenWeather.Api;
using Types;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting OpenWeather API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<OpenWeatherApiModule>());

    builder.Services.AddControllers();

    //We need the environment earlier than the builder is built, so we get it from the configuration
    var environment = builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

    if (environment.Equals("Local", StringComparison.InvariantCultureIgnoreCase))
    {
        var daprComponentsDirectory = builder.Configuration.GetValue<string>("CATAPULT_DAPR_FOLDER") ?? string.Empty;
        if (string.IsNullOrEmpty(daprComponentsDirectory)) throw new ApplicationException("Please set the CATAPULT_DAPR_FOLDER environment variable to point to the Dapr components folder.");

        //NOTE: Using Dapr Sidekick to manage Dapr sidecar
        builder.Services.AddDaprSidekick(p => p.Sidecar = new DaprSidecarOptions { AppId = Constants.Suppliers.OpenWeather, ComponentsDirectory = daprComponentsDirectory });
    }

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsEnvironment("Local"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "OpenWeather API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}