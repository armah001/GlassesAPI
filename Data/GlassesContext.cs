#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Glasses.Model;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;


namespace Glasses.Data
{
    public class GlassesContext :DbContext

        //public class GlassesContext : 
        //DbContext
    {
        public DbSet<Product> Product { get; set; }

        public DbSet<ApiUser> ApiUser { get; set; }

        public GlassesContext (DbContextOptions<GlassesContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // Use Fluent API to configure

            
            // Map entities to tables  
            builder.Entity<Product>().ToTable("GlassesProduct");

            // Configure Primary Keys  
            builder.Entity<Product>().HasKey(ug => ug.ProductId).HasName("ProductId");
            
            // Configure columns  
            builder.Entity<Product>().Property(ug => ug.ProductId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired() ;

            builder.Entity<Product>().Property(ug => ug.Product_Name).HasColumnType("nvarchar(100)").IsRequired();

            builder.Entity<Product>().Property(ug => ug.Price).HasColumnType("double").IsRequired();

            builder.Entity<Product>().Property(ug => ug.Image).HasColumnType("nvarchar(100)").IsRequired();

            //auth table
            builder.Entity<ApiUser>().ToTable("ApiUsers");
            // Configure Primary Keys  
            builder.Entity<ApiUser>().HasKey(ig => ig.UserId).HasName("UserId");

            //Configure columns
            builder.Entity<ApiUser>().Property(ig => ig.UserId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();

            builder.Entity<ApiUser>().Property(ig => ig.UserName).HasColumnType("nvarchar(100)").IsRequired();

            builder.Entity<ApiUser>().Property(ig => ig.Password).HasColumnType("nvarchar(100)").IsRequired();


        }
        //public DbSet<Product> Products { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ApiUser> ApiUsers { get; set; }
    }
}
