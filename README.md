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
