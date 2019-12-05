namespace Invoice
{
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Invoice> Invoices { get; set; }

        public DbSet<Models.LineItem> LineItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<Models.Invoice>()
                .HasMany(l => l.LineItems)
                .WithOne(g => g.Invoice);
        }
    }
}