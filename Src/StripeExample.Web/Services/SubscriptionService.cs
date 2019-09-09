using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe;
using StripeExample.Web;
using StripeExample.Web.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace StripeExample.Web.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private StripeCustomerService _stripeCustomerService;
        public StripeCustomerService StripeCustomerService
        {
            get
            {
                return _stripeCustomerService ?? new StripeCustomerService();
            }
            private set
            {
                _stripeCustomerService = value;
            }
        }


        private StripeSubscriptionService _stripeSubscriptionService;
        public StripeSubscriptionService StripeSubscriptionService
        {
            get
            {
                return _stripeSubscriptionService ?? new StripeSubscriptionService();
            }
            private set
            {
                _stripeSubscriptionService = value;
            }
        }

        public SubscriptionService()
        {

        }

        public SubscriptionService(ApplicationUserManager userManager, StripeCustomerService customerService, StripeSubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _stripeCustomerService = customerService;
            _stripeSubscriptionService = subscriptionService;
        }

        public void Create(string userName, Plan plan, string stripeToken)
        {
            //get User
            var user = UserManager.FindByName(userName);

            if (string.IsNullOrEmpty(user.StripeCustomerId))  //First Time customer
            {
                //create customer which will create subscription if plan is set and cc info via token is provided
                var customer = new StripeCustomerCreateOptions()
                {
                    Email = user.Email,
                    Source = new StripeSourceOptions() { TokenId = stripeToken },
                    PlanId = plan.ExternalId  //external id is Stripe plan id
                };

                StripeCustomer stripeCustomer = StripeCustomerService.Create(customer);

                //update user
                user.StripeCustomerId = stripeCustomer.Id;
                user.ActiveUntil = DateTime.Now.AddDays((double)plan.TrialPeriodDays);
                UserManager.Update(user);
            }
            else
            {
                //customer already exists, add subscription to customer
                var stripeSubscription = StripeSubscriptionService.Create(user.StripeCustomerId, plan.ExternalId);
                user.ActiveUntil = DateTime.Now.AddDays((double)plan.TrialPeriodDays);
                UserManager.Update(user);
            }

        }




    }
}