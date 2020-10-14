# Dickinsonbros.Middleware.Function
<a href="https://dev.azure.com/marksamdickinson/dickinsonbros/_build/latest?definitionId=84&amp;branchName=master"> <img alt="Azure DevOps builds (branch)" src="https://img.shields.io/azure-devops/build/marksamdickinson/DickinsonBros/84/master"> </a> <a href="https://dev.azure.com/marksamdickinson/dickinsonbros/_build/latest?definitionId=84&amp;branchName=master"> <img alt="Azure DevOps coverage (branch)" src="https://img.shields.io/azure-devops/coverage/marksamdickinson/dickinsonbros/84/master"> </a><a href="https://dev.azure.com/marksamdickinson/DickinsonBros/_release?_a=releases&view=mine&definitionId=37"> <img alt="Azure DevOps releases" src="https://img.shields.io/azure-devops/release/marksamdickinson/b5a46403-83bb-4d18-987f-81b0483ef43e/37/38"> </a><a href="https://www.nuget.org/packages/DickinsonBros.Middleware.Function/"><img src="https://img.shields.io/nuget/v/DickinsonBros.Middleware.Function"></a>

Middleware for Azure Functions

Features

* Logs requests redacted (With "api" in route)
* Logs responses redacted and status codes (With "api" in route)
* Adds Telemetry
* Handles Auth
* Handles correlation Ids
* Catch all uncaught exceptions and log them redacted

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


[Sample Runner](https://github.com/msdickinson/DickinsonBros.Middleware.Function/tree/master/DickinsonBros.Middleware.Function.Runner)
