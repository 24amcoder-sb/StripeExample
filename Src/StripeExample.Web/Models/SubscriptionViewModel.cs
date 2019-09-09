using StripeExample.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StripeExample.Web.Models.Subscription
{
    public class IndexViewModel
    {
        public IList<Plan> Plans { get; set; }
    }

    public class BillingViewModel
    {
        public Plan Plan { get; set; }
        public string StripePublishableKey { get; set; }
        public string StripeToken { get; set; }
    }
}