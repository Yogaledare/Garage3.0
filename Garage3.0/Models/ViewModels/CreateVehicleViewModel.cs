using Microsoft.AspNetCore.Mvc.Rendering;

namespace Garage3._0.Models.ViewModels;

public class CreateVehicleViewModel {
    public string? LicencePlate { get; set; }
    public string? Color { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? NumWheels { get; set; }
    // public VehicleType VehicleType { get; set; }
    public int? MemberId { get; set; }
    public int? VehicleTypeId { get; set; }
    public IEnumerable<SelectListItem> VehicleTypeOptions { get; set; } = []; 
}