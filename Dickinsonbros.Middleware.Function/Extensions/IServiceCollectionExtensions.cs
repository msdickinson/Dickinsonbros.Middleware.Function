using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dickinsonbros.Middleware.Function.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMiddlwareService<T>(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton(typeof(IMiddlewareService<T>), typeof(MiddlewareService<T>));
            return serviceCollection;
        }
    }
}
