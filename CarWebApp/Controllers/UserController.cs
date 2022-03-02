using System.Threading.Tasks;
using CarWebApp.Models;
using CarWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWebApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            string key = await _service.GetJWT(model);

            Response.Cookies.Append("Authorization", key);
            
            return RedirectToAction("Index", "Home");
        } 
    }
}