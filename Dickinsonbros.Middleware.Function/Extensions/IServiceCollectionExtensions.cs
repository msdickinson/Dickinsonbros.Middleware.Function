using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;

namespace Dickinsonbros.Middleware.Function.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMiddlwareService<T>(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.TryAddSingleton(typeof(IMiddlewareService<T>), typeof(MiddlewareService<T>));
            serviceCollection.TryAddSingleton<IFunctionHelperService, FunctionHelperService>();
            serviceCollection.Configure<JsonSerializerOptions>(configuration.GetSection(nameof(JsonSerializerOptions)));

            return serviceCollection;
        }
    }
}
