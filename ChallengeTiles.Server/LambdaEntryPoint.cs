﻿using Amazon.Lambda.APIGatewayEvents;
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

            builder.ConfigureServices((context, services) =>
            {
                // Ensure Lambda integration
                services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
                services.AddControllers();
            });

            builder.Configure(app =>
            {
                Console.WriteLine("Configuring Middleware for Lambda API Gateway requests...");

                app.UseRouting();
                app.UseCors("AllowFrontend");
                app.UseAuthorization();
                app.UseHttpsRedirection();
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseEndpoints(endpoints => endpoints.MapControllers());

                Console.WriteLine("All API routes mapped.");
            });
        }

        public override async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandlerAsync(
            APIGatewayHttpApiV2ProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine("Received request from API Gateway");

            if (request == null)
            {
                lambdaContext.Logger.LogLine("Received NULL request from API Gateway.");
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 400,
                    Body = JsonSerializer.Serialize(new { error = "Bad Request: Null request received from API Gateway" }),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }

            return await base.FunctionHandlerAsync(request, lambdaContext);
        }
    }
}
