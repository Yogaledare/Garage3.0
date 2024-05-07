namespace Garage3._0.Models
{
    public interface IGarageManager
    {
        int GetLimited();
        int GetTotalPlaces();
        ParkingEvent? ParkVehicle(int id);
        ParkingEvent? UnParkVehicle(int id);
    }
}