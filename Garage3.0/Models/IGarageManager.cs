using Garage3._0.Models.ViewModels;

namespace Garage3._0.Models
{
    public interface IGarageManager
    {
        int GetLimited();
        int GetTotalPlaces();
        ParkingEvent? ParkVehicle(int id);
		VehicleViewModel? UnParkVehicle(int id);
    }
}