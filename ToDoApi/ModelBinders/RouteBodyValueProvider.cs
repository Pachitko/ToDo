using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using System;

namespace ToDoApi.ModelBinders
{
    public class RouteBodyValueProvider : IValueProvider
    {
        private readonly HttpContext _httpContext;
        private readonly string _body;

        public RouteBodyValueProvider(HttpContext httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            
            using var reader = new StreamReader(httpContext.Request.Body);
            _body = reader.ReadToEnd();
        }

        public bool ContainsPrefix(string prefix)
        {
            return false;
        }

        public ValueProviderResult GetValue(string key)
        {
            ValueProviderResult result = new("Test", CultureInfo.CurrentCulture);
            return result;
        }
    }
}
