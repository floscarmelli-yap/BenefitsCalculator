﻿// <auto-generated />
using System;
using BenefitsCalculator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BenefitsCalculator.Migrations
{
    [DbContext(typeof(BenefitsContext))]
    [Migration("20231024215414_DBStructureUpdate")]
    partial class DBStructureUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.BenefitsHistGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("BasicSalary")
                        .HasColumnType("float");

                    b.Property<int>("ConsumerId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("GuaranteedIssue")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ConsumerId");

                    b.ToTable("BenefitsHistGroups");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.BenefitsHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AmountQuotation")
                        .HasColumnType("float");

                    b.Property<int>("BenefitsHistGroupId")
                        .HasColumnType("int");

                    b.Property<int>("BenefitsStatus")
                        .HasColumnType("int");

                    b.Property<int>("Multiple")
                        .HasColumnType("int");

                    b.Property<double>("PendedAmount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BenefitsHistGroupId");

                    b.ToTable("BenefitsHistories");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.Consumer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("BasicSalary")
                        .HasColumnType("float");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("Date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("SetupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SetupId");

                    b.ToTable("Consumers");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.Setup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("GuaranteedIssue")
                        .HasColumnType("float");

                    b.Property<int>("Increments")
                        .HasColumnType("int");

                    b.Property<int>("MaxAgeLimit")
                        .HasColumnType("int");

                    b.Property<int>("MaxRange")
                        .HasColumnType("int");

                    b.Property<int>("MinAgeLimit")
                        .HasColumnType("int");

                    b.Property<int>("MinRange")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Setups");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.BenefitsHistGroup", b =>
                {
                    b.HasOne("BenefitsCalculator.Data.Entities.Consumer", "Consumer")
                        .WithMany("BenefitsHistGroups")
                        .HasForeignKey("ConsumerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consumer");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.BenefitsHistory", b =>
                {
                    b.HasOne("BenefitsCalculator.Data.Entities.BenefitsHistGroup", "BenefitsHistGroup")
                        .WithMany("BenefitsHistories")
                        .HasForeignKey("BenefitsHistGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BenefitsHistGroup");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.Consumer", b =>
                {
                    b.HasOne("BenefitsCalculator.Data.Entities.Setup", "Setup")
                        .WithMany()
                        .HasForeignKey("SetupId");

                    b.Navigation("Setup");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.BenefitsHistGroup", b =>
                {
                    b.Navigation("BenefitsHistories");
                });

            modelBuilder.Entity("BenefitsCalculator.Data.Entities.Consumer", b =>
                {
                    b.Navigation("BenefitsHistGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
