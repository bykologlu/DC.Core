using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Core.Api
{

	public class BaseController<TService> : BaseController
    {
        protected TService Service
        {
            get
            {
                TService? instance = HttpContext.RequestServices.GetService<TService>();

                if (instance == null) throw new NullReferenceException($"{nameof(TService)} not registered");

                return instance;
            }
        }
        

        public BaseController():base()
        {

        }
    }

    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
    }
}
