using FluentValidation;
using Garage3._0.Data;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Validations;

public class CreateVehicleViewModelValidator : AbstractValidator<CreateVehicleViewModel> {
    private readonly GarageDbContext _context;

    public CreateVehicleViewModelValidator(GarageDbContext context) {
        _context = context;
        RuleFor(x => x.LicencePlate)
            .NotEmpty()
            .WithMessage("License plate is required.");

        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Brand is required.");

        RuleFor(x => x.LicencePlate)
            .Must(BeUniquelicensePlate)
            .WithMessage("License plate must be unique."); 
        
        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Color is required.");
        
        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Brand is required.");
        
        
        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required.");
        
        RuleFor(x => x.NumWheels)
            .NotEmpty()
            .WithMessage("Number of wheels is required.")      
            .Must(BeAllowedWheelConfiguration)
            .WithMessage("The number of wheels isn't allowed for the selected vehicle type.");

        ;
    }

    private bool BeAllowedWheelConfiguration(CreateVehicleViewModel viewModel, int? numWheels) {
        // Ensure VehicleTypeId is provided
        if (viewModel.VehicleTypeId == null || !numWheels.HasValue) return false;

        // Retrieve the WheelConfigurations associated with the given VehicleTypeId
        var vehicleType  = _context.VehicleTypes
            .Include(vt => vt.WheelConfigurations)
            .FirstOrDefault(vt => vt.VehicleTypeId == viewModel.VehicleTypeId);

        if (vehicleType == null) return false;

        var allowedConfigurations = vehicleType.WheelConfigurations
            .Select(wc => wc.NumWheels)
            .ToList(); 

        return allowedConfigurations.Contains(numWheels.Value);
    }

    private bool BeUniquelicensePlate(string? licensePlate) {
        if (string.IsNullOrWhiteSpace(licensePlate)) return false;
        // Use synchronous database query
        return !_context.Vehicles.Any(v => v.LicencePlate == licensePlate);
    }
}