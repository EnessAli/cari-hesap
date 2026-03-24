// Ensure you have the required NuGet package installed
// Run the following command in the Package Manager Console or use the NuGet Package Manager in Visual Studio:
// Install-Package Microsoft.EntityFrameworkCore

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CariHesap.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CariHesap.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TaxNumber).HasMaxLength(20);
                entity.Property(e => e.TaxOffice).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Balance).HasPrecision(18, 2);

                entity.HasMany(e => e.Invoices)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Transactions)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Invoice configuration
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SubTotal).HasPrecision(18, 2);
                entity.Property(e => e.VAT).HasPrecision(18, 2);
                entity.Property(e => e.Discount).HasPrecision(18, 2);
                entity.Property(e => e.Total).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasMany(e => e.Items)
                    .WithOne(e => e.Invoice)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Transactions)
                    .WithOne(e => e.Invoice)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // InvoiceItem configuration
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Quantity).HasPrecision(18, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.VATRate).HasPrecision(5, 2);
                entity.Property(e => e.Total).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CariHesap;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}