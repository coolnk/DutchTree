using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTree.Data;
using DutchTree.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DutchTree.Controllers
{
    public class AppController : Controller
    {
	    private readonly IDutchRepository _repository;

	    public AppController(IDutchRepository repository)
	    {
		    _repository = repository;
	    }

		public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contacts()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contacts(ContactViewModel model)
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }

	    public IActionResult Shop()
	    {
		    var result = _repository.GetAllProducts();
			return View(result);
	    }
    }
}