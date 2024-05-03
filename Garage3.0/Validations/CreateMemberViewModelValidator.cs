using FluentValidation;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validations;

public class CreateMemberViewModelValidator : AbstractValidator<CreateMemberViewModel> {
    public CreateMemberViewModelValidator() {
        const int minNameLength = 2;
        const int maxNameLength = 50;
        const string nameRegex = @"^[a-zA-Z\-\'\s]+$";
        const string ssnRegex = @"^\d{6}-\d{4}$";

        RuleFor(x => x.Firstname)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(minNameLength, maxNameLength)
            .WithMessage($"First name must be between {minNameLength} and {maxNameLength} characters.")
            .Matches(nameRegex)
            .WithMessage("First name must only contain letters, hyphens, apostrophes, and spaces.");

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("Surname is required.")
            .Length(minNameLength, maxNameLength)
            .WithMessage($"Surname must be between {minNameLength} and {maxNameLength} characters.")
            .Matches(nameRegex)
            .WithMessage("Surname must only contain letters, hyphens, apostrophes, and spaces.");

        RuleFor(x => x.SocialSecurityNr)
            .NotEmpty()
            .WithMessage("Social security number is required.")
            .Matches(ssnRegex)
            .WithMessage("Social Security Number must be in the format 123456-7890.");
    }
}