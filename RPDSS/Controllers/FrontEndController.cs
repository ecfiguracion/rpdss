﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPDSS.DataLayer.Models;

namespace RPDSS.Controllers
{
    public class FrontEndController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}