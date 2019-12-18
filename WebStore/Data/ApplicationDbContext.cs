using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Vote> Votes { get; set; }

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
                    CategoryId = 3,
                    ImageSource = "https://www.evrosem.ua/wp-content/uploads/2018/03/kkkrr.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Apple",
                    Description = "An apple is a sweet, edible fruit produced by an apple tree (Malus domestica).",
                    Price = 2.55M,
                    DisplayComments = true,
                    CategoryId = 3,
                    ImageSource = "https://image.shutterstock.com/image-photo/ripe-red-apple-fruit-half-260nw-699645961.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Cup",
                    Description = "A cup is an open-top container used to hold liquids for pouring " +
                    "or drinking; it also can be used to store solids for pouring (e.g., sugar, flour, grains).",
                    Price = 7.50M,
                    DisplayComments = true,
                    CategoryId = 2,
                    ImageSource = "https://finestenglishtea.com/wp-content/uploads/2015/01/550-BLU.jpg"
                }
            );
        }
    }
}
