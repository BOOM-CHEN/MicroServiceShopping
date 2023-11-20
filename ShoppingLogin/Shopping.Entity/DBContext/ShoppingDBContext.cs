using Microsoft.EntityFrameworkCore;
using Shopping.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Entity.DBContext
{
    public class ShoppingDBContext : DbContext
    {
        public ShoppingDBContext(DbContextOptions options) : base(options)
        {
        }

        public ShoppingDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("T_User");
            modelBuilder.Entity<User>()
                .HasOne(u => u.Passwords)
                .WithOne(p => p.User)
                .HasForeignKey<Password>(f => f.UserId);

            modelBuilder.Entity<Password>().ToTable("T_UserPassword");
            modelBuilder.Entity<Password>(entity =>
            {
                entity.Property(p => p.PublicKey).IsRequired();
                entity.Property(p => p.IV).IsRequired();
            });
        }
    }
}
