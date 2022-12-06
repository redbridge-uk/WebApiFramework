using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Extensions.Logging;
using Redbridge.Diagnostics;
using Redbridge.WebApi.Filters;
using Redbridge.WebApi.Handlers;

namespace Redbridge.WebApi.Configuration
{
    public static class HttpConfigurationFilterExtensions
    {
        public static void InstallExceptionFilters(this HttpConfiguration configuration, ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            logger.LogDebug("Installing exception filters...");
            configuration.Filters.Add(new ValidationExceptionFilterAttribute(logger));
            configuration.Filters.Add(new UnknownEntityExceptionFilterAttribute(logger));
            configuration.Filters.Add(new UserNotAuthenticatedExceptionFilterAttribute());
            configuration.Filters.Add(new UserNotAuthorizedExceptionFilterAttribute(logger));
            configuration.Filters.Add(new LoggingExceptionFilterAttribute(logger));
            configuration.MessageHandlers.Add(new NotFoundCustomMessageHandler());

            logger.LogDebug("Installing exception logger...");
            configuration.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger(logger));
        }
    }
}
