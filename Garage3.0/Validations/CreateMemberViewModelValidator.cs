using FluentValidation;
using Garage3._0.Models.ViewModels;

namespace Garage3._0.Validations;

public class CreateMemberViewModelValidator : AbstractValidator<CreateMemberViewModel> {
    public CreateMemberViewModelValidator() {
        var minLength = 2;
        var maxLength = 50;

        RuleFor(x => x.Firstname)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(minLength, maxLength)
            .WithMessage($"First name must be between {minLength} and {maxLength} characters.");
    }
}