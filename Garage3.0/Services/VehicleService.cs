using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Services;

public class VehicleService : IVehicleService {
    private readonly GarageDbContext _context;

    public VehicleService(GarageDbContext context) {
        _context = context;
    }


    public VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member, Membership membership) {
        var vehicleTypeName = vehicle.VehicleType?.VehicleTypeName ?? "Unknown";
        var parkingStartedDateTime = vehicle.ParkingEvent?.ArrivalTime;
        var parkingEndedDateTime = vehicle.ParkingEvent?.DepartureTime;

        return new VehicleViewModel {
            VehicleId = vehicle.VehicleId,
            OwnerMemberId = member.MemberId,
            OwnerFirstName = member.Firstname,
            OwnerLastName = member.Surname,
            MembershipType = membership.Type,
            VehicleTypeName = vehicleTypeName,
            LicensePlate = vehicle.LicencePlate ?? string.Empty,
            ParkingStartedDateTime = parkingStartedDateTime,
            ParkingEndedDateTime = parkingEndedDateTime
        };
    }

    private async Task<IEnumerable<VehicleViewModel>> GetParkedVehicleViewModels(string? searchQuery) {
        var queries = searchQuery?.Split(' ') ?? [];

        var vehiclesQuery = _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.Member)
            .ThenInclude(m => m.Membership)
            .Include(v => v.ParkingEvent)
            .Where(v => v.ParkingEvent != null);

        if (queries.Length > 0) {
            vehiclesQuery = vehiclesQuery.Where(v =>
                queries.Any(query =>
                    v.LicencePlate.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    v.VehicleType.VehicleTypeName.Contains(query, StringComparison.OrdinalIgnoreCase)
                )
            );
        }

        var vehicles = await vehiclesQuery.ToListAsync(); 
        
        var vehicleViewModels = vehicles
            .Select(v => CreateVehicleViewModel(v, v.Member, v.Member.Membership))
            .ToList();

        return vehicleViewModels;
    }

    public async Task<VehiclesIndexViewModel> GetVehiclesIndexViewModelAsync(string? searchQuery) {
        var vehicleViewModels = await GetParkedVehicleViewModels(searchQuery);
        var model = new VehiclesIndexViewModel {
            Vehicles = vehicleViewModels,
            SearchQuery = searchQuery
        };

        return model; 
    }
}