using Dickinsonbros.Middleware.Function.Extensions;
using DickinsonBros.DateTime.Abstractions;
using DickinsonBros.Encryption.JWT.Abstractions;
using DickinsonBros.Encryption.JWT.Abstractions.Models;
using DickinsonBros.Guid.Abstractions;
using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Stopwatch.Abstractions;
using DickinsonBros.Telemetry.Abstractions;
using DickinsonBros.Telemetry.Abstractions.Models;
using DickinsonBros.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function.Tests
{
    [TestClass]
    public class MiddlewareServiceTests : BaseTest
    {

        #region InvokeAsnyc

        [TestMethod]
        public async Task InvokeAsync_Runs_GetDateTimeUTCCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService

                    //--IGuidService


                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    dateTimeServiceMock.Verify
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC(),
                        Times.Once
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_Runs_StopwatchStartCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                    .Setup
                    (
                        stopwatchService => stopwatchService.Start()
                    );

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    stopwatchServiceMock.Verify
                    (
                        stopwatchService => stopwatchService.Start(),
                        Times.Once
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_Runs_NewGuidCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IGuidService
                    var guidExpected = new System.Guid("10000000-0000-0000-0000-000000000000");
                    var guidServiceMock = serviceProvider.GetMock<IGuidService>();
                    guidServiceMock
                    .Setup
                    (
                        guidService => guidService.NewGuid()
                    )
                   .Returns(() => guidExpected);

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    guidServiceMock.Verify
                    (
                        guidService => guidService.NewGuid(),
                        Times.Once
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_RequestPathContainsAPI_LogRequest()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 1)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            "+ " + expectedURL,
                            It.IsAny<IDictionary<string, object>>()
                        )
                    );

                    Assert.AreEqual(requestPath, propertiesObserved["Path"]);
                    Assert.AreEqual("B", ((Dictionary<string, StringValues>)propertiesObserved["Prams"])["A"].ToString());
                    Assert.AreEqual(method, propertiesObserved["Method"]);
                    Assert.AreEqual(scheme, propertiesObserved["Scheme"]);
                    Assert.AreEqual(requestBody, propertiesObserved["Body"]);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_RequestPathDoesNotContainsAPI_RequestNotLogged()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 1)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            "+ " + expectedURL,
                            It.IsAny<IDictionary<string, object>>()
                        ),
                        Times.Never
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_Runs_CorrelationIdAddedToResponseHeader()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             
                    var guidExpected = new System.Guid("10000000-0000-0000-0000-000000000000");
                    var guidServiceMock = serviceProvider.GetMock<IGuidService>();
                    guidServiceMock
                    .Setup
                    (
                        guidService => guidService.NewGuid()
                    )
                   .Returns(() => guidExpected);

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(guidExpected.ToString(), headerDictionaryResponse[MiddlewareService<SampleClass>.CORRELATION_ID].ToString());
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_WithAuthNullAccessTokenClaims_StatusCodeReturns401()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResultForCallBack = new ContentResult
                    {

                    };
                    Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultForCallBack;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback, null,true, new string[] { "User" }).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(401, contentResult.StatusCode);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_WithAuthIsNotAuthenticated_StatusCodeReturns401()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();
                    var bearerToken = "1234";

                    var headers = new Dictionary<string, string>()
                    {
                        { "Authorization", $"Bearer {bearerToken}" }
                    };

                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "User")
                    };
                    var accessTokenIsAuthenticated = false;
                    var accessIdentityMock = new Mock<IIdentity>();

                    accessIdentityMock
                    .SetupGet(accessIdentity => accessIdentity.IsAuthenticated)
                    .Returns(accessTokenIsAuthenticated);

                    var accessClaimsPrincipalMock = new Mock<ClaimsPrincipal>();

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Identity)
                    .Returns(accessIdentityMock.Object);

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Claims)
                    .Returns(accessClaims);

                    var jwtServiceMock = serviceProvider.GetMock<IJWTService<SampleClass>>();
                    jwtServiceMock
                    .Setup
                    (
                        jwtService => jwtService.GetPrincipal
                        (
                            bearerToken,
                            true,
                            TokenType.Access
                        )
                    )
                   .Returns(() => accessClaimsPrincipalMock.Object);

                    //--Callback
                    var contentResultForCallBack = new ContentResult
                    {

                    };
                    Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultForCallBack;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback, null, true, new string[] { "User" }).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(401, contentResult.StatusCode);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_WithAuthAuthenticatedWithNoMatchingRole_StatusCodeReturns401()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();
                    var bearerToken = "1234";

                    var headers = new Dictionary<string, string>()
                    {
                        { "Authorization", $"Bearer {bearerToken}" }
                    };

                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "DifferntUserType")
                    };
                    var accessTokenIsAuthenticated = true;
                    var accessIdentityMock = new Mock<IIdentity>();

                    accessIdentityMock
                    .SetupGet(accessIdentity => accessIdentity.IsAuthenticated)
                    .Returns(accessTokenIsAuthenticated);

                    var accessClaimsPrincipalMock = new Mock<ClaimsPrincipal>();

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Identity)
                    .Returns(accessIdentityMock.Object);

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Claims)
                    .Returns(accessClaims);

                    var jwtServiceMock = serviceProvider.GetMock<IJWTService<SampleClass>>();
                    jwtServiceMock
                    .Setup
                    (
                        jwtService => jwtService.GetPrincipal
                        (
                            bearerToken,
                            true,
                            TokenType.Access
                        )
                    )
                   .Returns(() => accessClaimsPrincipalMock.Object);

                    //--Callback
                    var contentResultForCallBack = new ContentResult
                    {

                    };
                    Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultForCallBack;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback, null, true, new string[] { "User" }).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(401, contentResult.StatusCode);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_WithAuthAuthenticatedWithAMatchingRole_StatusCodeReturnsNon401()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();
                    var bearerToken = "1234";

                    var headers = new Dictionary<string, string>()
                    {
                        { "Authorization", $"Bearer {bearerToken}" }
                    };

                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.NameIdentifier, "SampleUser")
                    };
                    var accessTokenIsAuthenticated = true;
                    var accessIdentityMock = new Mock<IIdentity>();

                    accessIdentityMock
                    .SetupGet(accessIdentity => accessIdentity.IsAuthenticated)
                    .Returns(accessTokenIsAuthenticated);

                    var accessClaimsPrincipalMock = new Mock<ClaimsPrincipal>();

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Identity)
                    .Returns(accessIdentityMock.Object);

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Claims)
                    .Returns(accessClaims);

                    var jwtServiceMock = serviceProvider.GetMock<IJWTService<SampleClass>>();
                    jwtServiceMock
                    .Setup
                    (
                        jwtService => jwtService.GetPrincipal
                        (
                            bearerToken,
                            true,
                            TokenType.Access
                        )
                    )
                   .Returns(() => accessClaimsPrincipalMock.Object);

                    //--Callback
                    var contentResultForCallBack = new ContentResult
                    {
                        StatusCode = 200
                    };
                    Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultForCallBack;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback, null, true, new string[] { "User" }).ConfigureAwait(false);

                    //Assert
                    Assert.AreNotEqual(401, contentResult.StatusCode);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_Runs_StopwatchStopCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                    .Setup
                    (
                        stopwatchService => stopwatchService.Stop()
                    );

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    stopwatchServiceMock.Verify
                    (
                        stopwatchService => stopwatchService.Stop(),
                        Times.Once
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_NoAuthFailures_ReturnsContentResultFromCallback()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();
                    var bearerToken = "1234";

                    var headers = new Dictionary<string, string>()
                    {
                        { "Authorization", $"Bearer {bearerToken}" }
                    };

                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.NameIdentifier, "SampleUser")
                    };
                    var accessTokenIsAuthenticated = true;
                    var accessIdentityMock = new Mock<IIdentity>();

                    accessIdentityMock
                    .SetupGet(accessIdentity => accessIdentity.IsAuthenticated)
                    .Returns(accessTokenIsAuthenticated);

                    var accessClaimsPrincipalMock = new Mock<ClaimsPrincipal>();

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Identity)
                    .Returns(accessIdentityMock.Object);

                    accessClaimsPrincipalMock
                    .SetupGet(accessClaimsPrincipal => accessClaimsPrincipal.Claims)
                    .Returns(accessClaims);

                    var jwtServiceMock = serviceProvider.GetMock<IJWTService<SampleClass>>();
                    jwtServiceMock
                    .Setup
                    (
                        jwtService => jwtService.GetPrincipal
                        (
                            bearerToken,
                            true,
                            TokenType.Access
                        )
                    )
                   .Returns(() => accessClaimsPrincipalMock.Object);

                    //--Callback
                    var contentResultForCallBack = new ContentResult
                    {
                        StatusCode = 200
                    };
                    Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultForCallBack;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback, null, true, new string[] { "User" }).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(contentResultForCallBack, contentResult);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }


        [TestMethod]
        public async Task InvokeAsync_RequestPathContainsAPI_LogResponse()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 2)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResultFromCallback = new ContentResult
                    {
                        Content = "{}",
                        ContentType = "Json",
                        StatusCode = 200
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultFromCallback;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            "Response " + expectedURL,
                            It.IsAny<IDictionary<string, object>>()
                        )
                    );

                    Assert.AreEqual(contentResultFromCallback.ContentType, propertiesObserved["ContentType"]);
                    Assert.AreEqual(contentResultFromCallback.StatusCode, propertiesObserved["StatusCode"]);
                    Assert.AreEqual(contentResultFromCallback.Content, propertiesObserved["Body"]);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_RequestPathDoesNotContainsAPI_ResponseNotLogged()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup


                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 2)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            $"Response {expectedURL}",
                            It.IsAny<IDictionary<string, object>>()
                        ),
                        Times.Never
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_RequestPathContainsAPI_LogExitWithElapsedMilliseconds()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 3)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResultFromCallback = new ContentResult
                    {
                        Content = "{}",
                        ContentType = "Json",
                        StatusCode = 200
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultFromCallback;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            "- " + expectedURL,
                            It.IsAny<IDictionary<string, object>>()
                        )
                    );

                    Assert.AreEqual(elapsedMillisecondsExpected, propertiesObserved["ElapsedMilliseconds"]);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_RequestPathDoesNotContainsAPI_LogExitNotLogged()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var logInformationRedactedInvokeCount = 0;
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, IDictionary<string, object>>((message, properties) =>
                    {
                        logInformationRedactedInvokeCount++;
                        if (logInformationRedactedInvokeCount == 3)
                        {
                            propertiesObserved = properties;
                        }
                    });

                    //--IJWTService

                    //--Callback
                    var contentResultFromCallback = new ContentResult
                    {
                        Content = "{}",
                        ContentType = "Json",
                        StatusCode = 200
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResultFromCallback;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogInformationRedacted
                        (
                            "- " + expectedURL,
                            It.IsAny<IDictionary<string, object>>()
                        ),
                        Times.Never
                    );

                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_StatusCodeLessThen200_AddsFailedTelemetry()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 199 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService
                    var telemetryDataObserved = (TelemetryData)null;
                    var telemetryServiceMock = serviceProvider.GetMock<ITelemetryService>();
                    telemetryServiceMock
                        .Setup(telemetryService => telemetryService.Insert(It.IsAny<TelemetryData>()))
                        .Callback<TelemetryData>((telemetryData) =>
                        {
                            telemetryDataObserved = telemetryData;
                        });

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {
                        StatusCode = 199
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    telemetryServiceMock.Verify
                    (
                        telemetryService => telemetryService.Insert
                        (
                            It.IsAny<TelemetryData>()
                        ),
                        Times.Once
                    );

                    Assert.AreEqual(datetimeExpected, telemetryDataObserved.DateTime);
                    Assert.AreEqual(elapsedMillisecondsExpected, telemetryDataObserved.ElapsedMilliseconds);
                    Assert.AreEqual(expectedURL, telemetryDataObserved.Name);
                    Assert.AreEqual(TelemetryState.Failed, telemetryDataObserved.TelemetryState);
                    Assert.AreEqual(TelemetryType.API, telemetryDataObserved.TelemetryType);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_StatusCode2xx_AddsSuccessfulTelemetry()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService
                    var telemetryDataObserved = (TelemetryData)null;
                    var telemetryServiceMock = serviceProvider.GetMock<ITelemetryService>();
                    telemetryServiceMock
                        .Setup(telemetryService => telemetryService.Insert(It.IsAny<TelemetryData>()))
                        .Callback<TelemetryData>((telemetryData) =>
                        {
                            telemetryDataObserved = telemetryData;
                        });

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {
                        StatusCode = 200
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    telemetryServiceMock.Verify
                    (
                        telemetryService => telemetryService.Insert
                        (
                            It.IsAny<TelemetryData>()
                        ),
                        Times.Once
                    );

                    Assert.AreEqual(datetimeExpected, telemetryDataObserved.DateTime);
                    Assert.AreEqual(elapsedMillisecondsExpected, telemetryDataObserved.ElapsedMilliseconds);
                    Assert.AreEqual(expectedURL, telemetryDataObserved.Name);
                    Assert.AreEqual(TelemetryState.Successful, telemetryDataObserved.TelemetryState);
                    Assert.AreEqual(TelemetryType.API, telemetryDataObserved.TelemetryType);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_StatusCode201_AddsSuccessfulTelemetry()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 201 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService
                    var telemetryDataObserved = (TelemetryData)null;
                    var telemetryServiceMock = serviceProvider.GetMock<ITelemetryService>();
                    telemetryServiceMock
                        .Setup(telemetryService => telemetryService.Insert(It.IsAny<TelemetryData>()))
                        .Callback<TelemetryData>((telemetryData) =>
                        {
                            telemetryDataObserved = telemetryData;
                        });

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {
                        StatusCode = 201
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    telemetryServiceMock.Verify
                    (
                        telemetryService => telemetryService.Insert
                        (
                            It.IsAny<TelemetryData>()
                        ),
                        Times.Once
                    );

                    Assert.AreEqual(datetimeExpected, telemetryDataObserved.DateTime);
                    Assert.AreEqual(elapsedMillisecondsExpected, telemetryDataObserved.ElapsedMilliseconds);
                    Assert.AreEqual(expectedURL, telemetryDataObserved.Name);
                    Assert.AreEqual(TelemetryState.Successful, telemetryDataObserved.TelemetryState);
                    Assert.AreEqual(TelemetryType.API, telemetryDataObserved.TelemetryType);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_StatusCode4xx_AddsBadRequestTelemetry()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 400 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService
                    var telemetryDataObserved = (TelemetryData)null;
                    var telemetryServiceMock = serviceProvider.GetMock<ITelemetryService>();
                    telemetryServiceMock
                        .Setup(telemetryService => telemetryService.Insert(It.IsAny<TelemetryData>()))
                        .Callback<TelemetryData>((telemetryData) =>
                        {
                            telemetryDataObserved = telemetryData;
                        });

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {
                        StatusCode = 400
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    telemetryServiceMock.Verify
                    (
                        telemetryService => telemetryService.Insert
                        (
                            It.IsAny<TelemetryData>()
                        ),
                        Times.Once
                    );

                    Assert.AreEqual(datetimeExpected, telemetryDataObserved.DateTime);
                    Assert.AreEqual(elapsedMillisecondsExpected, telemetryDataObserved.ElapsedMilliseconds);
                    Assert.AreEqual(expectedURL, telemetryDataObserved.Name);
                    Assert.AreEqual(TelemetryState.BadRequest, telemetryDataObserved.TelemetryState);
                    Assert.AreEqual(TelemetryType.API, telemetryDataObserved.TelemetryType);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_StatusCode500OrMore_AddsBadRequestTelemetry()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 500 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService
                    var telemetryDataObserved = (TelemetryData)null;
                    var telemetryServiceMock = serviceProvider.GetMock<ITelemetryService>();
                    telemetryServiceMock
                        .Setup(telemetryService => telemetryService.Insert(It.IsAny<TelemetryData>()))
                        .Callback<TelemetryData>((telemetryData) =>
                        {
                            telemetryDataObserved = telemetryData;
                        });

                    //--IStopwatchService
                    var elapsedMillisecondsExpected = 100;
                    var stopwatchServiceMock = serviceProvider.GetMock<IStopwatchService>();
                    stopwatchServiceMock
                        .Setup(stopwatchService => stopwatchService.ElapsedMilliseconds)
                        .Returns(elapsedMillisecondsExpected);

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {
                        StatusCode = 500
                    };
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    telemetryServiceMock.Verify
                    (
                        telemetryService => telemetryService.Insert
                        (
                            It.IsAny<TelemetryData>()
                        ),
                        Times.Once
                    );

                    Assert.AreEqual(datetimeExpected, telemetryDataObserved.DateTime);
                    Assert.AreEqual(elapsedMillisecondsExpected, telemetryDataObserved.ElapsedMilliseconds);
                    Assert.AreEqual(expectedURL, telemetryDataObserved.Name);
                    Assert.AreEqual(TelemetryState.Failed, telemetryDataObserved.TelemetryState);
                    Assert.AreEqual(TelemetryType.API, telemetryDataObserved.TelemetryType);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_CatchInvokedByException_LogErrorRedacted()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 500 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    var exception = new Exception("CallBackThrows");
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        throw exception;
                    };

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogErrorRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<Exception>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, Exception, IDictionary<string, object>>((message, exception, properties) =>
                    {
                        propertiesObserved = properties;
                    });
                    //--IJWTService

                    //--Callback

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    loggingServiceMock.Verify
                    (
                        loggingService => loggingService.LogErrorRedacted
                        (
                            $"Unhandled exception {expectedURL}",
                            exception,
                            It.IsAny<IDictionary<string, object>>()
                        )
                    );

                    Assert.AreEqual(500, propertiesObserved["StatusCode"]);
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        [TestMethod]
        public async Task InvokeAsync_CatchInvokedByException_StatusCodeReturns500()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 500 };
                    var requestPath = "/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    var exception = new Exception("CallBackThrows");
                    Func<Task<ContentResult>> callback = async () =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        throw exception;
                    };

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService

                    //--ITelemetryService

                    //--IStopwatchService

                    //--IGuidService             

                    //--ICorrelationService

                    //--ILoggingService
                    var propertiesObserved = (IDictionary<string, object>)null;
                    var loggingServiceMock = serviceProvider.GetMock<ILoggingService<MiddlewareService<SampleClass>>>();
                    loggingServiceMock.Setup
                    (
                        loggingService => loggingService.LogErrorRedacted
                        (
                            It.IsAny<string>(),
                            It.IsAny<Exception>(),
                            It.IsAny<IDictionary<string, object>>()
                        )
                    )
                    .Callback<string, Exception, IDictionary<string, object>>((message, exception, properties) =>
                    {
                        propertiesObserved = properties;
                    });
                    //--IJWTService

                    //--Callback

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var contentResult = await uutConcrete.InvokeAsync(httpContextMock.Object, callback).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(500, contentResult.StatusCode);

                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }
        #endregion

        #region InvokeWithJWTAuthAsync

        [TestMethod]
        public async Task InvokeWithJWTAuthAsync_Runs_GetDateTimeUTCCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    //--Context
                    var httpContextMock = new Mock<HttpContext>();
                    var httpRequestMock = new Mock<HttpRequest>();
                    var HttpResponseMock = new Mock<HttpResponse>();

                    var headers = new Dictionary<string, string>();
                    var headerDictionaryResponse = new HeaderDictionary();
                    StatusCodeTestClass responseStatusCode = new StatusCodeTestClass { StatusCode = 200 };
                    var requestPath = "/API/Test";
                    var requestBody = "Test Body";
                    var requestQuery = new QueryCollection
                    (
                        new Dictionary<string, StringValues>
                        {
                            {"A", "B" }
                        }
                    );
                    var method = "GET";
                    var scheme = "https";
                    var host = "testhost";
                    var expectedURL = $"{method} {scheme}://{host}{requestPath}";
                    ConfigureHttpContext
                    (
                        httpContextMock,
                        httpRequestMock,
                        HttpResponseMock,
                        headers,
                        method,
                        scheme,
                        host,
                        requestBody,
                        responseStatusCode,
                        requestPath,
                        requestQuery,
                        headerDictionaryResponse
                    );

                    //--Callback

                    //--WithAuth

                    //--Roles

                    //--IDateTimeService
                    var datetimeExpected = new System.DateTime(2020, 1, 1);
                    var dateTimeServiceMock = serviceProvider.GetMock<IDateTimeService>();
                    dateTimeServiceMock
                    .Setup
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC()
                    )
                   .Returns(() => datetimeExpected);

                    //--ITelemetryService

                    //--IGuidService


                    //--ICorrelationService

                    //--ILoggingService

                    //--IJWTService

                    //--Callback
                    var contentResult = new ContentResult
                    {

                    };
                   Func<ClaimsPrincipal, Task<ContentResult>> callback = async (user) =>
                    {
                        await Task.CompletedTask.ConfigureAwait(false);
                        return contentResult;
                    };

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    await uutConcrete.InvokeWithJWTAuthAsync(httpContextMock.Object, callback, new string[] { }).ConfigureAwait(false);

                    //Assert
                    dateTimeServiceMock.Verify
                    (
                        dateTimeService => dateTimeService.GetDateTimeUTC(),
                        Times.Once
                    );
                },
              serviceCollection => ConfigureServices(serviceCollection)
          );
        }

        #endregion

        #region EnsureCorrelationId
        [TestMethod]
        public async Task EnsureCorrelationId_ContainsCorrelationIdInHeader_ReturnsCorrelationIdFromHeader()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    await Task.CompletedTask;

                    var expected = "1000";
                    var httpRequestMock = new Mock<HttpRequest>();
                    httpRequestMock
                       .SetupGet(httpRequest => httpRequest.Headers)
                       .Returns(() =>
                       {
                           var headerDictionary = new HeaderDictionary
                           {
                               { MiddlewareService<SampleClass>.CORRELATION_ID, expected }
                           };

                           return headerDictionary;
                       });

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var observed = uutConcrete.EnsureCorrelationId(httpRequestMock.Object);

                    //Assert
                    Assert.AreEqual(expected, observed);
                },
                serviceCollection => ConfigureServices(serviceCollection)
            );
        }

        [TestMethod]
        public async Task EnsureCorrelationId_DoesNotContainsCorrelationIdInHeader_ReturnsNewGuid()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    await Task.CompletedTask;

                    //  Guid 
                    var guidExpected = new System.Guid("10000000-0000-0000-0000-000000000000");
                    var guidServiceMock = serviceProvider.GetMock<IGuidService>();
                    guidServiceMock
                        .Setup(guidService => guidService.NewGuid())
                        .Returns(() => guidExpected);

                    //  HttpRequest
                    var httpRequestMock = new Mock<HttpRequest>();
                    httpRequestMock
                       .SetupGet(httpRequest => httpRequest.Headers)
                       .Returns(() =>
                       {
                           var headerDictionary = new HeaderDictionary
                           {
                           };

                           return headerDictionary;
                       });

                    //--UUT
                    var uut = serviceProvider.GetRequiredService<IMiddlewareService<SampleClass>>();
                    var uutConcrete = (MiddlewareService<SampleClass>)uut;

                    //Act
                    var observed = uutConcrete.EnsureCorrelationId(httpRequestMock.Object);

                    //Assert
                    Assert.AreEqual(guidExpected.ToString(), observed);
                    guidServiceMock
                        .Verify(guidService => guidService.NewGuid());

                },
                serviceCollection => ConfigureServices(serviceCollection)
            );
        }
        #endregion



        #region Helpers

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            var configurationRoot = BuildConfigurationRoot(jsonSerializerOptions);

            serviceCollection.AddMiddlwareService<SampleClass>(configurationRoot);
            serviceCollection.AddSingleton(Mock.Of<ITelemetryService>());
            serviceCollection.AddSingleton(Mock.Of<IDateTimeService>());
            serviceCollection.AddSingleton(Mock.Of<IStopwatchService>());
            serviceCollection.AddSingleton(Mock.Of<IGuidService>());
            serviceCollection.AddSingleton(Mock.Of<IJWTService<SampleClass>>());
            serviceCollection.AddSingleton(Mock.Of<ILoggingService<MiddlewareService<SampleClass>>>());
            serviceCollection.AddSingleton(Mock.Of<ICorrelationService>());

            return serviceCollection;
        }

        public class SampleClass { };

        public class StatusCodeTestClass
        {
            public int StatusCode { get; set; }
        }

        private void ConfigureHttpContext
        (
             Mock<HttpContext> httpContextMock,
             Mock<HttpRequest> httpRequestMock,
             Mock<HttpResponse> httpResponseMock,
             Dictionary<string, string> headers,
             string requestMethod,
             string requestScheme,
             string requestHost,
             string requestBody,
             StatusCodeTestClass responseStatusCode,
             string requestPath,
             QueryCollection RequestQuery,
             HeaderDictionary ResponseHeaders
        )
        {
            byte[] byteArrayRequest = Encoding.UTF8.GetBytes(requestBody);
            MemoryStream requestBodyStream = new MemoryStream(byteArrayRequest);

            Stream ResponseStream = new MemoryStream();

            httpContextMock
                .SetupGet(httpContext => httpContext.Request)
                .Returns(() => httpRequestMock.Object);

            httpContextMock
                .SetupGet(httpContext => httpContext.Response)
                .Returns(() => httpResponseMock.Object);

            httpContextMock
                .SetupGet(httpContext => httpContext.Request)
                .Returns(() => httpRequestMock.Object);

            httpRequestMock
                .SetupGet(httpRequestMock => httpRequestMock.Path)
                .Returns(() => requestPath);

            httpRequestMock
            .SetupGet(httpRequestMock => httpRequestMock.Method)
            .Returns(() => requestMethod);

            httpRequestMock
            .SetupGet(httpRequestMock => httpRequestMock.Scheme)
            .Returns(() => requestScheme);


            httpRequestMock
            .SetupGet(httpRequestMock => httpRequestMock.Host)
            .Returns(() => new HostString(requestHost));

            httpRequestMock
              .SetupGet(httpRequestMock => httpRequestMock.ContentLength)
              .Returns(() => byteArrayRequest.Length);

            httpRequestMock
                .SetupGet(httpRequestMock => httpRequestMock.Query)
                .Returns(() => RequestQuery);

            httpRequestMock
                .SetupGet(httpRequest => httpRequest.Body)
                .Returns(() => requestBodyStream);

            httpRequestMock
                .SetupGet(httpRequest => httpRequest.Headers)
                .Returns(() =>
                {
                    var headerDictionary = new HeaderDictionary();

                    foreach (var header in headers)
                    {
                        headerDictionary.Add(header.Key, header.Value);
                    }
                    return headerDictionary;
                });

            httpResponseMock
                .SetupGet(HttpResponse => HttpResponse.Body)
                .Returns(() => ResponseStream);

            httpResponseMock
                .SetupSet(HttpResponse => HttpResponse.Body = It.IsAny<Stream>())
                .Callback<Stream>(stream => ResponseStream = stream);

            httpResponseMock
                .SetupGet(HttpResponse => HttpResponse.Headers)
                .Returns(() => ResponseHeaders);

            httpResponseMock
                .SetupGet(HttpResponse => HttpResponse.StatusCode)
                .Returns(() => responseStatusCode.StatusCode);

            httpResponseMock
                .SetupSet(HttpResponse => HttpResponse.StatusCode = It.IsAny<int>())
                .Callback<int>(statusCode => responseStatusCode.StatusCode = statusCode);
        }
        #endregion
    }
}
