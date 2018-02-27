using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTree.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace DutchTree.Data
{
    public class DutchSeeder
    {
	    private readonly DutchContext _ctx;
	    private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
	    {
		    _ctx = ctx;
		    _hosting = hosting;
	        _userManager = userManager;
	    }

	    public async Task Seed()
	    {    //Before we try to issue any queries ensure that table are created
		    _ctx.Database.EnsureCreated();

	        var user = await _userManager.FindByEmailAsync("shawn@dutchtreat.com");

	        if (user == null)
	        {
	            user = new StoreUser()
	            {
	                FirstName = "Shawn",
	                LastName = "Wildermuth",
	                UserName = "shawn@dutchtreat.com",
	                Email = "shawn@dutchtreat.com"
	            };

		        var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
		        if (result != IdentityResult.Success)
		        {
			        throw new InvalidOperationException("Failed to create default user");
		        }

			}

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
                    User = user,
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
