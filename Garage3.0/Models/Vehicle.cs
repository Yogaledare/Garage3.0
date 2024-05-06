namespace Garage3._0.Models;

public class Vehicle {
    
    public int VehicleId { get; set; }
    public string? LicencePlate { get; set; }
    public string? Color { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int NumWheels { get; set; }
    //Entities relations
    public int VehicleTypeId { get; set; }
    
    //nav prop
    public VehicleType VehicleType { get; set; }
    public int MemberId { get; set; }
    public Member  Member { get; set; }
    public int? ParkingEventID { get; set; } 
    public ParkingEvent? ParkingEvent { get; set; } 
}

