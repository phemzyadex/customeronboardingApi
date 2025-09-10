using System;
using Microsoft.EntityFrameworkCore;
using CustomerOnboarding.Core.Models;

namespace CustomerOnboarding.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<Lga> Lgas { get; set; } = null!;
        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // State → Lgas
            modelBuilder.Entity<State>()
                .HasMany(s => s.Lgas)
                .WithOne(l => l.State)
                .HasForeignKey(l => l.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lga → Customers
            modelBuilder.Entity<Lga>()
                .HasMany(l => l.Customers)
                .WithOne(c => c.Lga)
                .HasForeignKey(c => c.LgaId)
                .OnDelete(DeleteBehavior.Restrict);

            // State → Customers
            modelBuilder.Entity<State>()
                .HasMany(s => s.Customers)
                .WithOne(c => c.State)
                .HasForeignKey(c => c.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Soft delete filter
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);

            // Seed States & LGAs
            var lagosId = Guid.Parse("a05288f7-a58c-4af4-bd22-c2e2532f952b");
            modelBuilder.Entity<State>().HasData(
                new State { Id = lagosId, Name = "Lagos" }
            );
                        
            modelBuilder.Entity<Lga>().HasData(
                new Lga { Id = Guid.Parse("9d454989-b9b2-42fc-b3d4-42748afdef11"), Name = "Alimosho", StateId = lagosId },
                new Lga { Id = Guid.Parse("12c0f8cb-3b7f-4072-9061-ca2f3a3f2d90"), Name = "Ikeja", StateId = lagosId },
                new Lga { Id = Guid.Parse("05b1237e-b189-4c90-adcf-785047b76362"), Name = "Lagos-Island", StateId = lagosId },
                new Lga { Id = Guid.Parse("6dc08fa6-2b77-49d0-b3e2-1e3cd58cbcaf"), Name = "Lagos-Mainland", StateId = lagosId },
                new Lga { Id = Guid.Parse("14597d69-f037-49bf-97c1-20257aeeacd7"), Name = "Surulere", StateId = lagosId }
            );

            // Seed Users
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = Guid.Parse("b4a2301e-d019-489f-97ed-ed3eacbec1c7"),
                    Username = "admin",
                    PasswordHash = "$2a$11$09kqxVNLmgHxNAXJgg84weALfO2n.Hglevlz7Ddr..3CwtSoCzJWm", // admin123
                    Role = "Administrator"
                },
                new AppUser
                {
                    Id = Guid.Parse("6f5c1ca5-5f78-4d15-b573-1c2109054fcf"),
                    Username = "user",
                    PasswordHash = "$2a$11$Qz3LfrWfEHF5r6NhaaTkS.csRaL2KeAI2RYmkebUaaNG49bYgn.RW", // user123
                    Role = "User"
                }
            );
        }
    }
}
