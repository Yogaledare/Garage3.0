namespace Garage3._0.Models.ViewModels;

public class VehicleViewModel {
    public string OwnerFirstName { get; set; } = string.Empty;
    public string OwnerLastName { get; set; } = string.Empty;
    public string VehicleTypeName { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public DateTime? ParkingStartedDateTime { get; set; }
    public DateTime? ParkingEndedDateTime { get; set; }
    public TimeSpan ParkedTime {
        get {
            if (!ParkingStartedDateTime.HasValue) {
                return TimeSpan.Zero; 
            }

            var startTime = ParkingStartedDateTime.Value; 
            var endTime = ParkingEndedDateTime ?? DateTime.Now;

            return endTime - startTime; 
        }
    }
}