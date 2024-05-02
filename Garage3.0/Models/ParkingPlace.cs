namespace Garage3._0.Models
{
    public class ParkingPlace
    {
        public int ParkingPlaceId { get; set; }
        public int ParkingPlaceNr { get; set; }  
        public int ParkingEventID { get; set; }
        public ParkingEvent ParkingEvent { get; set; }
    }
}
