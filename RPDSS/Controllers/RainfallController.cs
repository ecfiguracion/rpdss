using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RPDSS.DataLayer.Repositories;
using RPDSS.DataLayer.Models;

namespace RPDSS.Controllers
{
    public class RainfallController : Controller
    {
        private RainfallRepository _model;

        public RainfallController(IConfiguration config)
        {
            _model = new RainfallRepository(config);
        }

        public IActionResult Index()
        {
            return View(_model.GetAll());
        }

        public IActionResult New()
        {
            var item = new RainfallDataEntryModel();
            item.Calendar = _model.GetLookup();
            return View("Form", item);
        }

        public IActionResult Edit(int id)
        {
            var item = new RainfallDataEntryModel
            {
                Rainfall = _model.GetById(id),
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

        public IActionResult Save(RainfallDataEntryModel item)
        {
            if (item.Rainfall.Id == 0)
                _model.New(item.Rainfall);
            else
                _model.Update(item.Rainfall);
            return View("Index", _model.GetAll());
        }
    }
}