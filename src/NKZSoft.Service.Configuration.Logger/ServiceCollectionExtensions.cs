namespace NKZSoft.Service.Configuration.Logger;

using NKZSoft.Service.Configuration.Logger;

public static class ServiceCollectionExtensions
{
    private const string MicroserviceNameProperty = "MicroserviceName";
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var serviceName = Assembly.GetExecutingAssembly().GetName().Name;

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty(MicroserviceNameProperty, serviceName, true)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;

        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog(logger);
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        return services;
    }
}
