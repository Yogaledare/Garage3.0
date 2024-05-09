using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3._0.Services;

public interface IVehicleService {
    VehicleViewModel CreateVehicleViewModel(Vehicle vehicle, Member member, Membership membership);
    // IEnumerable<VehicleViewModel> GetParkedVehicleViewModels(string? searchQuery);
    Task<VehiclesIndexViewModel> GetVehiclesIndexViewModelAsync(string? searchQuery);
    void CreateVehicle(CreateVehicleViewModel input);
    List<SelectListItem> GetVehicleTypeOptions();
    void CreateVehicleType(CreateVehicleTypeViewModel model);
}