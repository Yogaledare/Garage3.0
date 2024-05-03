using Bogus;
using Garage3._0.Models;

namespace Garage3._0.Data
{
    public class FakeDataGenerator
    {
        public Faker<Member> MemberFaker { get; set; }
        public Faker<Vehicle> VehicleFaker { get; set; }
        public Faker<VehicleType> VehicleTypeFaker { get; set; }

        public FakeDataGenerator()
        {
            Randomizer.Seed = new Random(123);
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType { VehicleTypeName = "Car", ParkingSpaceRequirement = 1, Vehicles = new List<Vehicle>() },
                new VehicleType { VehicleTypeName = "Truck", ParkingSpaceRequirement = 2 ,Vehicles = new List<Vehicle>()},
                new VehicleType { VehicleTypeName = "Bus", ParkingSpaceRequirement = 3 , Vehicles = new List < Vehicle >()}
            };

            VehicleFaker = new Faker<Vehicle>()
                .CustomInstantiator(f =>
                              {
                                  var vehicle = new Vehicle
                                  {
                                      //???###" means three random uppercase letters and three random numbers
                                      LicencePlate = f.Random.Replace("???###"),
                                      Color = f.Commerce.Color(),
                                      Brand = f.Vehicle.Manufacturer(),
                                      Model = f.Vehicle.Model(),
                                      NumWheels = 4,
                                  };
                                  var vehicleType = f.PickRandom(vehicleTypes);
                                  vehicle.VehicleType = vehicleType;
                                  vehicleType.Vehicles.Add(vehicle);
                                  return vehicle;
                              });

            MemberFaker = new Faker<Member>()
            .RuleFor(m => m.Firstname, f => f.Name.FirstName())
            .RuleFor(m => m.Surname, f => f.Name.LastName())
            .RuleFor(m => m.VehicleList, f => VehicleFaker.Generate(f.Random.Int(1, 5))) 
            .RuleFor(m => m.SocialSecurityNr, f => GenerateBirthdayCode(f)); 



        }
        private int GenerateBirthdayCode(Faker faker)
        {
            int year = faker.Date.Between(new DateTime(1950, 1, 1), new DateTime(2000, 12, 31)).Year;
            int month = faker.Random.Int(1, 12);
            int day = faker.Date.Between(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month))).Day;
            var birthdayCode = int.Parse($"{year}{month:00}{day:00}");
            return birthdayCode;
        }

        public List<Member> GenerateMember(int count)
        {
            return MemberFaker.Generate(count);
        }
    }
}
