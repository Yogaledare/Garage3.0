using Garage3._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Data;

public class GarageDbContext : DbContext {
    public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options) {
    }

    public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        
        // Seed data method calls
        SeedVehicleTypes(modelBuilder);
        SeedVehicles(modelBuilder);
    }

    private void SeedVehicleTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleType>().HasData(
            new VehicleType { VehicleTypeId = 1, VehicleTypeName = "Car", ParkingSpaceRequirement = 1 },
            new VehicleType { VehicleTypeId = 2, VehicleTypeName = "Motorcycle", ParkingSpaceRequirement = 1 },
            new VehicleType { VehicleTypeId = 3, VehicleTypeName = "Bus", ParkingSpaceRequirement = 2 }
        );
    }

    private void SeedVehicles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle { VehicleId = 1, LicencePlate = "ABC123", Color = "Blue", Brand = "Toyota", Model = "Corolla", NumWheels = 4, VehicleTypeId = 1 },
            new Vehicle { VehicleId = 2, LicencePlate = "XYZ789", Color = "Red", Brand = "Honda", Model = "Civic", NumWheels = 4, VehicleTypeId = 1 },
            new Vehicle { VehicleId = 3, LicencePlate = "HHH777", Color = "Black", Brand = "Harley", Model = "Davidson", NumWheels = 2, VehicleTypeId = 2 }
        );
    }
    
}

