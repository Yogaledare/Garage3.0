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
        public Dictionary<int,Dictionary<string,int>> parkingMap = new Dictionary<int,Dictionary<string,int>>();

        public GarageManager(GarageDbContext context)
        {
            _context = context;
            random = new Random();
            Init();
        }
        private void Init()
        {
            //if (_context.parkingPlaces.Count() >= 0)
            //{
            //    foreach (var p in _context.parkingPlaces.Select(p => p.ParkingPlaceNr))
            //    {
            //        currentUsedSpots.Add(p);
            //    }
            //}
            if(_context.ParkingEvents.Count() > 0)
            {
                var pEvents = _context.ParkingEvents.Include(p => p.ParkingPlaces);
				foreach (var pe in pEvents)
				{
					var parkingMapKey = pe.ParkingPlaces.First().ParkingPlaceNr;

					if (!parkingMap.ContainsKey(parkingMapKey))
					{
						parkingMap.Add(parkingMapKey, new Dictionary<string, int>());
					}
                    var vId = pe.VehicleID;
                    var v = _context.Vehicles.Find(vId);

                    if(v != null)
                    {
                        var vtId = v.VehicleTypeId;
                        var vt = _context.VehicleTypes.FirstOrDefault(vt => vt.VehicleTypeId == vtId);
                        parkingMap[parkingMapKey][vt!.VehicleTypeName] = vt.ParkingSpaceRequirement;
                    }
					

					foreach (var p in pe.ParkingPlaces)
					{
						currentUsedSpots.Add(p.ParkingPlaceNr);
					}
				}

			}
		}
        public ParkingEvent? ParkVehicle(int id)
        {
            //check for vehicle type for place set up
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {
                return null;
            }
            var vtId = vehicle.VehicleTypeId;
            var vtype = _context.VehicleTypes.Find(vtId);
            if (vtype == null)
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
                        currentUsedSpots.Add(s);
                        place.ParkingEvent = parkingEvent;
                        place.ParkingEventID = parkingEvent.ParkingEventID;
                        tempSpotsList.Add(place);
                    }
                    parkingEvent.VehicleID = vehicle.VehicleId;
                    parkingEvent.Vehicle = vehicle;
                    parkingEvent.ParkingPlaces = tempSpotsList;
                    parkingEvent.ArrivalTime = DateTime.Now;

                    //Save info in Dictionary
                    parkingMap.Add(availableSpots[0], new Dictionary<string, int>() { { vtype.VehicleTypeName, vtype.ParkingSpaceRequirement } });


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

        public List<int>? FindAvailableParingSpots(int placeTaken)
        {          
            if (placeTaken == 1)
            {
                int randomPlace;
                var tempList = new List<int>();
                do
                {
                    randomPlace = random.Next(1, TOTALPARKINGPLACE + 1);
                } while (currentUsedSpots.Contains(randomPlace));
                tempList.Add(randomPlace);
                return tempList;
            }
            else if (placeTaken > 1)
            {
                var tempMultiList = new List<List<int>>();
                for (int i = 1; i <= TOTALPARKINGPLACE; i++)
                {
                    if (!currentUsedSpots.Contains(i))
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
                            //Find all possible places
                            var temp = new List<int>();
                            for (int k = i; k < i + placeTaken; k++)
                            {
                                temp.Add(k);
                            }
                            tempMultiList.Add(temp);
                            i = i + placeTaken-1;
                            //spotFound = false;
                        }
                    }
                }
                int randomList = random.Next(0, tempMultiList.Count);
                return tempMultiList[randomList];
            }
            else
            {
                return null;
            }
        }

        public int GetTotalPlaces()
        {
            return TOTALPARKINGPLACE;
        }
        public int GetLimited()
        {
            return limitedWidth;
        }
    }
}
