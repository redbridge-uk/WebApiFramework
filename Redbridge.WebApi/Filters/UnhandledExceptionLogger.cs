﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Microsoft.Extensions.Logging;

namespace Redbridge.WebApi.Filters
{
    public class UnhandledExceptionLogger : IExceptionLogger
    {
        private readonly ILogger _logger;

        public UnhandledExceptionLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging (only) unhandled exceptions...");

            if (context.Exception != null)
                _logger.LogError(context.Exception, context.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
