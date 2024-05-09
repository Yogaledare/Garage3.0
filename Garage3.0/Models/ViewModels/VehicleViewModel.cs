namespace Garage3._0.Models.ViewModels;

public class VehicleViewModel {
    public int VehicleId { get; set; }
    public int OwnerMemberId { get; set; }

    public Membership Membership { get; set; }
    public string OwnerFirstName { get; set; } = string.Empty;
    public string OwnerLastName { get; set; } = string.Empty;
    public string VehicleTypeName { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public DateTime? ParkingStartedDateTime { get; set; }
    public DateTime? ParkingEndedDateTime { get; set; }
    public int ParkingPlaces {  get; set; }
    public string OwnerName => $"{OwnerFirstName} {OwnerLastName}";

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

    public bool IsParked => ParkingStartedDateTime.HasValue && !ParkingEndedDateTime.HasValue;

    public string ParkingStartDisplay => ParkingStartedDateTime.HasValue
        ? ParkingStartedDateTime.Value.ToString("yyyy-MM-dd HH:mm")
        : "N/A";

    public string ParkingEndDisplay => ParkingEndedDateTime.HasValue
        ? ParkingEndedDateTime.Value.ToString("yyyy-MM-dd HH:mm")
        : "N/A";

}