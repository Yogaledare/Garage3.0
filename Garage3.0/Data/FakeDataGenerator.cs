using Bogus;
using Garage3._0.Models;

namespace Garage3._0.Data
{
    public class FakeDataGenerator
    {
        public Faker<Member> MemberFaker { get; set; }
        public Faker<Vehicle> VehicleFaker { get; set; }
        public Faker<VehicleType> VehicleTypeFaker { get; set; }
        private static int _nextMemberId = 1;
        private static int _nextVehicleId = 1;

        public FakeDataGenerator()
        {
            Randomizer.Seed = new Random(123);


            MemberFaker = new Faker<Member>()
                         .CustomInstantiator(f =>
                         {
                             var member = new Member
                             {
                                 MemberId = _nextMemberId++,
                                 SocialSecurityNr = GenerateBirthdayCode(f),
                                 Firstname = f.Name.FirstName(),
                                 Surname = f.Name.LastName(),
                                 VehicleList = new List<Vehicle>()
                             };


                             int numberOfVehicles = f.Random.Int(1, 5);
                             for (int i = 0; i < numberOfVehicles; i++)
                             {
                                 var vehicleFaker = CreateVehicleFaker(member);
                                 var vehicle = vehicleFaker.Generate();
                                 member.VehicleList.Add(vehicle);
                             }
                              
                             return member;
                         });

        }
 
        public Faker<Vehicle> CreateVehicleFaker(Member member)
        {
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType { VehicleTypeId = 1,VehicleTypeName = "Car", ParkingSpaceRequirement = 1, Vehicles = new List<Vehicle>() },
                new VehicleType { VehicleTypeId = 2, VehicleTypeName = "Truck", ParkingSpaceRequirement = 2 ,Vehicles = new List<Vehicle>()},
                new VehicleType {  VehicleTypeId = 3,VehicleTypeName = "Bus", ParkingSpaceRequirement = 3 , Vehicles = new List < Vehicle >()}
            };
            return new Faker<Vehicle>()
                .CustomInstantiator(f =>
                {
                    var vehicle = new Vehicle
                    {
                        VehicleId = _nextVehicleId++,
                        LicencePlate = f.Random.Replace("???###"),
                        Color = f.Commerce.Color(),
                        Brand = f.Vehicle.Manufacturer(),
                        Model = f.Vehicle.Model(),
                        NumWheels = 4,
                        MemberId = member.MemberId,
                        Member = member
                    };

                    var vehicleType = f.PickRandom(vehicleTypes);
                    vehicle.VehicleType = vehicleType;
                    vehicle.VehicleTypeId = vehicleType.VehicleTypeId;
                    vehicleType.Vehicles.Add(vehicle);

                    vehicle.ParkingEvent = null;

                    return vehicle;
                });
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
