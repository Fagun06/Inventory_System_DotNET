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

        #region Methods

        // Home/Index action to show both equipment and customer lists
        //public IActionResult Index(string searchEquipment, string searchCustomer)
        //{

        //    ViewData["searchEquipment"] = searchEquipment;
        //    ViewData["searchCustomer"] = searchCustomer;

        //    var equipments = from e in _context.Equipment select e;

        //    if (!string.IsNullOrEmpty(searchEquipment))
        //    {
        //        equipments = equipments.Where(e => e.EquipmentName.Contains(searchEquipment));
        //    }


        //    // Fetch both Equipment and Customer lists
        //    var equipmentList = _context.Equipment.ToList();
        //    var customerList = _context.Customers.Include(c => c.Equipment).ToList();

        //    // Pass both lists to the view using ViewData
        //    ViewData["EquipmentList"] = equipmentList;
        //    ViewData["CustomerList"] = customerList;

        //    return View();
        //}

        public IActionResult Index(string searchEquipment, string searchCustomer)
        {
            // Pass search values back to the view for display in the search boxes
            ViewData["searchEquipment"] = searchEquipment;
            ViewData["searchCustomer"] = searchCustomer;

            // Initialize the equipment query
            var equipmentQuery = from e in _context.Equipment select e;

            // Filter by searchEquipment if the search term is not null or empty
            if (!string.IsNullOrEmpty(searchEquipment))
            {
                equipmentQuery = equipmentQuery.Where(e => e.EquipmentName.Contains(searchEquipment));
            }

            // Fetch the filtered equipment list
            var equipmentList = equipmentQuery.ToList();

            // Fetch the full customer list (if you want to add customer search later, adjust this too)
            var customerQuery = _context.Customers.Include(c => c.Equipment).AsQueryable();

            if (!string.IsNullOrEmpty(searchCustomer))
            {
                customerQuery = customerQuery.Where(c => c.CustomerName.Contains(searchCustomer));
            }

            var customerList = customerQuery.ToList();
            // Pass both lists to the view
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

        #endregion
    }
}
