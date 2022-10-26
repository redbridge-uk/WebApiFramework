using Microsoft.AspNetCore.Mvc;
using Redbridge.ApiManagement;

namespace Redbridge.WebApiCore.Controllers
{
    public abstract class ApiFactoryController : ControllerBase
    {
        protected ApiFactoryController(IApiFactory apiFactory)
        {
            ApiFactory = apiFactory ?? throw new ArgumentNullException(nameof(apiFactory));
        }

        protected IApiFactory ApiFactory { get; }
    }
}
