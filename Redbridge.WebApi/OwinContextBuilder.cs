using System.Collections.Generic;
using Microsoft.Owin;

namespace Redbridge.WebApi
{
    public class OwinContextBuilder : IOwinContextProvider
    {
        public OwinContextBuilder()
        {
            var environment = new Dictionary<string, object>();
            Current = new OwinContext(environment);
        }

        public IOwinContext Current { get; }
    }
}