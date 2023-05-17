namespace NKZSoft.Service.Configuration.Logger;

public static class ServiceCollectionExtensions
{
    private const string MicroserviceNameProperty = "MicroserviceName";

    /// <summary>
    /// Add Serilog to the logging pipeline.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing settings to be used.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
            .Enrich.WithCorrelationIdHeader(Constants.Headers.CorrelationId)
            .WriteTo.Console(formatProvider:CultureInfo.InvariantCulture)
            .CreateLogger();

        Log.Logger = logger;

        var loggerFactory = new LoggerFactory();
        loggerFactory.AddSerilog(logger);
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        return services;
    }
}
