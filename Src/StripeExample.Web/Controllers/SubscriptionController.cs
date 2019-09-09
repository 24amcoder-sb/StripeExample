using StripeExample.Web.Models.Subscription;
using StripeExample.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private IPlanService _planService;

        public SubscriptionController(IPlanService planService)
        {
            _planService = planService;
        }

        public SubscriptionController()
        {

        }

        public IPlanService PlanService
        {
            get
            {
                return _planService ?? new PlanService();
            }
            private set
            {
                _planService = value;
            }
        }


        // GET: Subscription
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel() { Plans = PlanService.List() };

            return View(viewModel);
        }
    }
}