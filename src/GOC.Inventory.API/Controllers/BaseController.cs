using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        protected ILogger Logger;

        protected UserInfo UserInfo { get; private set; }

        public BaseController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<T>();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims;
                var sub = identity.FindFirst("sub").Value;
                UserInfo = new UserInfo
                {
                    UserId = Guid.Parse(sub)
                };
            }
        }
    }
}
