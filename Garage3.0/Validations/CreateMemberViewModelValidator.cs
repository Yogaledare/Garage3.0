using FluentValidation;
using Garage3._0.Data;
using Garage3._0.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Garage3._0.Validations;

public class CreateMemberViewModelValidator : AbstractValidator<CreateMemberViewModel> {

    private readonly GarageDbContext _context;


    // public CreateMemberViewModelValidator(GarageDbContext context) {
        // _context = context; 
    // }
    
    public CreateMemberViewModelValidator(GarageDbContext context) {      
        _context = context; 

        const int minNameLength = 2;
        const int maxNameLength = 50;
        const string nameRegex = @"^[a-zA-Z\-\'\s]+$";
   
        const string ssnRegex = @"^(19\d{2}|20(?:0[0-5]))(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{4}$";

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
             .Matches(ssnRegex)
             .WithMessage("Invalid Social Security Number. Format 19YYMMDD1234.")
             .Must(BeUniqueSsn)
             .WithMessage("Social Security Number already exists in the database.");
        
    }
    
    
    
    private bool BeUniqueSsn(string? ssn) {
        if (string.IsNullOrWhiteSpace(ssn)) return false;
        // Use synchronous database query
        return !_context.Members.Any(m => m.SocialSecurityNr == ssn);
    }
    
    
    // private async Task<bool> BeUniqueSsn(string? ssn, CancellationToken cancellationToken)
    // {
    //     if (string.IsNullOrWhiteSpace(ssn)) return false;
    //     return !await _context.Members.AnyAsync(m => m.SocialSecurityNr == ssn, cancellationToken);
    // }
    
    
}