﻿// <auto-generated />
using System;
using Garage3._0.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Garage3._0.Migrations
{
    [DbContext(typeof(GarageDbContext))]
    partial class GarageDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Garage3._0.Models.Member", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberId"));

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SocialSecurityNr")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MemberId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Garage3._0.Models.ParkingEvent", b =>
                {
                    b.Property<int>("ParkingEventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParkingEventID"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("VehicleID")
                        .HasColumnType("int");

                    b.HasKey("ParkingEventID");

                    b.HasIndex("VehicleID")
                        .IsUnique();

                    b.ToTable("ParkingEvents");
                });

            modelBuilder.Entity("Garage3._0.Models.ParkingPlace", b =>
                {
                    b.Property<int>("ParkingPlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParkingPlaceId"));

                    b.Property<int>("ParkingEventID")
                        .HasColumnType("int");

                    b.Property<int>("ParkingPlaceNr")
                        .HasColumnType("int");

                    b.HasKey("ParkingPlaceId");

                    b.HasIndex("ParkingEventID");

                    b.ToTable("parkingPlaces");
                });

            modelBuilder.Entity("Garage3._0.Models.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleId"));

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicencePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumWheels")
                        .HasColumnType("int");

                    b.Property<int?>("ParkingEventID")
                        .HasColumnType("int");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("VehicleId");

                    b.HasIndex("MemberId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Garage3._0.Models.VehicleType", b =>
                {
                    b.Property<int>("VehicleTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleTypeId"));

                    b.Property<int>("ParkingSpaceRequirement")
                        .HasColumnType("int");

                    b.Property<string>("VehicleTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VehicleTypeId");

                    b.ToTable("VehicleTypes");
                });

            modelBuilder.Entity("Garage3._0.Models.WheelConfiguration", b =>
                {
                    b.Property<int>("WheelConfigurationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WheelConfigurationId"));

                    b.Property<int>("NumWheels")
                        .HasColumnType("int");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("WheelConfigurationId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("WheelConfiguration");
                });

            modelBuilder.Entity("Garage3._0.Models.ParkingEvent", b =>
                {
                    b.HasOne("Garage3._0.Models.Vehicle", "Vehicle")
                        .WithOne("ParkingEvent")
                        .HasForeignKey("Garage3._0.Models.ParkingEvent", "VehicleID");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Garage3._0.Models.ParkingPlace", b =>
                {
                    b.HasOne("Garage3._0.Models.ParkingEvent", "ParkingEvent")
                        .WithMany("ParkingPlaces")
                        .HasForeignKey("ParkingEventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingEvent");
                });

            modelBuilder.Entity("Garage3._0.Models.Vehicle", b =>
                {
                    b.HasOne("Garage3._0.Models.Member", "Member")
                        .WithMany("VehicleList")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3._0.Models.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Garage3._0.Models.WheelConfiguration", b =>
                {
                    b.HasOne("Garage3._0.Models.VehicleType", "VehicleType")
                        .WithMany("WheelConfigurations")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Garage3._0.Models.Member", b =>
                {
                    b.Navigation("VehicleList");
                });

            modelBuilder.Entity("Garage3._0.Models.ParkingEvent", b =>
                {
                    b.Navigation("ParkingPlaces");
                });

            modelBuilder.Entity("Garage3._0.Models.Vehicle", b =>
                {
                    b.Navigation("ParkingEvent");
                });

            modelBuilder.Entity("Garage3._0.Models.VehicleType", b =>
                {
                    b.Navigation("Vehicles");

                    b.Navigation("WheelConfigurations");
                });
#pragma warning restore 612, 618
        }
    }
}
