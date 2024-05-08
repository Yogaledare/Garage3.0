using Garage3._0.Models;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Services;

public interface IVehicleService {
    VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member, Membership membership);

    // IEnumerable<VehicleViewModel> GetParkedVehicleViewModels(string? searchQuery);

    Task<VehiclesIndexViewModel> GetVehiclesIndexViewModelAsync(string? searchQuery);
}