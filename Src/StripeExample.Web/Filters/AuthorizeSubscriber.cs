using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.Mvc;

namespace StripeExample.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class AuthorizeSubscriber : FilterAttribute, IAuthorizationFilter
    {        
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(filterContext.HttpContext.User.Identity.Name);

            if(user == null || DateTime.Now > user.ActiveUntil)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}