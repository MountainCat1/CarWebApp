using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace CarWebApp.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IUserService _userService;

        public AuthenticationFilter(IUserService userService)
        {
            _userService = userService;
        }
        
        

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is

            if (hasAllowAnonymous)
                return;

            var user = _userService.GetUser(context.HttpContext.User).Result;

            if (user is not {Role: UserRole.Admin})
            {
                HandleUnauthorizedRequest(context);
            }
        }

        private void HandleUnauthorizedRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    {"controller", "User"},
                    {"action", "Login"}
                });
        }
    }
}