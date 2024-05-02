namespace Garage3._0.Models
{
    public class ParkingEvent
    {
        public int ParkingEventID { get; set; }
        public int VehicleID { get; set; }
        public Vehicle Vehicle { get; set; }
        public ICollection<ParkingPlace> ParkingPlaces { get; set; }

        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
