using Inventory_System_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_System_MVC.Data
{

    public class InventoryDbContext : DbContext
		{
			public InventoryDbContext(DbContextOptions options) : base(options)
			{

			}
			public DbSet<Register> Register { get; set; }
			public DbSet<Equipment> Equipment { get; set; }
			public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting up the relationship between Customer and Equipment
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Equipment)
                .WithMany(e => e.Customers)
                .HasForeignKey(c => c.EquipmentID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
	
}
