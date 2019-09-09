using Stripe;
using StripeExample.Web.Models.Subscription;
using StripeExample.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private IPlanService _planService;
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

        private ISubscriptionService _subscriptionService;
        public ISubscriptionService SubscriptionService
        {
            get
            {
                return _subscriptionService ?? new SubscriptionService();
            }
            private set
            {
                _subscriptionService = value;
            }
        }

        public SubscriptionController(IPlanService planService, ISubscriptionService subscriptionService)
        {
            _planService = planService;
            _subscriptionService = subscriptionService;
        }

        public SubscriptionController()
        {

        }


        // GET: Subscription
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel() { Plans = PlanService.List() };

            return View(viewModel);
        }

        public ActionResult Billing(int planId)
        {
            string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            var viewModel = new BillingViewModel() { Plan = PlanService.Find(planId), StripePublishableKey = stripePublishableKey };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Billing(BillingViewModel billingViewModel)
        {
            billingViewModel.Plan = PlanService.Find(billingViewModel.Plan.Id);

            try
            {
                SubscriptionService.Create(User.Identity.Name, billingViewModel.Plan, billingViewModel.StripeToken);
            }
            catch (StripeException stripeEx)
            {
                ModelState.AddModelError(string.Empty, stripeEx.Message);
                return View(billingViewModel);                
            }
            return RedirectToAction("Index", "Dashboard");
        }

    }
}