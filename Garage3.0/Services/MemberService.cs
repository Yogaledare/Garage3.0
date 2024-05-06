using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Services;

public class MemberService : IMemberService
{
    private readonly GarageDbContext _context;

    public MemberService(GarageDbContext context)
    {
        _context = context;
    }

    public async Task<List<MemberViewModel>> GetAllMemberViewModelsAsync()
    {
        // Retrieve members and associated vehicles with their parking events and vehicle types
        var members = await _context.Members
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.VehicleType)
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.ParkingEvent)
            .ToListAsync();

        // Map members to view models
        return members.Select(m => new MemberViewModel
        {
            FirstName = m.Firstname,
            Surname = m.Surname,
            SocialSecurityNr = m.SocialSecurityNr.ToString(),
            Vehicles = m.VehicleList.Select(v => CreateVehicleViewModel(v, m)).ToList()
        }).ToList();
    }

    public VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member)
    {
        var vehicleTypeName = vehicle.VehicleType?.VehicleTypeName ?? "Unknown";
        var parkingStartedDateTime = vehicle.ParkingEvent?.ArrivalTime;
        var parkingEndedDateTime = vehicle.ParkingEvent?.DepartureTime;
        
        return new VehicleViewModel
        {
            OwnerFirstName = member.Firstname,
            OwnerLastName = member.Surname,
            VehicleTypeName = vehicleTypeName,
            LicensePlate = vehicle.LicencePlate ?? string.Empty,
            ParkingStartedDateTime = parkingStartedDateTime,
            ParkingEndedDateTime = parkingEndedDateTime
        };
    }
}