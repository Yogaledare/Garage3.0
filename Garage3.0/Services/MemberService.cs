using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Services;

public class MemberService : IMemberService {
    private readonly GarageDbContext _context;

    public MemberService(GarageDbContext context) {
        _context = context;
    }
    
    
    


    public async Task<MembersIndexViewModel> GetMembersIndexViewModelAsync(string? searchQuery) {
        var memberViewModels = await GetMemberViewModelsAsync(searchQuery);
        var model = new MembersIndexViewModel {
            Members = memberViewModels,
            SearchQuery = searchQuery ?? string.Empty,
        };
        return model; 
    }
    
    public async Task<List<MemberViewModel>> GetMemberViewModelsAsync(string? searchQuery) {
        // Retrieve members and associated vehicles with their parking events and vehicle types
        var membersQuery = _context.Members
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.VehicleType)
            .Include(m => m.VehicleList)
            .ThenInclude(v => v.ParkingEvent)
            .AsQueryable();


        if (!string.IsNullOrEmpty(searchQuery)) {
            membersQuery = membersQuery.Where(m => (m.Firstname + " " + m.Surname).Contains(searchQuery)); 
        }

        var members = await membersQuery.ToListAsync();
        // Map members to view models
        var memberViewModels = members.Select(m => new MemberViewModel {
                MemberId = m.MemberId,
                FirstName = m.Firstname,
                Surname = m.Surname,
                SocialSecurityNr = m.SocialSecurityNr.ToString(),
                Vehicles = m.VehicleList.Select(v => CreateVehicleViewModel(v, m)).ToList()
            })
            .ToList();

        memberViewModels = memberViewModels
            .OrderBy(m => m.FirstName.Length >= 2 ? m.FirstName[..2] : m.FirstName)
            .ToList(); 

        return memberViewModels; 
    }

    public VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member) {
        var vehicleTypeName = vehicle.VehicleType?.VehicleTypeName ?? "Unknown";
        var parkingStartedDateTime = vehicle.ParkingEvent?.ArrivalTime;
        var parkingEndedDateTime = vehicle.ParkingEvent?.DepartureTime;

        return new VehicleViewModel {
            VehicleId = vehicle.VehicleId,
            OwnerMemberId = member.MemberId,
            OwnerFirstName = member.Firstname,
            OwnerLastName = member.Surname,
            VehicleTypeName = vehicleTypeName,
            LicensePlate = vehicle.LicencePlate ?? string.Empty,
            ParkingStartedDateTime = parkingStartedDateTime,
            ParkingEndedDateTime = parkingEndedDateTime
        };
    }
}