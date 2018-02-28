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
	    Order GetOrderById(string username, int id);
        bool SaveAll();
	    void AddEntity(object model);
	    IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
	}
}