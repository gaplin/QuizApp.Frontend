using FluentValidation;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Validators;

public class RegistrationModelValidator : AbstractValidator<RegistrationViewModel>
{
    public RegistrationModelValidator()
    {
        RuleFor(_ => _.UserName)
           .NotEmpty()
           .MinimumLength(5)
           .MaximumLength(20);

        RuleFor(_ => _.Login)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(20);

        RuleFor(_ => _.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(20);

        RuleFor(_ => _.ConfirmPassword)
            .Equal(_ => _.Password).WithMessage("ConfirmPassword must equal Password");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<RegistrationViewModel>.CreateWithOptions(
                (RegistrationViewModel) model, x => x.IncludeProperties(propertyName)
                )
            );
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}