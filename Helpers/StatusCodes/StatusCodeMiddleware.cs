using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Helpers.StatusCodes
{
    public class StatusCodeMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public StatusCodeMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _requestDelegate(httpContext);

            var response = httpContext.Response;

            if (!response.Headers.IsReadOnly)
            {
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.ContentType = Application.Json;
                    await response.WriteAsync(JsonSerializer.Serialize("Unauthenticated."));
                }
                else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    response.ContentType = Application.Json;
                    await response.WriteAsync(JsonSerializer.Serialize("Access denied."));
                }
            }
        }
    }
}
