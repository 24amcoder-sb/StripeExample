using Stripe;
using StripeExample.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace StripeExample.Web.Services
{
    public class PlanService
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

        public IList<Plan> List()
        {
            var plans = (from p in _db.Plans.Include("Features")
                         orderby p.DisplayOrder
                         select p).ToList();

            return plans;
        }
    }
}