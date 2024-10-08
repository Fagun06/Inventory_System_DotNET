using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Inventory_System_MVC.Models.Account
{
    public class Login
    {
        [Key]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
