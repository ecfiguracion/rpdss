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
    public class FuzzyParameterController : Controller
    {
        private FuzzyParameterRepository _model;

        public FuzzyParameterController(IConfiguration config)
        {
            _model = new FuzzyParameterRepository(config);
        }

        public IActionResult Index()
        {
            return View(_model.GetAll());
        }

        public IActionResult New()
        {
            var item = new FuzzyParameterDataEntryModel();
            item.GrowthStages = _model.GetLookup();
            return View("Form", item);
        }

        public IActionResult Edit(int id)
        {
            var item = new FuzzyParameterDataEntryModel
            {
                FuzzyParameter = _model.GetById(id),
                GrowthStages = _model.GetLookup()
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

        public IActionResult Save(FuzzyParameterDataEntryModel item)
        {
            if (item.FuzzyParameter.Id == 0)
                _model.New(item.FuzzyParameter);
            else
                _model.Update(item.FuzzyParameter);
            return View("Index", _model.GetAll());
        }
    }
}