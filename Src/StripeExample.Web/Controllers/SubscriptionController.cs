using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        // GET: Subscription
        public ActionResult Index()
        {
            return View();
        }
    }
}