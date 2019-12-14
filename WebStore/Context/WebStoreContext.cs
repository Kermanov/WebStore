using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Context
{
    public class WebStoreContext: DbContext
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options)
            :base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Clothes" },
                new Category { Id = 2, Name = "Dishes" },
                new Category { Id = 3, Name = "Food" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    Name = "Corn", 
                    Description = "Corn, Zea mays, is an annual grass in the family Poaceae " +
                    "and is a staple food crop grown all over the world. The corn plant possesses " +
                    "a simple stem of nodes and internodes.",
                    Price = 5.99M,
                    DisplayComments = true,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 2,
                    Name = "Apple",
                    Description = "An apple is a sweet, edible fruit produced by an apple tree (Malus domestica).",
                    Price = 2.55M,
                    DisplayComments = true,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 3,
                    Name = "Cup",
                    Description = "A cup is an open-top container used to hold liquids for pouring " +
                    "or drinking; it also can be used to store solids for pouring (e.g., sugar, flour, grains).",
                    Price = 7.50M,
                    DisplayComments = true,
                    CategoryId = 2
                }
            );
        }
    }
}
