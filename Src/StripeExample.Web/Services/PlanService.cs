using Stripe;
using StripeExample.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace StripeExample.Web.Services
{
    public class PlanService : IPlanService
    {
        private IPaymentsModel _db;
        private StripePlanService _stripePlanService;

        public PlanService(IPaymentsModel paymentsModel, StripePlanService stripePlanService)
        {
            _db = paymentsModel;
            _stripePlanService = stripePlanService;
        }

        public PlanService() :
            this(new PaymentsModel(), new StripePlanService())
        {

        }


        public Plan Find(int id)
        {
            var plan = (from p in _db.Plans.Include("Features")
                        where p.Id == id
                        select p).SingleOrDefault();

            var stripePlan = _stripePlanService.Get(plan.ExternalId);
            StripePlanToPlan(stripePlan, plan);

            return plan;
        }


        public IList<Plan> List()
        {
            var plans = (from p in _db.Plans.Include("Features")
                         orderby p.DisplayOrder
                         select p).ToList();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var stripePlans = (from p in _stripePlanService.List() select p).ToList();

            foreach (var plan in plans)
            {
                var stripePlan = stripePlans.Single(p => p.Id == plan.ExternalId);
                StripePlanToPlan(stripePlan, plan);
            }

            return plans;
        }

        private void StripePlanToPlan(StripePlan stripePlan, Plan plan)
        {
            plan.Name = stripePlan.Name;
            plan.AmountInCents = stripePlan.Amount;
            plan.Currency = stripePlan.Currency;
            plan.Interval = stripePlan.Interval;
            plan.TrialPeriodDays = stripePlan.TrialPeriodDays;
        }
    }
}