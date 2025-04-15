using System.Net;

namespace ChallengeTiles.Server
{
    public class ExceptionMiddleware
    {
        //class for ASP.NET global exception handling (eliminates need for try/catch blocks in every API request)
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        //constructor
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;     //next middleware (next delegate to be invoked) in ASP.NET pipeline
            _logger = logger; //ASP.NET logging service
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); //calls next middlware in pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); //logs error

                //error responses (broadly returns 400 for now)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                //sends response to client
                var response = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
