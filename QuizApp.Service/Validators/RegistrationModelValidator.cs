using FluentValidation;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Validators;

public class RegistrationModelValidator : CustomValidator<RegistrationViewModel>
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
}