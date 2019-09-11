using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stripe;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class StripeWebhookController : Controller
    {
        private ApplicationUserManager userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        private StripeCustomerService customerService;
        public StripeCustomerService StripeCustomerService
        {
            get
            {
                return customerService ?? new StripeCustomerService();
            }
            private set
            {
                customerService = value;
            }
        }

        public StripeWebhookController() { }

        public StripeWebhookController(ApplicationUserManager userManager, StripeCustomerService customerService)
        {
            this.userManager = userManager;
            this.StripeCustomerService = customerService;
        }

        [HttpPost]
        public ActionResult Index()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();

            StripeEvent stripeEvent = null;

            try
            {
                stripeEvent = StripeEventUtility.ParseEvent(json);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Unable to parse incoming event. The following error occured: {0}", e.Message));
            }

            if(stripeEvent == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Incoming event empty");

            var emailService = new Services.EmailService();


            switch (stripeEvent.Type)
            {
                case StripeEvents.ChargeRefunded:
                    var charge = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendRefundEmail(charge);
                    break;
                case StripeEvents.CustomerSubscriptionTrialWillEnd:
                    var subscription = Mapper<StripeSubscription>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendTrialEndEmail(subscription);
                    break;
                case StripeEvents.InvoicePaymentSucceeded:
                    StripeInvoice invoice = Mapper<StripeInvoice>.MapFromJson(stripeEvent.Data.Object.ToString());
                    var customer = StripeCustomerService.Get(invoice.CustomerId);
                    var user = UserManager.FindByEmail(customer.Email);
                    user.ActiveUntil = user.ActiveUntil.AddMonths(1);
                    UserManager.Update(user);
                    emailService.SendSubscriptionPaymentReceiptEmail(invoice, customer);
                    break;
                default:
                    break;
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}