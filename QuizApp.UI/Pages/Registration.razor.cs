using Microsoft.AspNetCore.Components;
using MudBlazor;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Interface.APIClient;
using QuizApp.Service.Validators;

namespace QuizApp.UI.Pages;

public partial class Registration
{
    private readonly RegistrationViewModel _registrationModel = new();
    private readonly RegistrationModelValidator _registrationValidator = new();

    private MudForm _form = null!;
    private string? _apiErrorMessage;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    private async Task RegisterAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            var (token, errors) = await ApiClient.RegisterAndGetTokenAsync(_registrationModel);

            if(token != null)
            {
                NavigationManager.NavigateTo("/registration-confirmation");
            } else if(errors != null)
            {
                _apiErrorMessage = "";
                foreach (var item in errors)
                {
                    foreach (var errorMessage in item.Value)
                    {
                        _apiErrorMessage += $"{errorMessage} | ";
                    }
                }
            }
        }
    }
}