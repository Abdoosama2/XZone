﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XZone.Models;

namespace XZone.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Game> Games { get; set; }

        public DbSet<Category> Categories { get; set; }


        public DbSet<Device> Devices { get; set; }

        public DbSet<GameDevice> GameDevices { get; set; }

        

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
          .HasData(new Category[]
          {
                new Category { Id = 1, Name = "Sports" },
                new Category { Id = 2, Name = "Action" },
                new Category { Id = 3, Name = "Adventure" },
                new Category { Id = 4, Name = "Racing" },
                new Category { Id = 5, Name = "Fighting" },
                new Category { Id = 6, Name = "Film" }
          });

            modelBuilder.Entity<Device>()
                .HasData(new Device[]
                {
                new Device { Id = 1, Name = "PlayStation", Icon = "bi bi-playstation" },
                new Device { Id = 2, Name = "Xbox", Icon = "bi bi-xbox" },
                new Device { Id = 3, Name = "Nintendo Switch", Icon = "bi bi-nintendo-switch" },
                new Device { Id = 4, Name = "PC", Icon = "bi bi-pc-display" }
                });
            modelBuilder.Entity<GameDevice>().HasKey(k => new { k.DeviceId, k.GameId });
        }
    }
}
