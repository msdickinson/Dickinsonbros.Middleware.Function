using DickinsonBros.DateTime.Abstractions;
using DickinsonBros.Encryption.JWT.Abstractions;
using DickinsonBros.Encryption.JWT.Abstractions.Models;
using DickinsonBros.Guid.Abstractions;
using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Stopwatch.Abstractions;
using DickinsonBros.Telemetry.Abstractions;
using DickinsonBros.Telemetry.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function
{
    public class MiddlewareService<T> : IMiddlewareService<T>
    {
        internal const string CORRELATION_ID = "X-Correlation-ID";
        internal const string TOKEN_TYPE = "Bearer";

        internal readonly IServiceProvider _serviceProvider;
        internal readonly IDateTimeService _dateTimeService;
        internal readonly ITelemetryService _telemetryService;
        internal readonly IGuidService _guidService;
        internal readonly ILoggingService<MiddlewareService<T>> _loggingService;
        internal readonly ICorrelationService _correlationService;
        internal readonly IJWTService<T> _jwtService;
        
        public MiddlewareService(
            IServiceProvider serviceProvider,
            IDateTimeService dateTimeService,
            ITelemetryService telemetryService,
            IGuidService guidService,
            ICorrelationService correlationService,
            ILoggingService<MiddlewareService<T>> loggingService,
            IJWTService<T> jwtService
        )
        {
            _guidService = guidService;
            _loggingService = loggingService;
            _correlationService = correlationService;
            _serviceProvider = serviceProvider;
            _dateTimeService = dateTimeService;
            _telemetryService = telemetryService;
            _jwtService = jwtService;
        }

        public async Task<ContentResult> InvokeAsync(HttpContext context, Func<Task<ContentResult>> callback)
        {
            return await InvokeAsync(context, callback, false).ConfigureAwait(false);
        }

        public async Task<ContentResult> InvokeWithJWTAuthAsync(HttpContext context, Func<Task<ContentResult>> callback, params string[] roles)
        {
            return await InvokeAsync(context, callback, true, roles).ConfigureAwait(false);
        }

        internal async Task<ContentResult> InvokeAsync(HttpContext context, Func<Task<ContentResult>> callback, bool withAuth, params string[] roles)
        {
            var contentResult = (ContentResult)null;
            var telemetryData = new TelemetryData
            {
                DateTime = _dateTimeService.GetDateTimeUTC(),
                Name = $"{context.Request.Method} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                TelemetryType = TelemetryType.API
            };
            var stopwatchService = _serviceProvider.GetRequiredService<IStopwatchService>();
            stopwatchService.Start();
            _correlationService.CorrelationId = EnsureCorrelationId(context.Request);

            var requestBody = await FormatRequestAsync(context.Request);



            try
            {
                context.Response.Headers.TryAdd
                (
                    CORRELATION_ID,
                   _correlationService.CorrelationId
                );

                bool vaildAuth = !withAuth;
                var role = (string)null;
                var nameIdentifier = (string)null;
                if (withAuth)
                {
                    string token = context.Request.Headers.FirstOrDefault(header => header.Key == "Authorization").Value.ToString().Split("Bearer").LastOrDefault().Trim();
                    var accessTokenClaims = _jwtService.GetPrincipal(token, true, TokenType.Access);

                    if
                    (
                        accessTokenClaims == null ||
                        !accessTokenClaims.Identity.IsAuthenticated ||
                        !accessTokenClaims.Claims.Any(claim => claim.Type == ClaimTypes.Role && roles.Any(role => role.ToString() == claim.Value))
                    )
                    {
                        contentResult = new ContentResult
                        {
                            StatusCode = 401
                        };
                    }
                    else
                    {
                        role = accessTokenClaims.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
                        nameIdentifier = accessTokenClaims.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                        vaildAuth = true;
                    }
                }

                if (context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    _loggingService.LogInformationRedacted
                    (
                        $"+ {telemetryData.Name}",
                        new Dictionary<string, object>
                        {
                            { "Path", context.Request.Path.Value },
                            { "Method", context.Request.Method },
                            { "Scheme", context.Request.Scheme },
                            { "Prams", context.Request.Query.ToDictionary() },
                            { "Body", requestBody },
                            { "Role", role},
                            { "NameIdentifier",  nameIdentifier}
                        }
                    );
                }

                if (vaildAuth)
                {
                    contentResult = await callback().ConfigureAwait(false);
                }
         
                stopwatchService.Stop();

                if (context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    _loggingService.LogInformationRedacted
                    (
                        $"Response {telemetryData.Name}",
                        new Dictionary<string, object>
                        {
                            { "ContentType", contentResult.ContentType },
                            { "Body", contentResult.Content },
                            { "StatusCode", contentResult.StatusCode }
                        }
                    );
                }

                if (contentResult.StatusCode >= 200 && contentResult.StatusCode < 300)
                {
                    telemetryData.TelemetryState = TelemetryState.Successful;
                }
                else if (contentResult.StatusCode >= 400 && contentResult.StatusCode < 500)
                {
                    telemetryData.TelemetryState = TelemetryState.BadRequest;
                }
                else
                {
                    telemetryData.TelemetryState = TelemetryState.Failed;
                }

                return contentResult;
            }
            catch (Exception exception)
            {
                contentResult = new ContentResult
                {
                    StatusCode = 500
                };
                telemetryData.TelemetryState = TelemetryState.Failed;
                stopwatchService.Stop();

                _loggingService.LogErrorRedacted
                (
                    $"Unhandled exception {telemetryData.Name}",
                    exception,
                    new Dictionary<string, object>
                    {
                        { "StatusCode", contentResult.StatusCode }
                    }
                );

                return contentResult;
            }
            finally
            {
                telemetryData.ElapsedMilliseconds = (int)stopwatchService.ElapsedMilliseconds;

                if (context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
                {
                    _loggingService.LogInformationRedacted
                    (
                        $"- {telemetryData.Name}",
                        new Dictionary<string, object>
                        {
                        { "ElapsedMilliseconds", telemetryData.ElapsedMilliseconds }
                        }
                    );
                }

                _telemetryService.Insert(telemetryData);
            }
        }


        internal string EnsureCorrelationId(HttpRequest request)
        {
            if (!request.Headers.Any(e => e.Key == CORRELATION_ID))
            {
                return _guidService.NewGuid().ToString();
            }

            return request.Headers.First(e => e.Key == CORRELATION_ID).Value;
        }

        internal async Task<string> FormatRequestAsync(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            var body = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return body;
        }

        
    }
}
