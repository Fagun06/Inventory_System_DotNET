using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Inventory_System_MVC.Models
{
	public class Register
	{
		[Key]
		public string Name { get; set; }
		public string Password { get; set; }
    }
}
