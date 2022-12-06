using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Data;
using Redbridge.Diagnostics;

namespace Redbridge.WebApi.Filters
{
    public class UnknownEntityExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UnknownEntityExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UnknownEntityException unknownEntityException)
            {
                _logger.LogInformation($"Unknown entity exception processing with message {unknownEntityException.Message}");

                var errorMessageError = new HttpError(unknownEntityException.Message);
                // Only a single result issue.
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse((HttpStatusCode)422, errorMessageError);
                actionExecutedContext.Response.ReasonPhrase = string.Join(",", unknownEntityException.Message.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
                actionExecutedContext.Exception = null;
            }
        }
    }
}
