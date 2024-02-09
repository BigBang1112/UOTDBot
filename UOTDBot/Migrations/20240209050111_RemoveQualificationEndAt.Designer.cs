﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UOTDBot;

#nullable disable

namespace UOTDBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240209050111_RemoveQualificationEndAt")]
    partial class RemoveQualificationEndAt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("UOTDBot.Models.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DownloadUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FileSize")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("MapId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MapUid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UploadedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });
#pragma warning restore 612, 618
        }
    }
}
