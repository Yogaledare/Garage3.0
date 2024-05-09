using FluentValidation;
using Garage3._0.Data;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validations
{
    public class CreateVehicleTypeViewModelValidator : AbstractValidator<CreateVehicleTypeViewModel>
    {
        private readonly GarageDbContext _context;

        public CreateVehicleTypeViewModelValidator(GarageDbContext context)
        {
            _context = context;

            // Rule for VehicleTypeName: required and must be unique
            RuleFor(x => x.VehicleTypeName)
                .NotEmpty()
                .WithMessage("Vehicle Type Name is required.")
                .Must(BeUniqueVehicleTypeName)
                .WithMessage("Vehicle Type Name already exists in the database.");

            // Rule for ParkingSpaceRequirement: required and must be 0 or positive
            RuleFor(x => x.ParkingSpaceRequirement)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Parking Space Requirement must be 0 or positive.");

            // Rule for AllowedWheelNumbers: required and must contain positive integers
            RuleFor(x => x.AllowedWheelNumbers)
                .NotEmpty()
                .WithMessage("Allowed Wheel Numbers cannot be empty.")
                .Must(numbers => numbers.All(n => n > 0))
                .WithMessage("All wheel numbers must be greater than 0.");
        }

        // Custom method to check if the Vehicle Type Name is unique in the database
        private bool BeUniqueVehicleTypeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // Use a case-insensitive query to avoid duplicate names with different casing
            return !_context.VehicleTypes.Any(v => v.VehicleTypeName.ToLower() == name.ToLower());
        }
    }
}