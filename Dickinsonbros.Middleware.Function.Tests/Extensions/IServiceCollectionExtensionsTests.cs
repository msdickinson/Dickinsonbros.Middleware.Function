using Dickinsonbros.Middleware.Function.Extensions;
using DickinsonBros.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.Json;

namespace Dickinsonbros.Middleware.Function.Tests.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests : BaseTest
    {
        public class SampleClass {};

        [TestMethod]
        public void AddMiddlwareService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            var jsonSerializerOptions = new JsonSerializerOptions();
            var configurationRoot = BuildConfigurationRoot(jsonSerializerOptions);

            // Act
            serviceCollection.AddMiddlwareService<SampleClass>(configurationRoot);

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IMiddlewareService<SampleClass>) &&
                                           serviceDefinition.ImplementationType == typeof(MiddlewareService<SampleClass>) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

        }
    }
}
