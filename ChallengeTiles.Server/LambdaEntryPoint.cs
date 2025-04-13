using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ChallengeTiles.Server
{
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            Console.WriteLine("Initializing LambdaEntryPoint...");

            //Register services for Lambda's ASP.NET host
            builder.ConfigureServices((context, services) =>
            {
                //Register AWS Hosting integrtion
                services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
                Console.WriteLine("AWS Lambda Hosting configured.");

                //Reuse ServiceConfig for rest of builder services
                ServiceConfigurator.ConfigureAppServices(services);
            });

            builder.Configure(app =>
            {
                Console.WriteLine("Configuring Middleware for Lambda API Gateway requests...");

                app.UseRouting();
                app.UseCors("AllowFrontend");

                //Handle preflight OPTIONS requests
                app.Use(async (context, next) =>
                {
                    if (context.Request.Method == "OPTIONS")
                    {
                        context.Response.StatusCode = 204;
                        await context.Response.CompleteAsync();
                    }
                    else
                    {
                        await next();
                    }
                });

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseEndpoints(endpoints => endpoints.MapControllers());

                Console.WriteLine("All API routes mapped.");
            });
        }

        public override async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandlerAsync(
            APIGatewayHttpApiV2ProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine("Received request from API Gateway");

            var response = await base.FunctionHandlerAsync(request, lambdaContext);

            //header debug
            lambdaContext.Logger.LogLine("Request headers received:");
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    lambdaContext.Logger.LogLine($"{header.Key}: {header.Value}");
                }
            }

            //Dynamic CORS logic
            var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? Array.Empty<string>();

            //debug
            var requestOrigin = request.Headers
                .FirstOrDefault(h => string.Equals(h.Key, "origin", StringComparison.OrdinalIgnoreCase)).Value;

            lambdaContext.Logger.LogLine("ALLOWED_ORIGINS: " + string.Join(", ", allowedOrigins));
            lambdaContext.Logger.LogLine($"Request origin: {requestOrigin}");
            lambdaContext.Logger.LogLine($"Origin match: {allowedOrigins.Contains(requestOrigin)}");

            response.Headers ??= new Dictionary<string, string>();

            /* commented out for testing
            if (!string.IsNullOrEmpty(requestOrigin) && allowedOrigins.Contains(requestOrigin))
            {
                response.Headers["Access-Control-Allow-Origin"] = requestOrigin;
                response.Headers["Access-Control-Allow-Credentials"] = "true";
                response.Headers["Access-Control-Allow-Methods"] = "GET,POST,OPTIONS";
                response.Headers["Access-Control-Allow-Headers"] = "Content-Type";
            };*/

            response.Headers["Access-Control-Allow-Origin"] = "https://d3hjdy7rno6k48.cloudfront.net";
            response.Headers["Access-Control-Allow-Credentials"] = "true";
            response.Headers["Access-Control-Allow-Methods"] = "GET,POST,OPTIONS";
            response.Headers["Access-Control-Allow-Headers"] = "Content-Type";

            return response;
        }
    }
}
