using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var vehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.Member)
            .ThenInclude(m => m.Membership)
            .Include(v => v.ParkingEvent)
            .Where(v => v.ParkingEvent != null)
            .ToListAsync();

        if (!string.IsNullOrEmpty(searchQuery)) {
            var queries = searchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            vehicles = vehicles
                .Where(v =>
                    queries.Any(query =>
                        v.LicencePlate.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        v.VehicleType.VehicleTypeName.Contains(query, StringComparison.OrdinalIgnoreCase)
                    )
                )
                .ToList();
        }

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
    
    public void CreateVehicle(CreateVehicleViewModel input) {
        Console.WriteLine(input.LicencePlate);
        Console.WriteLine(input.Color);
        Console.WriteLine(input.Brand);
        Console.WriteLine(input.Model);
        Console.WriteLine(input.NumWheels);
        Console.WriteLine(input.MemberId);
        Console.WriteLine(input.VehicleTypeId);

        var vehicle = new Vehicle {
            LicencePlate = input.LicencePlate,
            Color = input.Color,
            Brand = input.Brand,
            Model = input.Model,
            NumWheels = input.NumWheels ?? 0,
            VehicleTypeId = input.VehicleTypeId ?? 0,
            MemberId = input.MemberId ?? 0,
        };

        _context.Vehicles.Add(vehicle);
        _context.SaveChanges();
    }
    
    public List<SelectListItem> GetVehicleTypeOptions() {
        return _context.VehicleTypes
            .Select(vt => new SelectListItem {
                Text = vt.VehicleTypeName,
                Value = vt.VehicleTypeId.ToString()
            })
            .ToList();
    }
    
    
    public void CreateVehicleType(CreateVehicleTypeViewModel model) {
        var vehicleType = new VehicleType {
            VehicleTypeName = model.VehicleTypeName,
            ParkingSpaceRequirement = model.ParkingSpaceRequirement
        };

        foreach (var wheels in model.AllowedWheelNumbers) {
            vehicleType.WheelConfigurations.Add(new WheelConfiguration {
                NumWheels = wheels
            });
        }

        _context.VehicleTypes.Add(vehicleType);
        _context.SaveChanges();
    }
    
    
    
    
}

