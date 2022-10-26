using Microsoft.AspNetCore.Builder;
using Redbridge.Web;
using Redbridge.Web.Messaging;

namespace Redbridge.WebApiCore
{
    public static class QueryStringAuthenticationExtension
    {
        public static void UseQueryStringAuthentication(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.QueryString.HasValue)
                {
                    if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeader))
                    {
                        if (string.IsNullOrWhiteSpace(authorizationHeader))
                        {
                            var queryString = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                            if (queryString.ContainsKey(QueryStringParts.Authentication))
                            {
                                var token = queryString[QueryStringParts.Authentication];

                                if (!string.IsNullOrWhiteSpace(token))
                                {
                                    context.Request.Headers.Add(HeaderNames.Authorization,
                                        new[] { BearerTokenFormatter.CreateToken(token) });
                                }
                            }
                        }
                    }
                }

                try
                {
                    await next.Invoke();
                }
                catch (OperationCanceledException)
                {
                    // Do not propagate this exception.
                }
            });
        }
    }
}
