﻿// <auto-generated />
using System;
using ListaFilmConMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ListaFilmConMVC.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ListaFilmConMVC.Models.Film", b =>
                {
                    b.Property<int>("FilmID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FilmID"));

                    b.Property<int?>("PictureID")
                        .HasColumnType("int");

                    b.Property<string>("Poster")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("imdbIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FilmID");

                    b.HasIndex("PictureID");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("ListaFilmConMVC.Models.Picture", b =>
                {
                    b.Property<int>("PictureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PictureID"));

                    b.Property<string>("PictureName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RawData")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("PictureID");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ListaFilmConMVC.Models.Film", b =>
                {
                    b.HasOne("ListaFilmConMVC.Models.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureID");

                    b.Navigation("Picture");
                });
#pragma warning restore 612, 618
        }
    }
}
