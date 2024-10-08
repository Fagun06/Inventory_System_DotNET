using System.Runtime.Serialization;

namespace Inventory_System_MVC.Models
{
    public class Equipment
    {

        public int EquipmentID { get; set; }
        public string EquipmentName { get; set; }  // Name of the equipment
        public int Quantity { get; set; }          // Stock available
        public DateTime Date { get; set; }         // Date added to inventory

        // Navigation property to reference the customers who bought this equipment
        public ICollection<Customer> Customers { get; set; }

    }
}
