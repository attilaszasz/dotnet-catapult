# dotnet-catapult
An ASP.NET 7 backend solution demonstrating best practices

## v1
Added default ASP.NET Core Web Api project.  
Added Directory.Build.props with some defaults.  

## v2
Refactored default ASP.NET Core Web Api project to resemble a real life system.  
Added logging using Serilog.  
Added tests.  

## v3
Added another weather supplier (OpenWeather), selectable by parameter.  

## v4
Refactored to interface based programming model and dependency injection without using a DI container.  
Able to test WeatherForecastService in isolation.  

### v4.1
Added OpenWeatherAdapter because the OpenWeatherApi library does not follow interface based programming model.  
This way we can inject all dependencies into OpenWeatherSupplier.  
We can test OpenWeatherSupplier in isolation.  

## v5
Added Autofac as DI container.

## v6
Implemented strategy pattern for selecting weather supplier.

## v7
Added multi-level caching to showcase decorator pattern using dependency injection