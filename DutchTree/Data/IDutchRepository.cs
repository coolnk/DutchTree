using System.Collections.Generic;
using DutchTree.Data.Entities;
using DutchTree.ViewModel;

namespace DutchTree.Data
{
	public interface IDutchRepository
	{
		IEnumerable<Product> GetAllProducts();
	    IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Product> GetProductsByCategory(string category);
	    Order GetOrderById(int id);
        bool SaveAll();
	    void AddEntity(object model);
	}
}