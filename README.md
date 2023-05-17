# NKZSoft.Service.Configuration.Logger

[![Nuget](https://img.shields.io/nuget/v/NKZSoft.Service.Configuration.Logger?style=plastic)](https://www.nuget.org/packages/NKZSoft.Service.Configuration.Logger/)

NKZSoft.Service.Configuration.Logger is a simple logger based on [Serilog](https://github.com/serilog/serilog) to use with a microservice architecture.

## Using
```csharp
services.AddLogging(Configuration)
```

You can [configure](https://github.com/serilog-contrib/Serilog.Enrichers.Sensitive#json-configuration) the Serilog.Enrichers.Sensitive enricher through appsettings.json
```json
{
  "Serilog": {
    "Using": [
      "Serilog.Enrichers.Sensitive"
    ],
    "Enrich": [
      {
        "Name": "WithSensitiveDataMasking",
        "Args": {
          "options": {
            "MaskValue": "CUSTOM_MASK_FROM_JSON",
            "ExcludeProperties": [
              "email"
            ],
            "Mode": "Globally"
          }
        }
      }
    ]
  }
}
```
