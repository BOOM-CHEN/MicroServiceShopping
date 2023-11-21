﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoppingUser.EntityModel.ShoppingUserDbContext;

#nullable disable

namespace ShoppingUser.EntityModel.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ShoppingUser.EntityModel.Models.Password", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("IV")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("T_UserPassword", (string)null);
                });

            modelBuilder.Entity("ShoppingUser.EntityModel.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(128)")
                        .HasColumnName("Id");

                    b.Property<string>("RegisterTime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("UserAvatar")
                        .HasColumnType("longtext");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserPhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("UserRecieveAddress")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("T_User", (string)null);
                });

            modelBuilder.Entity("ShoppingUser.EntityModel.Models.Password", b =>
                {
                    b.HasOne("ShoppingUser.EntityModel.Models.User", "User")
                        .WithOne("Passwords")
                        .HasForeignKey("ShoppingUser.EntityModel.Models.Password", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ShoppingUser.EntityModel.Models.User", b =>
                {
                    b.Navigation("Passwords");
                });
#pragma warning restore 612, 618
        }
    }
}
