using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Redbridge.Exceptions;
using Redbridge.Validation;

namespace Redbridge.WebApiCore.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute
    {
        readonly ILogger _logger;

        public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public override void OnException(ExceptionContext actionExecutedContext)
        {
            // Convert ValidationResultsException results or a ValidationException into ValidationResults
            _logger.LogInformation("Checking exception for validation exception filtering....");

            if (actionExecutedContext.Exception is ValidationResultsException ||
                actionExecutedContext.Exception is ValidationException)
            {
                ValidationResult[] results = Array.Empty<ValidationResult>();

                if (actionExecutedContext.Exception is ValidationResultsException validationResultsException)
                {
                    _logger.LogInformation(
                        "Validation exception filtering being applied to a multi-results exception...");
                    results = validationResultsException.Results?.Results != null
                        ? validationResultsException.Results.Results.ToArray()
                        : new[] { new ValidationResult(false, validationResultsException.Message) };
                }
                else if (actionExecutedContext.Exception is ValidationException validationException)
                {
                    _logger.LogInformation(
                        "Validation exception filtering being applied to a single result validation exception...");
                    results = new[] { new ValidationResult(false, validationException.Message) };
                }

                _logger.LogInformation("Serializing results into JSON for transmission...");
                var rawJson = JsonConvert.SerializeObject(results, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                actionExecutedContext.Result = new JsonResult(rawJson) { StatusCode = 422 };
                actionExecutedContext.ExceptionHandled = true;
            }
        }

    }
}
