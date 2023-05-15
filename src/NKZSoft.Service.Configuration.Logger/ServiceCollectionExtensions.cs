namespace NKZSoft.Service.Configuration.Logger;

using System.Globalization;

public static class ServiceCollectionExtensions
{
    private const string MicroserviceNameProperty = "MicroserviceName";
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var serviceName = Assembly.GetCallingAssembly().GetName().Name!;

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty(MicroserviceNameProperty, serviceName, true)
            .Enrich.FromLogContext()
            .WriteTo.Console(formatProvider:CultureInfo.InvariantCulture)
            .CreateLogger();

        Log.Logger = logger;

        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog(logger);
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        return services;
    }
}
