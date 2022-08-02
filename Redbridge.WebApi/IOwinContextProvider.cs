using Microsoft.Owin;

namespace Redbridge.WebApi
{
    public interface IOwinContextProvider
    {
        IOwinContext Current { get; }
    }
}