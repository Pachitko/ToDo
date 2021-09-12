using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;

namespace ToDoApi.ActionConstraints
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly MediaTypeCollection _mediaTypes = new();
        private readonly string _requestHeaderToMatch;

        public int Order => 0;

        public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch, string mediaType, params string[] otherMediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch 
                ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));

            if (MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
            {
                _mediaTypes.Add(parsedMediaType);
            }
            else
            {
                throw new ArgumentException(nameof(mediaType));
            }

            foreach (var otherMediaType in otherMediaTypes)
            {
                if(MediaTypeHeaderValue.TryParse(otherMediaType, out var parsedOtherMediaType))
                {
                    _mediaTypes.Add(parsedOtherMediaType);
                }
            }
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
                return false;

            var parsedRequestMediaType = new MediaType(requestHeaders[_requestHeaderToMatch]);

            foreach (var mediaType in _mediaTypes)
            {
                var parsedMediaType = new MediaType(mediaType);
                if(parsedRequestMediaType.Equals(parsedMediaType))
                {
                    return true;
                }
            }

            return false;

            //if (!context.RouteContext.HttpContext.Request.Headers.TryGetValue(_requestHeaderToMatch, out var headerValue))
            //    return false;

            //return _mediaTypes.Contains(headerValue);
        }
    }
}
