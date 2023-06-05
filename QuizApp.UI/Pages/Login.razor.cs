using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Auth;
using QuizApp.Service.Interface.APIClient;
using QuizApp.Service.Validators;

namespace QuizApp.UI.Pages;

public partial class Login
{
    private readonly LoginViewModel _loginModel = new();
    private readonly LoginModelValidator _loginValidator = new();

    private MudForm _form = null!;
    private string? _apiErrorMessage;

    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        if (user!.Identity!.IsAuthenticated)
        {
            NavManager.NavigateTo("/");
        }
    }

    private async Task LoginAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            var (token, errors) = await ApiClient.LogInAndGetTokenAsync(_loginModel);

            if (token != null)
            {
                await LocalStorageService.SetItemAsync("jwt-access-token", token);
                (AuthStateProvider as CustomAuthProvider)!.NotifyAuthState();
            }
            else if (errors != null)
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
