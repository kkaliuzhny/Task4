using Microsoft.EntityFrameworkCore;
using task4.Models.Entities;

namespace task4.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {   
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .HasDatabaseName("IX_Users_Email") // Optional: specify a name for the index
                .IsUnique(); // Optional: enforce uniqueness if needed

            modelBuilder.Entity<User>()
             .Property(e => e.Status)
             .HasColumnType("nvarchar(8)");

           
        }
    }
}
