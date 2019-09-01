using StripeExample.Web.Models.Book;
using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class BookController : Controller
    {
        // embedded form
        public ActionResult Index()
        {
            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var model = new IndexViewModel() { StripePublishableKey = stripePublishableKey };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Charge(ChargeViewModel chargeViewModel)
        {
            Debug.WriteLine(chargeViewModel.StripeEmail);
            Debug.WriteLine(chargeViewModel.StripeToken);
            return RedirectToAction("Confirmation");
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        //custom form
        public ActionResult Custom()
        {
            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var model = new CustomViewModel() { StripePublishableKey = stripePublishableKey };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Custom(CustomViewModel customViewModel)
        {
            System.Diagnostics.Debug.WriteLine(customViewModel);
            return RedirectToAction("Confirmation");
        }
    }
}