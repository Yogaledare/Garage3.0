﻿using Bogus;
using Garage3._0.Models;
using System;

namespace Garage3._0.Data
{
    public class SeedDataGenerator
    {
        private readonly GarageDbContext _context;
        private readonly Faker faker;

        public SeedDataGenerator(GarageDbContext context)
        {
            _context = context;
            faker = new Faker();
        }

        public void Generate(int count)
        {
            if (_context.Members.Any())
            {
                Console.WriteLine("Database already contains data. Skipping data generation.");
                return;
            }

            List<WheelConfiguration> wheelConfigurations = [
                new WheelConfiguration {
                    NumWheels = 4
                }, 
                new WheelConfiguration {
                    NumWheels = 6
                }, 
                new WheelConfiguration {
                    NumWheels = 8
                }, 
                new WheelConfiguration {
                    NumWheels = 10
                },
                new WheelConfiguration {
                NumWheels = 12
                }
            ];

            var vehicleTypes = new List<VehicleType> {
                new VehicleType {
                    VehicleTypeName = "Car", 
                    ParkingSpaceRequirement = 1,
                    Vehicles = new List<Vehicle>(),
                    WheelConfigurations = [
                        new WheelConfiguration {
                            NumWheels = 4
                        }
                    ]
                },
                new VehicleType {
                    VehicleTypeName = "Truck", 
                    ParkingSpaceRequirement = 2, 
                    Vehicles = new List<Vehicle>(),
                    WheelConfigurations = [
                        new WheelConfiguration {
                            NumWheels = 4
                        }, 
                        new WheelConfiguration {
                            NumWheels = 6
                        }, 
                        new WheelConfiguration {
                            NumWheels = 8
                        }, 
                        new WheelConfiguration {
                            NumWheels = 10
                        }, 
                        new WheelConfiguration {
                            NumWheels = 12
                        }, 
                    ]
                },
                new VehicleType {
                    VehicleTypeName = "Bus",
                    ParkingSpaceRequirement = 3,
                    Vehicles = new List<Vehicle>(), 
                    WheelConfigurations = [
                        new WheelConfiguration {
                            NumWheels = 4
                        }, 
                        new WheelConfiguration {
                            NumWheels = 6
                        }, 
                        new WheelConfiguration {
                            NumWheels = 8
                        }, 
                    ]
                }
            };
            for (int i = 0; i < count; i++) {
                var ssr = GenerateBirthdayCode(faker);
                var member = new Member {
                 
                    SocialSecurityNr = ssr,
                    Firstname = faker.Name.FirstName(),
                    Surname = faker.Name.LastName(),
                    VehicleList = new List<Vehicle>()
                };
                _context.Members.Add(member);
                //generate start and end date using bogus
                DateTime startDate = faker.Date.Between(DateTime.Now.AddDays(-40), DateTime.Now);
                DateTime endDate = CalculateAge(ssr) < 65 ? startDate.AddDays(30) : startDate.AddYears(2);
                var membership = new Membership
                {
                    Type = Membership.MembershipType.Premium,
                    StartDate = startDate,
                    EndDate = endDate,
                    Member = member //will database set the id?
                };
                _context.Memberships.Add(membership);
                member.Membership = membership;
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

        private string GenerateBirthdayCode(Faker faker)
        {
            int year = faker.Date.Between(new DateTime(1950, 1, 1), new DateTime(2000, 12, 31)).Year;
            int month = faker.Random.Int(1, 12);
            int day = faker.Date.Between(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month))).Day;
            int randomFourDigits = faker.Random.Int(1000, 9999);
            var birthdayCode = $"{year}{month:00}{day:00}{randomFourDigits}";
            return birthdayCode;
        }

        private int CalculateAge(string code)
        {
            //ugly code lol
            if (code == null || code.Length < 4)
            {
                throw new ArgumentException("Invalid code: Code must contain at least four characters.");
            }
            int birthYear;
            if (!int.TryParse(code.Substring(0, 4), out birthYear))
            {
                throw new ArgumentException("Invalid year in code: The first four characters must be a valid year.");
            }
            int currentYear = DateTime.Today.Year;
            return currentYear - birthYear;
        }
    }
}
