
using Azure;
using DomainLayer.Exceptions;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate next,ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //Request

                await _next.Invoke(httpContext);

                //Response
                await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "Some Thing Went Wrong");



            //set status code
            // httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnAuthorizedException=> StatusCodes.Status401Unauthorized,
                BadRequestException=> StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            //set content type
            //httpContext.Response.ContentType = "application/json";

            var response = new ErrorToReturn
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message,
                Errors = ex switch
                {
                    BadRequestException badRequestException => badRequestException.Errors,
                    _ => []
                }
            };

            //var responseJson = System.Text.Json.JsonSerializer.Serialize(response);

            await httpContext.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                };
                await httpContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
