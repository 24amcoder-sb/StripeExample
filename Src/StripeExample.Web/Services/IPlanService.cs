using System.Collections.Generic;
using StripeExample.Web.Data;

namespace StripeExample.Web.Services
{
    public interface IPlanService
    {
        Plan Find(int id);
        IList<Plan> List();
    }
}