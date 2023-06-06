using FluentValidation;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Validators;

public class LoginModelValidator : CustomValidator<LoginViewModel>
{
    public LoginModelValidator()
    {
        RuleFor(credentials => credentials.Login)
            .NotEmpty();

        RuleFor(credentials => credentials.Password)
            .NotEmpty();
    }
}