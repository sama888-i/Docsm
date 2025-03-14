using Newtonsoft.Json;

namespace Docsm.Exceptions
{
    public class CustomExceptionMiddleware
    {
            private readonly RequestDelegate _next;

            public CustomExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext httpContext)
            {
                try
                {
                    await _next(httpContext);
                }
                catch (ExistException ex)
                {
                    await HandleExceptionAsync(httpContext, ex.StatusCode, ex.ErrorMessage);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(httpContext, 500, ex.Message); 
                }
            }

            private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
            {
                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "text/plain";
              // context.Response.ContentType = "application/json";
              // var result = JsonConvert.SerializeObject(new { message });
                 await context.Response.WriteAsync(message); 
            }
       

    }
}
