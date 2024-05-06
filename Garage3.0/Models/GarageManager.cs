using Garage3._0.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Models
{
    public class GarageManager
    {
        private readonly GarageDbContext _context;
        private Random random;
        private const int TOTALPARKINGPLACE = 50;
        private int currentUsed = 0;
        private int limitedWidth = 10; //5x10
        private HashSet<int> currentUsedSpots = new HashSet<int>();

        public GarageManager(GarageDbContext context)
        {
            _context = context;
            random = new Random();
            Init();
        }
        private void Init()
        {
            if (_context.parkingPlaces.Count() >= 0)
            {
                foreach (var p in _context.parkingPlaces.Select(p => p.ParkingPlaceNr))
                {
                    currentUsedSpots.Add(p);
                }
            }
        }
        public ParkingEvent? ParkVehicle(int id)
        {
            //en 5x10 garage
            //random place
            //check for vehicle type for place set up
            var vehicle = _context.Vehicles.Find(id);
            if(vehicle == null)
            {
                return null;
            }
            var vtId = vehicle.VehicleTypeId;
            var vtype = _context.VehicleTypes.Find(vtId);
            if(vtype ==null)
            {
                return null;
            }
            else
            {
                var placeTaken = vtype.ParkingSpaceRequirement;
                var availableSpots = FindAvailableParingSpots(placeTaken);
                if (availableSpots != null)
                {
                    //actual implementation 
                    var tempSpotsList = new List<ParkingPlace>();
                    ParkingEvent parkingEvent = new ParkingEvent();
                    foreach (var s in availableSpots)
                    {
                        var place = new ParkingPlace { ParkingPlaceNr = s };
                        place.ParkingEvent = parkingEvent;
                        place.ParkingEventID = parkingEvent.ParkingEventID;
                        tempSpotsList.Add(place);
                    }
                    parkingEvent.VehicleID = vehicle.VehicleId;
                    parkingEvent.Vehicle = vehicle;
                    parkingEvent.ParkingPlaces = tempSpotsList;
                    parkingEvent.ArrivalTime = DateTime.Now;
                
                    //Add to database
                    _context.parkingPlaces.AddRange(tempSpotsList);
                    _context.ParkingEvents.AddRange(parkingEvent);
                    
                    _context.SaveChanges();
                    //_context.Entry(vehicle).State = EntityState.Modified;
                    vehicle.ParkingEventID = parkingEvent.ParkingEventID;
                    vehicle.ParkingEvent = parkingEvent;
                    _context.SaveChanges();
                    return parkingEvent;
                }
                else
                {
                    return null;
                }
            }
            //search for empty place, check adjacent spot, see if it's empty or is side(out of range)
     
        }

        private List<int>? FindAvailableParingSpots(int placeTaken)
        {
            var tempList = new List<int>();
            for (int i = 1; i <= TOTALPARKINGPLACE; i++)
            {
                if (!currentUsedSpots.Contains(i))
                {
                    if (placeTaken == 1)
                    {
                        tempList.Add(i);
                        return tempList;  // Found the needed single spot and exit the loop
                    }
                    else
                    {
                        bool spotFound = true;
                        for (int j = i + 1; j <= i + placeTaken - 1; j++)
                        {
                            if (i % limitedWidth == 0)
                            {
                                spotFound = false;
                                break;
                            }
                            if (currentUsedSpots.Contains(j))
                            {
                                spotFound = false;
                                i = j; // Skip to the next position after the taken spot
                                break;
                            }
                        }
                        if (spotFound)
                        {
                            for (int k = i; k < i + placeTaken; k++)
                            {
                                tempList.Add(k);
                            }
                            return tempList; // Found all the needed spots and exit the loop
                        }
                    }
                }
            }
            return null;
        }


    }
}
