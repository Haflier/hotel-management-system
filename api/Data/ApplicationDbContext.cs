using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configuration;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions Options)
        : base(Options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

            builder.Entity<RoomService>(x => x.HasKey(p => new { p.RoomId, p.ServiceId }));

            builder.Entity<RoomService>()
                .HasOne(r => r.Room)
                .WithMany(r => r.RoomServices)
                .HasForeignKey(p => p.RoomId);

            builder.Entity<RoomService>()
                .HasOne(s => s.Service)
                .WithMany(r => r.RoomServices)
                .HasForeignKey(p => p.ServiceId);

            builder.Entity<Room>()
                .Ignore(r => r.ReservedDates);

            builder.Entity<Room>()
                .Ignore(r => r.ActiveServices);

            builder.Entity<Room>()
                .Ignore(r => r.TotalPricePerDay);

            SeedData.Seed(builder);

        }
    }
}
