using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ChallengeTiles.Server
{
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.Configure(app => { })
                   .ConfigureServices((context, services) => { });
        }
    }
}
