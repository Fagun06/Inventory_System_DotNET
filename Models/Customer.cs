namespace Inventory_System_MVC.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }    // Name of the customer
        public string CustomerMobile { get; set; }  // Mobile number of the customer

        // Foreign key relationship with Equipment
        public int EquipmentID { get; set; }        // The ID of the equipment they bought
        public Equipment Equipment { get; set; }    // Navigation property to Equipment

        public int EquiCount { get; set; }          // Quantity of equipment purchased by the customer
    }
}
