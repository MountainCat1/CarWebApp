using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarWebApp.Middleware
{
    public class AuthorizationHeaderMiddleware
    {
        RequestDelegate next;

        public AuthorizationHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Cookies["Authorization"];

            if(!context.Request.Headers.ContainsKey("Authorization"))
                context.Request.Headers.Add("Authorization", $"Bearer {token}");

            await next(context);
        }
    }
}