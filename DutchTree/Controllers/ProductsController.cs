using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using DutchTree.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTree.Controllers
{
	// [Produces("application/json")]
	// [Route("api/Products")]
	[Route("api/[Controller]")]
	public class ProductsController : Controller
    {
		private readonly IDutchRepository _repository;
		private readonly ILogger<ProductsController> _logger;

		public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
	    {
			_repository = repository;
			_logger = logger;

		}
		
		[HttpGet]
	    public IEnumerable<Product> Get()
	    {
		    return _repository.GetAlProducts();
	    }
    }
}