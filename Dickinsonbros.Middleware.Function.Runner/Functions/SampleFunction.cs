using DickinsonBros.Logger.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using Dickinsonbros.Middleware.Function.Runner.Models.JWTService;

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
        
        #endregion

        #region .ctor
        public SampleFunction
        (
            ILoggingService<SampleFunction> loggingService,
            IMiddlewareService<GeneralWebsite> middlewareService
        )
        {
            _loggingService = loggingService;
            _middlewareService = middlewareService;
        }
        #endregion
        #region function
        [FunctionName(FUNCTION_NAME)]
        public async Task<IActionResult> RunAsync
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
            );
        }

        #endregion
    }
}





//var transactionId = Guid.NewGuid().ToString();
//var methodIdentifier = $"{nameof(TestAutomationFunction)}.{nameof(TestAutomationFunction.RunAsync)}";
//try
//{
//    _loggingService.LogInformationRedacted
//    (
//        $"+ {methodIdentifier}",
//        new Dictionary<string, object>()
//        {
//                        { nameof(transactionId), transactionId }
//        }
//    );

//    var tests = new List<Test>();
//    tests.AddRange(_integrationTestService.SetupTests(_accountAPITests));
//    tests.AddRange(_integrationTestService.SetupTests(_coasterAPITests));
//    tests.AddRange(_integrationTestService.SetupTests(_administrationAPITests));

//    //Test Group Filter
//    if (req != null && req.Query["TestGroup"].Any())
//    {
//        tests = tests.Where(e => e.TestGroup == req.Query["TestGroup"].First()).ToList();
//    }

//    //Addtional Filter
//    //tests = tests.Where(e => e.MethodInfo.Name.Contains("FetchCoasterbyTokenAsync_VaildAuthAndCoasterExist_Return200")).ToList();

//    //Run Tests
//    var testSummary = await _integrationTestService.RunTests(tests).ConfigureAwait(false);

//    //Process Tests
//    var reportTRX = _integrationTestService.GenerateTRXReport(testSummary);
//    var log = _integrationTestService.GenerateLog(testSummary, false);
//    var zipBytes = await _integrationTestService.GenerateZip(reportTRX, log).ConfigureAwait(false);

//    //Return Results
//    _loggingService.LogInformationRedacted(log);

//    return new ContentResult
//    {
//        Content = log,
//        ContentType = "text",
//        StatusCode = 200
//    };

//    //return new FileContentResult(zipBytes.ToArray(), ZIP_CONTENT_TYPE)
//    //{
//    //    FileDownloadName = $"Report {DateTime.Now:MM/dd/yyyy h:mm tt}.zip"
//    //};
//}
//catch (Exception exception)
//{
//    _loggingService.LogErrorRedacted
//    (
//        methodIdentifier,
//        exception,
//        new Dictionary<string, object>()
//        {
//                        { nameof(transactionId), transactionId }
//        }
//    );
//    var result = new ObjectResult("");
//    result.StatusCode = StatusCodes.Status500InternalServerError;
//    return result;
//}
//finally
//{
//    _loggingService.LogInformationRedacted
//    (
//        $"- {methodIdentifier}",
//        new Dictionary<string, object>()
//        {
//                        { nameof(transactionId), transactionId }
//        }
//    );
//}