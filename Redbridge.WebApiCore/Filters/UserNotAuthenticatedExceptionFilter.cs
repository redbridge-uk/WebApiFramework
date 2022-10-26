using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Exceptions;

namespace Redbridge.WebApiCore.Filters
{
    public class UserNotAuthenticatedExceptionFilter : ExceptionFilterAttribute
    {
        readonly ILogger _logger;

        public UserNotAuthenticatedExceptionFilter(ILogger<UserNotAuthenticatedExceptionFilter> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UserNotAuthenticatedException exception)
            {
                _logger.LogDebug("Processing user not authenticated exception filtering.");
                actionExecutedContext.Result = new UnauthorizedResult();
                actionExecutedContext.ExceptionHandled = true;
            }
        }
    }
}
