namespace Garage3._0.Models;

public class Vehicle {
    
    public int VehicleId { get; set; }
    public string? LicencePlate { get; set; }
    public string? Color { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int NumWheels { get; set; }
    public int VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; }
}

