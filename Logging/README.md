# Logging

Having logging is really required in any serious application. It will assist in montoring and debugging.

## References

* [Using ASP.NET Core](https://docs.datalust.co/docs/using-aspnet-core)
* [Enrich Serilog](https://nblumhardt.com/2019/06/selective-enrichment/)
* [Serilog and Seq in eShopContainers](https://github.com/dotnet-architecture/eShopOnContainers/wiki/Serilog-and-Seq)
* [SeriLog on Github](https://github.com/serilog/serilog)
* [Datalust Seq](https://datalust.co/)

## Seq / Serilog 

Serilog is a diagnostic logging library for .NET applications. It is easy to set up, has a clean API, and runs on all recent .NET platforms. While it's useful even in the simplest applications, Serilog's support for structured logging shines when instrumenting complex, distributed, and asynchronous applications and systems.

Seq is the intelligent search, analysis, and alerting server built specifically for modern structured log data. 

Seq is a log server that runs on a central machine. Your applications send structured events through a framework like ASP.NET Core logging or Serilog:

### Install

**Serilog**

```
Install-Package Serilog
Install-Package Serilog.Sinks.Console
Install-Package Serilog.Sinks.File
```

**Seq**

```
Install-Package Seq.Extensions.Logging
```

### Configure

**Startup.cs**

Add seq to the services

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSeq();
    });
    // ...
```

**JSON configuration**

In appsettings.json add a "Seq" property:

```json
{
  "Seq": {
    "ServerUrl": "http://localhost:5341",
    "ApiKey": "1234567890",
    "MinimumLevel": "Trace",
    "LevelOverride": {
      "Microsoft": "Warning"
    }
  }
}
```

### Usage

Use dependency injection to provide a logger to a class:

```csharp
class HomeController : Controller
{
    readonly ILogger<HomeController> _log;

    public HomeController(ILogger<HomeController> log)
    {
        _log = log;
    }

    public IActionResult Index()
    {
        _log.LogInformation("Hello, world!");
    }
}
```



