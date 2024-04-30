using Garage3._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Data;

public class GarageDbContext : DbContext {
    public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options) {
    }

    public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    
    
}

