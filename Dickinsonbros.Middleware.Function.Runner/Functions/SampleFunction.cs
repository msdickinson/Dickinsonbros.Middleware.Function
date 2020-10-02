using DickinsonBros.Logger.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using Dickinsonbros.Middleware.Function.Runner.Models.JWTService;
using Microsoft.Extensions.Logging;

namespace Dickinsonbros.Middleware.Function.Runner.Functions
{
    public class SampleFunction
    {
        #region constants

        internal const string USER = "User";
        internal const string FUNCTION_NAME = "SampleFunction";
        #endregion

        #region members
        internal readonly ILoggingService<SampleFunction> _loggingService;
        internal readonly IMiddlewareService<GeneralWebsite> _middlewareService;
        internal readonly ILogger<SampleFunction> _logger;
        #endregion

        #region .ctor
        public SampleFunction
        (
            ILoggingService<SampleFunction> loggingService,
            IMiddlewareService<GeneralWebsite> middlewareService,
            ILogger<SampleFunction> logger
        )
        {
            _loggingService = loggingService;
            _middlewareService = middlewareService;
            _logger = logger;
        }
        #endregion
        #region function
        [FunctionName(FUNCTION_NAME)]
        public async Task<ContentResult> RunAsync
        (
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "SampleFunctionWithAuth")] HttpRequest req
        )
        {
            return await _middlewareService.InvokeWithJWTAuthAsync
            (
                req.HttpContext,
                async () =>
                {
                    await Task.CompletedTask.ConfigureAwait(false);

                    return new ContentResult
                    {
                        StatusCode = 200,
                        Content = "{}",
                        ContentType = "application/json"
                    };
                },
                USER
            ).ConfigureAwait(false);
        }

        #endregion
    }
}