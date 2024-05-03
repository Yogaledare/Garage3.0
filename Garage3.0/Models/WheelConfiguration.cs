namespace Garage3._0.Models;

public class WheelConfiguration() {
    public int NumWheels { get; set; }
    
    // FK
    public int VehicleTypeId { get; set; }
    
    // Nav
    public VehicleType VehicleType { get; set; }
}