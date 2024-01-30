using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University.Entities;
using University.ExtensionMethods;

namespace University.ActionFilters
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetObject<User>("loggedUser") == null)
                context.Result = new RedirectResult("/Home/Login");
            
        }
    }
}
