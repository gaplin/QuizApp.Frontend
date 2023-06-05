using FluentValidation;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Validators;

public class LoginModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginModelValidator()
    {
        RuleFor(credentials => credentials.Login)
            .NotEmpty();

        RuleFor(credentials => credentials.Password)
            .NotEmpty();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<LoginViewModel>.CreateWithOptions(
                (LoginViewModel)model, x => x.IncludeProperties(propertyName)
                )
            );
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}