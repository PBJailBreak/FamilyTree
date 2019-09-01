using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace UI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            this.SetStatusCode(context, exception);

            return context.Response.WriteAsync(this.GetExceptionMessage(exception));
        }

        private void SetStatusCode(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case EntityNotFoundException enfe:
                    {
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
            }

            context.Response.StatusCode = statusCode;
        }

        private string GetExceptionMessage(Exception exception)
        {
            switch (exception)
            {
                case EntityNotFoundException enfe:
                    {
                        return exception.Message;
                    }

                default:
                    {
                        return "Internal server error";
                    }
            }
        }
    }
}
