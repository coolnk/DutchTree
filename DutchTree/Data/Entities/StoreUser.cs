using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DutchTree.Data.Entities
{
    public class StoreUser : IdentityUser //have a bunch fo default properties for identity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}
