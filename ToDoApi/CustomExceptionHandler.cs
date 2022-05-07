using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ToDoApi.Models;
using System.Net;

namespace ToDoApi
{
    public static class CustomExceptionHandler
    {
        public static void HandleException(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ctx.Response.ContentType = "application/json";

                //var exceptionHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();

                await ctx.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = ctx.Response.StatusCode,
                    Message = "Internal Server Error."
                }.ToString());
            });
        }
    }
}