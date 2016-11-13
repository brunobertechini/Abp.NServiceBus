using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Abp.NServiceBus.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : NServiceBusControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}