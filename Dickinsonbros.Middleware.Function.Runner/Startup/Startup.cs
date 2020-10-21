using Dickinsonbros.Middleware.Function.Extensions;
using Dickinsonbros.Middleware.Function.Runner.Models.JWTService;
using DickinsonBros.DateTime.Extensions;
using DickinsonBros.Encryption.Certificate.Extensions;
using DickinsonBros.Encryption.JWT.Extensions;
using DickinsonBros.Guid.Extensions;
using DickinsonBros.Logger.Extensions;
using DickinsonBros.Redactor.Extensions;
using DickinsonBros.Stopwatch.Extensions;
using DickinsonBros.Telemetry.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.IO;

[assembly: WebJobsStartup(typeof(Dickinsonbros.Middleware.Function.Runner.Startup.Startup))]
namespace Dickinsonbros.Middleware.Function.Runner.Startup
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        const string _siteRootPath = @"\home\site\wwwroot\";
        const string FUNCTION_ENVIRONMENT_NAME = "FUNCTION_ENVIRONMENT_NAME";
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = EnrichConfiguration(builder.Services);
            ConfigureServices(builder.Services, configuration);
        }
        private IConfiguration EnrichConfiguration(IServiceCollection serviceCollection)
        {
            var existingConfiguration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddConfiguration(existingConfiguration);
            var configTransform = $"appsettings.{System.Environment.GetEnvironmentVariable(FUNCTION_ENVIRONMENT_NAME)}.json";
            var isCICD = !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), configTransform));
            var functionConfigurationRootPath = isCICD ? _siteRootPath : Directory.GetCurrentDirectory();
            var config =
                configurationBuilder
                .SetBasePath(functionConfigurationRootPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile(configTransform, false)
                .Build();
            serviceCollection.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), config));

            return config;
        }
        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddLogging(loggingBuilder =>
            {
                var logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                   .Enrich.FromLogContext()
                   .CreateLogger();

                loggingBuilder.AddSerilog
                (
                    logger,
                    dispose: true
                );

                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            });

            services.AddLoggingService();
            services.AddRedactorService();
            services.AddDateTimeService();
            services.AddStopwatchService();
            services.AddGuidService();
            services.AddTelemetryService();
            services.AddConfigurationEncryptionService();
            services.AddJWTService<GeneralWebsite>();

            //#Local Packages
            services.AddMiddlwareService<GeneralWebsite>(configuration);
        }

    }
}

