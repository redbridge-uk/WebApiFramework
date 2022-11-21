using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Redbridge.Exceptions;

namespace Redbridge.WebApi.Filters
{
    public class UserNotAuthenticatedExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UserNotAuthenticatedException)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionExecutedContext.Response = responseMessage;
                actionExecutedContext.Exception = null;
            }
        }
    }
}
