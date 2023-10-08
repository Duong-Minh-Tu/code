using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using SellingBook.Constants;
using SellingBook.Entities;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;

namespace SellingBook.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartDetail> cartDetails { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<BillDetail> billDetails { get; set; }
        public DbSet<HistoryBill> historyBills { get; set; }
        public DbSet<Like> likes { get; set; }
        public DbSet<ListImageBook> listImageBooks { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Discount> discounts { get; set; }
        public DbSet<ListImageReview> listImageReviews { get; set; }
        public DbSet<Address> address { get; set; }

        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(s => s.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(e => e.UserName)
                    .IsUnicode(false)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.CustomerName)
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.Sex)
                    .HasColumnType("char");

                entity.Property(e => e.Email)
                    .HasColumnType("nvarchar(100)");

                entity.Property(e => e.Phone)
                    .HasColumnType("nvarchar(50)");

                entity.Property(e => e.UserType)
                    .HasDefaultValue(UserTypes.Customer);
            });
        }
    }
    }
