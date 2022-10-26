using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Data;

namespace Redbridge.WebApiCore.Filters
{
    public class UnknownEntityExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public UnknownEntityExceptionFilter(ILogger<UnknownEntityExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UnknownEntityException unknownEntityException)
            {
                _logger.LogInformation($"Unknown entity exception processing with message {unknownEntityException.Message}");
                var errorMessageError = new { error = unknownEntityException.Message};
                actionExecutedContext.Result = new JsonResult(errorMessageError) { StatusCode = 422 };
            }
        }
    }
}
