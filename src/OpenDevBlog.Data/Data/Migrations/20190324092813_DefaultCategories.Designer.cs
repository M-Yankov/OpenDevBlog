﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenDevBlog.Data;

namespace OpenDevBlog.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190324092813_DefaultCategories")]
    partial class DefaultCategories
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsAnonymous");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .HasMaxLength(100000);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<DateTime?>("ReviewDate");

                    b.Property<string>("ReviewerId");

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 428, DateTimeKind.Utc).AddTicks(8023),
                            IsDeleted = false,
                            Name = "Programing"
                        },
                        new
                        {
                            Id = 2,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 428, DateTimeKind.Utc).AddTicks(9772),
                            IsDeleted = false,
                            Name = "Web"
                        },
                        new
                        {
                            Id = 3,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(54),
                            IsDeleted = false,
                            Name = "Desktop"
                        },
                        new
                        {
                            Id = 4,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(58),
                            IsDeleted = false,
                            Name = "Front-end"
                        },
                        new
                        {
                            Id = 5,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(80),
                            IsDeleted = false,
                            Name = "Administration"
                        },
                        new
                        {
                            Id = 6,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(80),
                            IsDeleted = false,
                            Name = "Operating System"
                        },
                        new
                        {
                            Id = 7,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(84),
                            IsDeleted = false,
                            Name = "Game development"
                        },
                        new
                        {
                            Id = 8,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(88),
                            IsDeleted = false,
                            Name = "UX"
                        },
                        new
                        {
                            Id = 9,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(97),
                            IsDeleted = false,
                            Name = "Design"
                        },
                        new
                        {
                            Id = 10,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(101),
                            IsDeleted = false,
                            Name = "Code Quality"
                        },
                        new
                        {
                            Id = 11,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(101),
                            IsDeleted = false,
                            Name = "Software Engineering"
                        },
                        new
                        {
                            Id = 12,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(105),
                            IsDeleted = false,
                            Name = "Agile"
                        },
                        new
                        {
                            Id = 13,
                            CreatedOn = new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(110),
                            IsDeleted = false,
                            Name = "Source control"
                        });
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.CategoryArticle", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<int>("Id");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("ArticleId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryArticles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.Article", b =>
                {
                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenDevBlog.Models.Database.ApplicationUser", "Reviewer")
                        .WithMany("ReviewedArticles")
                        .HasForeignKey("ReviewerId");
                });

            modelBuilder.Entity("OpenDevBlog.Models.Database.CategoryArticle", b =>
                {
                    b.HasOne("OpenDevBlog.Models.Database.Article", "Article")
                        .WithMany("Categories")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OpenDevBlog.Models.Database.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
