using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DutchTree.Data
{
    public class DutchRepository : IDutchRepository
	{
	    private readonly DutchContext _context;
		private readonly ILogger<DutchRepository> _logger;

		public DutchRepository( DutchContext context, ILogger<DutchRepository> logger)
		{
			_context = context;
			_logger = logger;
		}

	    public IEnumerable<Product> GetAlProducts()
	    {
		    try
		    {
			    _logger.LogInformation("Get all products was called");
			    return _context.Products.OrderBy(p => p.Title).ToList();
			}
		    catch (Exception ex)
		    {
			    _logger.LogError($"Error in all products: {ex}");
			    throw;
		    }
		
	    }

	    public IEnumerable<Product> GetProductsByCategory(string category)
	    {
		    try
		    {
			    _logger.LogInformation(" GetProducts by Categorory was called");
				return _context.Products.Where(p => p.Category == category).ToList();
			}
		    catch (Exception ex)
		    {
			    _logger.LogError($"Error was thrown in GetProducts by Categor {ex}");
			    throw;
		    }
		
	    }

		public bool SaveAll()
		{
			try
			{
				_logger.LogInformation("Save all was called");
				return _context.SaveChanges() > 0;

			}
			catch (Exception ex)
			{
				_logger.LogError($"An error was thrown in Save All {ex}");
				throw;
			}
			
		}
    }
}
