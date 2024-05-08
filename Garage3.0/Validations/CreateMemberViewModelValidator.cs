using FluentValidation;
using Garage3._0.Models.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Garage3._0.Data;

namespace Garage3._0.Validations;

public class CreateMemberViewModelValidator : AbstractValidator<CreateMemberViewModel> {

    private readonly GarageDbContext dbContext;
    public CreateMemberViewModelValidator(GarageDbContext dbContext)
    {

        this.dbContext = dbContext;

        const int minNameLength = 2;
        const int maxNameLength = 50;
        const string nameRegex = @"^[a-zA-Z\-\'\s]+$";
        //const string ssnRegex = @"^\d{8}$";
        //const int ssnLength = 8;
        const string ssnRegex = @"^(19\d{2}|20(?:0[0-5]))(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])$";

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
            .WithMessage("Surname must only contain letters, hyphens, apostrophes, and spaces.")
            .Must((x, surname) => surname != x.Firstname)
            .WithMessage("First name and surname must be different.");

        RuleFor(x => x.SocialSecurityNr)
             .NotEmpty()
             .WithMessage("Social security number is required.")
             .Must(ssn => Regex.IsMatch(ssn.ToString(), ssnRegex))
             .WithMessage("Invalid Social Security Number. Format 19YYMMDD.");
        //.MustAsync(async (ssn, cancellationToken) =>
        //{

        //    var existingMember = await dbContext.Members.FirstOrDefaultAsync(m => m.SocialSecurityNr == ssn);
        //    return existingMember == null;
        //})
        //.WithMessage("Social Security Number already exists in the database.");
        //}
    }
}
