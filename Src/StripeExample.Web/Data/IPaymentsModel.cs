using System.Data.Entity;

namespace StripeExample.Web.Data
{
    public interface IPaymentsModel
    {
        DbSet<Feature> Features { get; set; }
        DbSet<Plan> Plans { get; set; }
    }
}