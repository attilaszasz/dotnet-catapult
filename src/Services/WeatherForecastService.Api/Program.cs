using Autofac.Extensions.DependencyInjection;
using Autofac;
using WeatherForecastService.Api;
using Man.Dapr.Sidekick;

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

    //NOTE: Using Dapr Sidekick to manage Dapr sidecar
    builder.Services.AddDaprSidekick(p => p.Sidecar = new DaprSidecarOptions { AppId = "WeatherService" });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
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