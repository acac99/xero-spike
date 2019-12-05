using Microsoft.EntityFrameworkCore;

namespace Invoice
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<Models.Invoice>()
                .HasMany<Models.LineItem>(l => l.LineItems)
                .WithOne(g => g.Invoice);
        }
        
        public DbSet<Models.Invoice> Invoices { get; set; } 
        
        public DbSet<Models.LineItem> LineItems { get; set; }
    }
}