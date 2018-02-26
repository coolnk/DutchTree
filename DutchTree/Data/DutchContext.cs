using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTree.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTree.Data
{
    public class DutchContext: DbContext
    {
	    public DutchContext(DbContextOptions<DutchContext> options): base(options)
	    {}

	    public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
    }
}
