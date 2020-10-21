using DickinsonBros.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function.Tests
{
    [TestClass]
    public class FunctionHelperServiceTests : BaseTest
    {

        #region StatusCode

        [TestMethod]
        public async Task StatusCode_InputStatusCode_ReturnsTextContentResult()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var statuscode = 200;

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                    var uutConcrete = (FunctionHelperService)uut;

                    //Act
                    var observed = uutConcrete.StatusCode(statuscode);

                    //Assert
                    Assert.IsNotNull(observed);
                    Assert.AreEqual("", observed.Content);
                    Assert.AreEqual("text/html", observed.ContentType);
                    Assert.AreEqual(statuscode, observed.StatusCode);

                    await Task.CompletedTask.ConfigureAwait(false);
                },
                serviceCollection => ConfigureServices(serviceCollection)
             ).ConfigureAwait(false);
        }

        #endregion

        #region StatusCodeWithText

        [TestMethod]
        public async Task StatusCode_InputStatusCodeAndString_ReturnsTextContentResultWithString()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var statuscode = 200;
                    var text = "SampleText";

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                    var uutConcrete = (FunctionHelperService)uut;

                    //Act
                    var observed = uutConcrete.StatusCode(statuscode, text);

                    //Assert
                    Assert.IsNotNull(observed);
                    Assert.AreEqual(text, observed.Content);
                    Assert.AreEqual("text/html", observed.ContentType);
                    Assert.AreEqual(statuscode, observed.StatusCode);

                    await Task.CompletedTask.ConfigureAwait(false);
                },
                serviceCollection => ConfigureServices(serviceCollection)
             ).ConfigureAwait(false);
        }


        #endregion

        #region StatusCodeWithData

        [TestMethod]
        public async Task StatusCode_InputStatusCodeAndObject_ReturnsTextContentResultWithString()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var statuscode = 200;
                    var expectedContent = "{\"Name\":\"SampleName\",\"Age\":1}";
                    var sampleDataClass = new SampleDataClass
                    {
                        Age = 1,
                        Name = "SampleName"
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                    var uutConcrete = (FunctionHelperService)uut;

                    //Act
                    var observed = uutConcrete.StatusCode(statuscode, sampleDataClass);

                    //Assert
                    Assert.IsNotNull(observed);
                    Assert.AreEqual(expectedContent, observed.Content);
                    Assert.AreEqual("application/json", observed.ContentType);
                    Assert.AreEqual(statuscode, observed.StatusCode);

                    await Task.CompletedTask.ConfigureAwait(false);
                },
                serviceCollection => ConfigureServices(serviceCollection)
             ).ConfigureAwait(false);
        }

        #endregion

        #region ProcessRequest
        [TestMethod]
        public async Task ProcessRequestAsync_InvaildBody_ReturnBadRequest()
        {
            await RunDependencyInjectedTestAsync
            (
              async (serviceProvider) =>
              {
                  //Setup
                  var stream = new MemoryStream();

                  Mock<HttpRequest> httpRequestMock = new Mock<HttpRequest>();
                  httpRequestMock
                      .SetupGet(httpRequest => httpRequest.Body)
                      .Returns(stream);

                  //--UUT
                  var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                  var uutConcrete = (FunctionHelperService)uut;

                  //Act
                  var observed = await uutConcrete.ProcessRequestAsync<SampleDataClass>(httpRequestMock.Object).ConfigureAwait(false);

                  //Assert
                  Assert.IsNotNull(observed);
                  Assert.IsFalse(observed.IsSuccessful);
                  Assert.IsNull(observed.Data);
                  Assert.IsNotNull(observed.ContentResult);
                  Assert.AreEqual("", observed.ContentResult.Content);
                  Assert.AreEqual("text/html", observed.ContentResult.ContentType);
                  Assert.AreEqual(400, observed.ContentResult.StatusCode);
              },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task ProcessRequestAsync_ValidationFails_ReturnsBadRequestWithValidationResults()
        {
            await RunDependencyInjectedTestAsync
            (
              async (serviceProvider) =>
              {
                  //Setup
                  var expectedContentResult = "[{\"MemberNames\":[\"Name\"],\"ErrorMessage\":\"The Name field is required.\"}]";
                  byte[] byteArray = Encoding.ASCII.GetBytes("{}");
                  var stream = new MemoryStream(byteArray);

                  Mock<HttpRequest> httpRequestMock = new Mock<HttpRequest>();
                  httpRequestMock
                      .SetupGet(httpRequest => httpRequest.Body)
                      .Returns(stream);

                  //--UUT
                  var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                  var uutConcrete = (FunctionHelperService)uut;

                  //Act
                  var observed = await uutConcrete.ProcessRequestAsync<SampleDataClass>(httpRequestMock.Object).ConfigureAwait(false);

                  //Assert
                  Assert.IsNotNull(observed);
                  Assert.IsFalse(observed.IsSuccessful);
                  Assert.IsNotNull(observed.Data);
                  Assert.IsNull(observed.Data.Name);
                  Assert.AreEqual(0, observed.Data.Age);
                  Assert.IsNotNull(observed.ContentResult);
                  Assert.AreEqual(expectedContentResult, observed.ContentResult.Content);
                  Assert.AreEqual("application/json", observed.ContentResult.ContentType);
                  Assert.AreEqual(400, observed.ContentResult.StatusCode);
              },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task ProcessRequestAsync_VaildInput_ReturnsSuccesfulWithData()
        {
            await RunDependencyInjectedTestAsync
            (
              async (serviceProvider) =>
              {
                  //Setup
                  byte[] byteArray = Encoding.ASCII.GetBytes("{\"Name\":\"SampleName\"}");
                  var stream = new MemoryStream(byteArray);

                  Mock<HttpRequest> httpRequestMock = new Mock<HttpRequest>();
                  httpRequestMock
                      .SetupGet(httpRequest => httpRequest.Body)
                      .Returns(stream);

                  //--UUT
                  var uut = serviceProvider.GetRequiredService<IFunctionHelperService>();
                  var uutConcrete = (FunctionHelperService)uut;

                  //Act
                  var observed = await uutConcrete.ProcessRequestAsync<SampleDataClass>(httpRequestMock.Object).ConfigureAwait(false);

                  //Assert
                  Assert.IsNotNull(observed);
                  Assert.IsTrue(observed.IsSuccessful);
                  Assert.IsNotNull(observed.Data);
                  Assert.AreEqual("SampleName", observed.Data.Name);
                  Assert.AreEqual(0, observed.Data.Age);
                  Assert.IsNull(observed.ContentResult);
              },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }
        #endregion

        #region Helpers

        public class SampleDataClass
        {
            [Required]
            public string Name { get; set; }

            public int Age { get; set; }
        }

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            var configurationRoot = BuildConfigurationRoot(jsonSerializerOptions);
            serviceCollection.Configure<JsonSerializerOptions>(configurationRoot.GetSection(nameof(JsonSerializerOptions)));

            serviceCollection.AddSingleton<IFunctionHelperService, FunctionHelperService>();
            return serviceCollection;
        }

        #endregion
    }
}
