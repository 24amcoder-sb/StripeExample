using System.Configuration;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class BookController : Controller
    {
        // embedded form
        public ActionResult Index()
        {            
            return View();
        }

        //custom form
        public ActionResult Custom()
        {
            return View();
        }
    }
}