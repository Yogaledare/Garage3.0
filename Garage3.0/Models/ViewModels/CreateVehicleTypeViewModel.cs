namespace Garage3._0.Models.ViewModels;

public class CreateVehicleTypeViewModel {
    public int? MemberId { get; set; }
    public string VehicleTypeName { get; set; }
    public int ParkingSpaceRequirement { get; set; }
    public List<int> AllowedWheelNumbers { get; set; } = []; 
}