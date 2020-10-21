using Dickinsonbros.Middleware.Function.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dickinsonbros.Middleware.Function
{
    public class FunctionHelperService : IFunctionHelperService
    {
        internal readonly JsonSerializerOptions _jsonSerializerOptions;

        public FunctionHelperService
        (
            IOptions<JsonSerializerOptions> jsonSerializerOptions
        )
        {
            _jsonSerializerOptions = jsonSerializerOptions.Value;
        }

        public ContentResult StatusCode(int statusCode)
        {
            return new ContentResult
            {
                StatusCode = statusCode,
                Content = "",
                ContentType = "text/html"
            };
        }

        public ContentResult StatusCode(int statusCode, string text)
        {
            return new ContentResult
            {
                StatusCode = statusCode,
                Content = text,
                ContentType = "text/html"
            };
        }

        public ContentResult StatusCode<T>(int statusCode, T data)
        {
            return new ContentResult
            {
                StatusCode = statusCode,
                Content = JsonSerializer.Serialize(data, typeof(T),  _jsonSerializerOptions),
                ContentType = "json/html"
            };
        }

        public async Task<ProcessRequestDescriptor<T>> ProcessRequestAsync<T>(HttpRequest httpRequest) where T : class
        {
            var processRequestDescriptor = new ProcessRequestDescriptor<T>();
            try
            {
                processRequestDescriptor.Data = await JsonSerializer.DeserializeAsync<T>(httpRequest.Body, _jsonSerializerOptions).ConfigureAwait(false);
            }
            catch
            {
                processRequestDescriptor.IsSuccessful = false;
                processRequestDescriptor.ContentResult =
                    new ContentResult
                    {
                        StatusCode = 400,
                        Content = "",
                        ContentType = "text/html"
                    };

                return processRequestDescriptor;
            }

            var context = new ValidationContext(processRequestDescriptor.Data, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(processRequestDescriptor.Data, context, results, true))
            {
                processRequestDescriptor.IsSuccessful = false;
                processRequestDescriptor.ContentResult =
                    new ContentResult
                    {
                        StatusCode = 400,
                        Content = JsonSerializer.Serialize(results),
                        ContentType = "application/json"
                    };

                return processRequestDescriptor;
            }

            processRequestDescriptor.IsSuccessful = true;

            return processRequestDescriptor;
        }

    }
}
