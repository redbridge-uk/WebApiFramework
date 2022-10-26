using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Redbridge.WebApiCore.Filters
{
    public class LoggingExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public LoggingExceptionFilter(ILogger<LoggingExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public override void OnException(ExceptionContext actionExecutedContext)
        {
            _logger.LogError(actionExecutedContext.Exception, actionExecutedContext.Exception.Message);
        }
    }
}
