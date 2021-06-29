using DealerTrack.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealerTrack.DataBase
{
    public class DealerTrackDbContext: DbContext
    {
        public DealerTrackDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Deal> Deals { get; set; }
        public virtual DbSet<Dealership> Dealerships { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Dealership>(entity =>
            {
                entity.ToTable("Dealerships");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Deal>(entity =>
            {
                entity.ToTable("Deals");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Dealership)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.DealershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}