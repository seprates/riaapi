using FluentValidation;
using RIA.API.Dtos;

namespace RIA.API.Validations;

public class CustomerCreateValidator : BaseValidator<CustomerCreateDto>
{
    public CustomerCreateValidator()
    {
        When(x => x != null, () =>
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(1);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Age).GreaterThanOrEqualTo(18).LessThanOrEqualTo(120)
                .WithMessage("Valid Age is between 18 and 120.");
        });
    }
}