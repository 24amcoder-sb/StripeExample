using Stripe;
using StripeExample.Web.Data;

namespace StripeExample.Web.Services
{
    public interface ISubscriptionService
    {
        StripeCustomerService StripeCustomerService { get; }
        StripeSubscriptionService StripeSubscriptionService { get; }
        ApplicationUserManager UserManager { get; }

        void Create(string userName, Plan plan, string stripeToken);
    }
}