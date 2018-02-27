using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTree.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutchTree.Data
{
    public class DutchContext: IdentityDbContext<StoreUser>  //Exchanged with Dbcontext default to allow identityt
    {
	    public DutchContext(DbContextOptions<DutchContext> options): base(options)
	    {}

	    public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
    }
}
