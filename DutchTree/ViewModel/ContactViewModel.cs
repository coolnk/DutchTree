using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTree.ViewModel
{
    public class ContactViewModel
    {
		[Required]
	    public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Subject { get; set; }
		[Required]
		[MaxLength(250)]
		public string Message { get; set; }
    }
}
