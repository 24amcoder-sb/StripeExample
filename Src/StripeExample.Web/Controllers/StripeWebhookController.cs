using Stripe;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace StripeExample.Web.Controllers
{
    public class StripeWebhookController : Controller
    {
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

                default:
                    break;
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}