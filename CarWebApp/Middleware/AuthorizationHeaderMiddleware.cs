using System.Security.Claims;
using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Services;
using Microsoft.AspNetCore.Http;

namespace CarWebApp.Middleware
{
    public class AuthorizationHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizationHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Cookies["Authorization"];

            if(!context.Request.Headers.ContainsKey("Authorization"))
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            
            await _next(context);
        }
    }
}