using Stripe;
using System.Net.Mail;

namespace StripeExample.Web.Services
{
    public class EmailService
    {
        public void SendRefundEmail(StripeCharge stripeCharge)
        {
            var message = new MailMessage() { IsBodyHtml = true };
            message.To.Add(new MailAddress("RECIPIENT@outlook.com"));
            message.Subject = string.Format("refund requested on charge: {0}", stripeCharge.Id);
            message.Body = string.Format("<p>{0}</p>", string.Format("A customer at this email address {0} was issued a refund on their purchase: '{1}'.  Please follow up to determine a reason.", stripeCharge.ReceiptEmail, stripeCharge.Description));

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
    }
}