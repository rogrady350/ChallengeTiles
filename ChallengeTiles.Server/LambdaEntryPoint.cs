using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text.Json;

namespace ChallengeTiles.Server
{
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.Configure(app => { })
                   .ConfigureServices((context, services) => { });
        }

        public override async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandlerAsync(
            APIGatewayHttpApiV2ProxyRequest request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine("Lambda invoked by API Gateway.");

            if (request == null)
            {
                lambdaContext.Logger.LogLine("Error: Received NULL request from API Gateway.");
                return new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 400,
                    Body = "Bad Request: Null request received from API Gateway"
                };
            }

            lambdaContext.Logger.LogLine($"Received request path: {request.RawPath ?? "NULL"}");
            lambdaContext.Logger.LogLine($"Request Details: {JsonConvert.SerializeObject(request)}");

            return await base.FunctionHandlerAsync(request, lambdaContext);
        }
    }
}
