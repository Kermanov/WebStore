﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Repositories;

namespace WebStore.UnitOfWork
{
    public class WebStoreUnitOfWork: IDisposable
    {
        private readonly ApplicationDbContext context;
        private bool disposed = false;

        public WebStoreUnitOfWork(ApplicationDbContext context)
        {
            this.context = context;

            Categories = new Repository<Category>(context);
            Products = new Repository<Product>(context);
            Comments = new Repository<Comment>(context);
            Users = context.Users;
            UserRoles = context.UserRoles;
            Roles = context.Roles;
            ShoppingCartItems = context.ShoppingCartItems;
            Deliveries = context.Deliveries;
            Votes = new Repository<Vote>(context);
        }

        public Repository<Category> Categories { get; }
        public Repository<Product> Products { get; }
        public Repository<Comment> Comments { get; }
        public DbSet<IdentityUser> Users { get; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; }
        public DbSet<IdentityRole> Roles { get; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public Repository<Vote> Votes { get; }

        public void Save()
        {
            context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
