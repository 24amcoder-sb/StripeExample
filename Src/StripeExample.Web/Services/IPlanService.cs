using System.Collections.Generic;
using StripeExample.Web.Data;

namespace StripeExample.Web.Services
{
    public interface IPlanService
    {
        IList<Plan> List();
    }
}