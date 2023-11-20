using Microsoft.EntityFrameworkCore;
using ShoppingUser.EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.EntityModel.ShoppingUserDbContext
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        protected UserDbContext()
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
