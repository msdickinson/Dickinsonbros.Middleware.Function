using Dickinsonbros.Middleware.Function.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function
{
    public interface IFunctionHelperService
    {
        Task<ProcessRequestDescriptor<T>> ProcessRequestAsync<T>(HttpRequest httpRequest) where T : class;
        ContentResult StatusCode(int statusCode);
        ContentResult StatusCode(int statusCode, string text);
        ContentResult StatusCode<T>(int statusCode, T data);
    }
}