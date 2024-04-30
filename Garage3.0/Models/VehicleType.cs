namespace Garage3._0.Models;

public class VehicleType {
    public int VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; }
    public int ParkingSpaceRequirement { get; set; }
    public List<Vehicle> Vehicles { get; set; } = []; 
}

