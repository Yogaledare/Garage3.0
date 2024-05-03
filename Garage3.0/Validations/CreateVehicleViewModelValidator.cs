using FluentValidation;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validations;

public class CreateVehicleViewModelValidator : AbstractValidator<CreateVehicleViewModel> {
    
    public CreateVehicleViewModelValidator() {
        RuleFor(x => x.LicencePlate)
            .NotEmpty().WithMessage("License plate is required.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.");

        
        
        
        
        
    }
    
    
}