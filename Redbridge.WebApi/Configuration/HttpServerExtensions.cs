using System;
using System.Web.Http;
using System.Web.Http.Batch;
using Redbridge.WebApi.Handlers;

namespace Redbridge.WebApi.Configuration
{
    public static class HttpServerExtensions
    {
        public static void ConfigureHttpBatching(this HttpServer httpServer)
        {
            // Our internal compressing HTTP batch handler only supports Gzip/Deflate at the moment.
            httpServer.Configuration.Routes.MapHttpBatchRoute("batch", "$batch", new CompressingHttpBatchHandler(httpServer)
            {
                ExecutionOrder = BatchExecutionOrder.NonSequential
            });
        }
    }
}
