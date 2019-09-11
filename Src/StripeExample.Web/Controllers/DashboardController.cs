using StripeExample.Web.Filters;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    [AuthorizeSubscriber]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}