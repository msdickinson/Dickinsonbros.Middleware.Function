using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Dickinsonbros.Middleware.Function.Models
{
    [ExcludeFromCodeCoverage]
    public class ProcessRequestDescriptor<T>
    {
        public bool IsSuccessful { get; set; }
        public ContentResult ContentResult { get; set; }
        public T Data { get; set; }
    }
}
