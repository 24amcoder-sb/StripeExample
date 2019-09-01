/*
 * COM_001 - To resolve ambiguity between
 * 
 * ~/Models/ 
 *         /ManageViewModels.IndexViewModel     and,
 *         /Book.IndexViewModel 
 */

//namespace StripeExample.Web.Models -> COM_001

namespace StripeExample.Web.Models.Book
{
    public class IndexViewModel
    {
        public string StripePublishableKey { get; set; }
    }

    public class ChargeViewModel
    {
        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }
    }

    public class CustomViewModel
    {
        public string StripePublishableKey { get; set; }
        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }
        public bool PaymentFormHidden { get; set; }
        public string PaymentFormHiddenCss
        {
            get
            {
                return PaymentFormHidden ? "hidden" : "";
            }
        }
    }
}