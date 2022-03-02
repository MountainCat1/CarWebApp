using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarWebApp.Entities;
using CarWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarWebApp.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICarModelService _carModelService;
        private readonly IUserService _userService;

        public CarController(ICarService carService, ICarModelService carModelService, IUserService userService)
        {
            _carService = carService;
            _carModelService = carModelService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            User user = await _userService.GetUser(User);
            ViewBag.Authorized = user is {RoleId: 1};
            
            var model = await _carService.GetAll();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var carModels = await _carModelService.GetAll();
            var comboBoxList = carModels
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            
            ViewBag.CarModelId = comboBoxList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Car model)
        {
            await _carService.Add(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var carModels = await _carModelService.GetAll();
            var comboBoxList = carModels
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            
            ViewBag.CarModelId = comboBoxList;
            
            return View();
        }
    }
}