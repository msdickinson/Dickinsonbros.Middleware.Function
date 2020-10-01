using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Dickinsonbros.Middleware.Function.Extensions;

namespace Dickinsonbros.Middleware.Function.Tests.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        public class SampleClass {};

        [TestMethod]
        public void AddMiddlwareService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddMiddlwareService<SampleClass>();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IMiddlewareService<SampleClass>) &&
                                           serviceDefinition.ImplementationType == typeof(MiddlewareService<SampleClass>) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }
    }
}
