﻿using Bogus;
using Garage3._0.Models;
using System;

namespace Garage3._0.Data
{
    public class SeedDataGenerator
    {
        private readonly GarageDbContext _context;
        private readonly Faker faker;

        public SeedDataGenerator(GarageDbContext context )
        {
            _context = context;
            faker = new Faker();
        }

        public void Generate()
        {
            if (_context.Members.Any())
            {
                Console.WriteLine("Database already contains data. Skipping data generation.");
                return;
            }
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType { VehicleTypeName = "Car", ParkingSpaceRequirement = 1, Vehicles = new List<Vehicle>() },
                new VehicleType { VehicleTypeName = "Truck", ParkingSpaceRequirement = 2 ,Vehicles = new List<Vehicle>()},
                new VehicleType {  VehicleTypeName = "Bus", ParkingSpaceRequirement = 3 , Vehicles = new List < Vehicle >()}
            };
            for(int i =0; i < 10; i++)
            {
                var member = new Member
                {
                 
                    SocialSecurityNr = GenerateBirthdayCode(faker),
                    Firstname = faker.Name.FirstName(),
                    Surname = faker.Name.LastName(),
                    VehicleList = new List<Vehicle>()
                };
                _context.Members.Add(member);
                for(int j =0; j <faker.Random.Int(1,5); j++)
                {
                    var vehicle = new Vehicle
                    {
                    
                        LicencePlate = faker.Random.Replace("???###"),
                        Color = faker.Commerce.Color(),
                        Brand = faker.Vehicle.Manufacturer(),
                        Model = faker.Vehicle.Model(),
                        NumWheels = 4,
                        Member = member,
                        VehicleType = faker.PickRandom(vehicleTypes)
                    };
                    member.VehicleList.Add(vehicle);
                    _context.Vehicles.Add(vehicle);
                }
            }
            _context.SaveChanges();
        }

        private int GenerateBirthdayCode(Faker faker)
        {
            int year = faker.Date.Between(new DateTime(1950, 1, 1), new DateTime(2000, 12, 31)).Year;
            int month = faker.Random.Int(1, 12);
            int day = faker.Date.Between(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month))).Day;
            var birthdayCode = int.Parse($"{year}{month:00}{day:00}");
            return birthdayCode;
        }
    }
}