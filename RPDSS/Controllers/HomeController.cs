﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPDSS.DataLayer.Models;
using Microsoft.Extensions.Configuration;
using RPDSS.DataLayer.Repositories;

namespace RPDSS.Controllers
{
    public class HomeController : Controller
    {
        private HomeRepository _model;

        public HomeController(IConfiguration config)
        {
            _model = new HomeRepository(config);
        }

        public IActionResult Index()
        {
            return View(_model.GetLookUps());
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Authenticate(LoginModel login)
        {
            if (_model.Authenticate(login))
                return RedirectToAction("Index", "Rice");
            else
            {
                ModelState.AddModelError("", "Invalid username or password. Please try again.");
                return View("Login");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
