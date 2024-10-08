using Inventory_System_MVC.Data;
using Inventory_System_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventory_System_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly InventoryDbContext _context;

        public HomeController(InventoryDbContext context)
        {
            _context = context;
        }

        // Home/Index action to show both equipment and customer lists
        public IActionResult Index()
        {
            // Fetch both Equipment and Customer lists
            var equipmentList = _context.Equipment.ToList();
            var customerList = _context.Customers.Include(c => c.Equipment).ToList();

            // Pass both lists to the view using ViewData
            ViewData["EquipmentList"] = equipmentList;
            ViewData["CustomerList"] = customerList;

            return View();
        }

        public IActionResult CreateEquipment ()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateEquipment(Equipment equipment)
        {
          
                try
                {
                    _context.Equipment.Add(equipment);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it)
                    ModelState.AddModelError("", "An error occurred while saving the equipment. Please try again.");
                }
            
            return View(equipment);
        }
        public IActionResult CreateCustomer()
        {
            var equipmentList = _context.Equipment.ToList();
            var equipmentCount = equipmentList.Count;
            if (equipmentCount == 0)
            {
                ModelState.AddModelError("", "No equipment available to purchase.");
                return View(); // Return the view without populating the dropdown
            }
            ViewBag.EquipmentList = equipmentList;

            return View();
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            var equipment = _context.Equipment.Find(customer.EquipmentID);
            if (equipment == null)
            {
                ModelState.AddModelError("", "Selected equipment not found.");
                return View(customer); // Return the view with the customer data
            }
            if (equipment.Quantity < customer.EquiCount)
            {
                ModelState.AddModelError("", "Not enough equipment available to purchase.");
                return View(customer); // Return the view with the customer data
            }

            // Update the equipment quantity
            equipment.Quantity -= customer.EquiCount;
            try
            {
                
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                ModelState.AddModelError("", "An error occurred while saving the equipment. Please try again.");
            }

            return View(customer);
        }

    }
}
