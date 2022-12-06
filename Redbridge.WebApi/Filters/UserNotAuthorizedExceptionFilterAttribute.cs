﻿using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.Extensions.Logging;
using Redbridge.Diagnostics;
using Redbridge.Exceptions;

namespace Redbridge.WebApi.Filters
{
    public class UserNotAuthorizedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        readonly ILogger _logger;

        public UserNotAuthorizedExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _logger.LogInformation("Checking exception for user not authorized exception filtering....");

            var exception = actionExecutedContext.Exception as UserNotAuthorizedException;

            if (exception != null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    RequestMessage = actionExecutedContext.Request,
                };

                actionExecutedContext.Response = response;
                actionExecutedContext.Exception = null;
            }
        }
    }
}
