using Autofac.Extensions.DependencyInjection;
using Autofac;
using WeatherForecastService.Api;
using Man.Dapr.Sidekick;
using Types;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting WeatherForecast Service");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    //Hook up Autofac container to the Asp.Net dependency injection
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule<WeatherForecastServiceApiModule>());

    // Add services to the container.
    builder.Services.AddControllers();

    //We need the environment earlier than the builder is built, so we get it from the configuration
    var environment = builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

    if (environment.Equals("Local", StringComparison.InvariantCultureIgnoreCase))
    {
        var daprComponentsDirectory = builder.Configuration.GetValue<string>("CATAPULT_DAPR_FOLDER") ?? string.Empty;
        if (string.IsNullOrEmpty(daprComponentsDirectory)) throw new ApplicationException("Please set the CATAPULT_DAPR_FOLDER environment variable to point to the Dapr components folder.");

        //NOTE: Using Dapr Sidekick to manage Dapr sidecar
        builder.Services.AddDaprSidekick(p => p.Sidecar = new DaprSidecarOptions { AppId = Constants.Services.WeatherForecast, ComponentsDirectory = daprComponentsDirectory });
    }
 
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsEnvironment("Local") || app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "WeatherForecast Service terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}