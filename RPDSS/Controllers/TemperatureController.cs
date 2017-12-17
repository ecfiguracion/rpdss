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
    public class TemperatureController : Controller
    {
        private TemperatureRepository _model;

        public TemperatureController(IConfiguration config)
        {
            _model = new TemperatureRepository(config);
        }

        public IActionResult Index()
        {
            return View(_model.GetAll());
        }

        public IActionResult New()
        {
            var item = new TemperatureDataEntryModel();
            item.Calendar = _model.GetLookup();
            return View("Form", item);
        }

        public IActionResult Edit(int id)
        {
            var item = new TemperatureDataEntryModel
            {
                Temperature = _model.GetById(id),
                Calendar = _model.GetLookup()
            };
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

        public IActionResult Save(TemperatureDataEntryModel item)
        {
            if (item.Temperature.Id == 0)
                _model.New(item.Temperature);
            else
                _model.Update(item.Temperature);
            return View("Index", _model.GetAll());
        }
    }
}