using Autofac;
using Autofac.Extensions.DependencyInjection;
using Man.Dapr.Sidekick;
using OpenWeather.Api;

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

    //NOTE: Using Dapr Sidekick to manage Dapr sidecar
    builder.Services.AddDaprSidekick(p => p.Sidecar = new DaprSidecarOptions { AppId = "OpenWeather" });

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