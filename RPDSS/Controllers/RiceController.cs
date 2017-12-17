using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPDSS.DataLayer.Repositories;
using Microsoft.Extensions.Configuration;
using RPDSS.DataLayer.Models;

namespace RPDSS.Controllers
{
    public class RiceController : Controller
    {
        private RiceRepository _model;

        public RiceController(IConfiguration config)
        {
            _model = new RiceRepository(config);
        }

        public IActionResult Index()
        {
            return View(_model.GetAll());
        }

        public IActionResult New()
        {
            var item = new RiceModel();
            return View("Form", item);
        }

        public IActionResult Edit(int id)
        {
            var item = _model.GetById(id);
            return View("Form", item);
        }

        public IActionResult Details(int id, bool isfordetails)
        {
            var item = _model.GetById(id);
            return View(isfordetails ? "Details" : "Delete", item);
        }

        public IActionResult Delete(int id)
        {
            _model.Delete(id);
            return View("Index", _model.GetAll());
        }

        public IActionResult Save(RiceModel item)
        {
            if (item.Id == 0)
                _model.New(item);
            else
                _model.Update(item);
            return View("Index", _model.GetAll());
        }
    }
}