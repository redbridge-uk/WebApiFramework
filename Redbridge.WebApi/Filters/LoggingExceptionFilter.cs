using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Redbridge.Diagnostics;

namespace Redbridge.WebApi.Filters
{
    public class LoggingExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public LoggingExceptionFilter(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                _logger.WriteInfo($"Logging that an exception has occurred in LoggingExceptionFilter: {actionExecutedContext.Exception.Message}...");
                var messagePhrase = actionExecutedContext.Exception.Message ?? "Internal server error - no additional detail supplied";
                messagePhrase = string.Join(",", messagePhrase.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Where(s => !string.IsNullOrWhiteSpace(s))); // Carriage returns are not permitted in reason phrases.

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    RequestMessage = actionExecutedContext.Request,
                    ReasonPhrase = messagePhrase
                };

                actionExecutedContext.Response = response;
                _logger.WriteException(actionExecutedContext.Exception);
            }
        }
    }
}
