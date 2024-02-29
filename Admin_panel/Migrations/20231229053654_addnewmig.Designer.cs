﻿// <auto-generated />
using Admin_panel.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Admin_panel.Migrations
{
    [DbContext(typeof(Applicationdbcontext))]
    [Migration("20231229053654_addnewmig")]
    partial class addnewmig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Admin_panel.Models.Data.Category", b =>
                {
                    b.Property<int>("cat_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("cat_id"), 1L, 1);

                    b.Property<string>("cat_name")
                        .IsRequired()
                        .HasColumnType("Varchar(50)");

                    b.Property<int>("cat_status")
                        .HasColumnType("int");

                    b.HasKey("cat_id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Admin_panel.Models.Data.Product", b =>
                {
                    b.Property<int>("p_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("p_id"), 1L, 1);

                    b.Property<int>("p_category")
                        .HasColumnType("int");

                    b.Property<string>("p_description")
                        .IsRequired()
                        .HasColumnType("Varchar(max)");

                    b.Property<string>("p_img")
                        .IsRequired()
                        .HasColumnType("Varchar(max)");

                    b.Property<double>("p_mrp")
                        .HasColumnType("float");

                    b.Property<string>("p_name")
                        .IsRequired()
                        .HasColumnType("Varchar(50)");

                    b.Property<double>("p_price")
                        .HasColumnType("float");

                    b.Property<string>("p_stock")
                        .IsRequired()
                        .HasColumnType("Varchar(50)");

                    b.Property<int>("p_supermart")
                        .HasColumnType("int");

                    b.HasKey("p_id");

                    b.HasIndex("p_category");

                    b.HasIndex("p_supermart");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Admin_panel.Models.Data.SuperMarket", b =>
                {
                    b.Property<int>("sp_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("sp_id"), 1L, 1);

                    b.Property<string>("sp_name")
                        .IsRequired()
                        .HasColumnType("Varchar(50)");

                    b.Property<string>("sp_town")
                        .IsRequired()
                        .HasColumnType("Varchar(50)");

                    b.HasKey("sp_id");

                    b.ToTable("SuperMarkets");
                });

            modelBuilder.Entity("Admin_panel.Models.Data.Product", b =>
                {
                    b.HasOne("Admin_panel.Models.Data.Category", "p_cat")
                        .WithMany()
                        .HasForeignKey("p_category")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Admin_panel.Models.Data.SuperMarket", "p_spmart")
                        .WithMany()
                        .HasForeignKey("p_supermart")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("p_cat");

                    b.Navigation("p_spmart");
                });
#pragma warning restore 612, 618
        }
    }
}
