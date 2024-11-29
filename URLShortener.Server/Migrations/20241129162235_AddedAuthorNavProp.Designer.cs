﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using URLShortener.Server.Data;

#nullable disable

namespace URLShortener.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241129162235_AddedAuthorNavProp")]
    partial class AddedAuthorNavProp
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("URLShortener.Server.Models.URLData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastFetchedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("URLs", (string)null);
                });

            modelBuilder.Entity("URLShortener.Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("URLShortener.Server.Models.Admin", b =>
                {
                    b.HasBaseType("URLShortener.Server.Models.User");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("URLShortener.Server.Models.AuthorizedUser", b =>
                {
                    b.HasBaseType("URLShortener.Server.Models.User");

                    b.HasDiscriminator().HasValue("AuthorizedUser");
                });

            modelBuilder.Entity("URLShortener.Server.Models.URLData", b =>
                {
                    b.HasOne("URLShortener.Server.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("URLShortener.Server.Services.UrlMetadata", "Metadata", b1 =>
                        {
                            b1.Property<int>("URLDataId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Description")
                                .HasColumnType("TEXT");

                            b1.Property<string>("ImageUrl")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Title")
                                .HasColumnType("TEXT");

                            b1.HasKey("URLDataId");

                            b1.ToTable("URLs");

                            b1.WithOwner()
                                .HasForeignKey("URLDataId");
                        });

                    b.Navigation("Author");

                    b.Navigation("Metadata");
                });
#pragma warning restore 612, 618
        }
    }
}
