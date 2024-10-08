using Inventory_System_MVC.Data;
using Inventory_System_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Inventory_System_MVC.Controllers
{
	public class AccountController : Controller
	{
        private readonly InventoryDbContext _context;

        public AccountController(InventoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Login(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Register
                    .FirstOrDefault(u => u.Name == model.Name && u.Password == model.Password);

                if (user != null)
                {
                    // Login successful, redirect to home or dashboard
                    return RedirectToAction("Index", "Home");
                }

                // If user is not found or password doesn't match
                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }


        public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Register(Register register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Register.Add(register);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Registration successful! Please log in.";
                    return RedirectToAction("Register");
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                {
                    ModelState.AddModelError(string.Empty, "A user with this key already exists.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                }
            }
            return View(register);
        }
    }
}
