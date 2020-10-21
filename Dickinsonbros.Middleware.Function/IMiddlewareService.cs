using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function
{
    public interface IMiddlewareService<T>
    {
        Task<ContentResult> InvokeAsync(HttpContext context, Func<Task<ContentResult>> callback);
        Task<ContentResult> InvokeWithJWTAuthAsync(HttpContext context, Func<ClaimsPrincipal, Task<ContentResult>> callback, params string[] roles);
    }
}