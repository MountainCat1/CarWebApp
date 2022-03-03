using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;

namespace CarWebApp.Middleware
{
    public class AssignUserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public AssignUserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var user = await userService.GetUser(context.User);

            if (user != null)
            {
                context.Items.Add("Username", user.Username);
                context.Items.Add("UserRole", user.Role);
            }
            else
            {
                context.Items.Add("UserRole", UserRole.Guest);
            }
            
            await _next(context);
        }
    }
}