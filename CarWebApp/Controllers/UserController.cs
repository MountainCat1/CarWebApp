using System;
using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Models;
using CarWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWebApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        private const string AuthorizationCookieName = "Authorization";
        
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> WhoAmI()
        {
            User user = await _service.GetUser(User);
            return Ok(user != null ? user.Username : "Not found" );
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            string key;
            try
            {
                key = await _service.GetJWT(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
            
            Response.Cookies.Append(AuthorizationCookieName, key);
            
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult LogoutConfirm()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(AuthorizationCookieName);

            return RedirectToAction("Index", "Home");
        }
    }
}