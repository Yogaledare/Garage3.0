namespace Garage3._0.Models.ViewModels;

public class CreateVehicleTypeViewModel {
    public string VehicleTypeName { get; set; }
    public int ParkingSpaceRequirement { get; set; }
    public List<int> AllowedWheelNumbers { get; set; } = []; 
}