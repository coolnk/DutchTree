using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace DutchTree.Data
{
    public class DutchSeeder
    {
	    private readonly DutchContext _ctx;
	    private readonly IHostingEnvironment _hosting;

	    public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting)
	    {
		    _ctx = ctx;
		    _hosting = hosting;
	    }

	    public void Seed()
	    {    //Before we try to issue any queries ensure that table are created
		    _ctx.Database.EnsureCreated();
		    if (!_ctx.Products.Any())
		    {

			    var filepath = Path.Combine(_hosting.ContentRootPath, "Data/products.json");
			    var file = File.ReadAllText(filepath);
			    var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(file);
				_ctx.Products.AddRange(products);
			    
				var order = new Order()
				{
					OrderDate = DateTime.Now,
					OrderNumber = "12345",
					Items = new List<OrderItem>()
					{
						new OrderItem()
						{
							Product = products.First(),
							Quantity = 2,
							UnitPrice = products.First().Price
						}
					}

				};

			    _ctx.Orders.Add(order);
			    _ctx.SaveChanges();

		    }
	    }

    }
}
