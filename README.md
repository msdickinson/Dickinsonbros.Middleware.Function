# Dickinsonbros.Middleware.Function
<a href="https://www.nuget.org/packages/DickinsonBros.Middleware.Function/">
    <img src="https://img.shields.io/nuget/v/DickinsonBros.Middleware.Function">
</a>

Middleware for ASP.Net

Features

* Logs requests redacted
* Logs responses redacted and statuscode
* Adds Telemetry
* Handles correlation Ids
* Catch all uncaught exceptions and log them redacted

<a href="https://dev.azure.com/marksamdickinson/DickinsonBros/_build?definitionScope=%5CDickinsonBros.Middleware.Function">Builds</a>

<h2>Example Usage</h2>

```csharp
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
```  
  
<h2>Example Ouput</h2>

        + GET http://localhost:7071/api/SampleFunctionWithAuth
        Path: /api/SampleFunctionWithAuth
        Method: GET
        Scheme: http
        Prams: {}
        Body:
        Role: User
        NameIdentifier: 1924
        CorrelationId: 5b2213dd-8a93-4f73-9d54-fd84e93069ef

        Response GET http://localhost:7071/api/SampleFunctionWithAuth
        ContentType: application/json
        Body: {}
        StatusCode: 200
        CorrelationId: 5b2213dd-8a93-4f73-9d54-fd84e93069ef

        - GET http://localhost:7071/api/SampleFunctionWithAuth
        ElapsedMilliseconds: 29
        CorrelationId: 5b2213dd-8a93-4f73-9d54-fd84e93069ef


![Alt text](https://raw.githubusercontent.com/msdickinson/DickinsonBros.Middleware/develop/TelemetryAPISample.PNG)

Note: Logs can be redacted via configuration (see https://github.com/msdickinson/DickinsonBros.Redactor)

Telemetry generated when using DickinsonBros.Telemetry and connecting it to a configured database for ITelemetry See https://github.com/msdickinson/DickinsonBros.Telemetry on how to configure DickinsonBros.Telemetry and setup the database.

Example Runner Included in folder "DickinsonBros.Middleware.Function.Runner"


