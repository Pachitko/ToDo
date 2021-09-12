using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ToDoApi.Models.StatusCodeResults
{
    public class ResponseFailedResult : StatusCodeResult
    {
        public ResponseFailedResult([ActionResultStatusCode] int statusCode) : base(statusCode)
        {
        }
    }
}
