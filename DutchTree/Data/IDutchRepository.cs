using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTree.Data
{
	public interface IDutchRepository
	{
		IEnumerable<Product> GetAlProducts();
		IEnumerable<Product> GetProductsByCategory(string category);
		bool SaveAll();
	}
}