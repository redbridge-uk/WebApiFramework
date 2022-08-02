using System.Web;
using Microsoft.Owin;

namespace Redbridge.WebApi
{
    public class OwinContextProvider : IOwinContextProvider
    {
        public IOwinContext Current => HttpContext.Current?.GetOwinContext();
    }
}
