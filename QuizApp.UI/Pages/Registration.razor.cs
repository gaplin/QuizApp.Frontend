using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Interface.APIClient;
using QuizApp.Service.Validators;

namespace QuizApp.UI.Pages;

public sealed partial class Registration : IAsyncDisposable
{
    private readonly RegistrationViewModel _registrationModel = new();
    private readonly RegistrationModelValidator _registrationValidator = new();

    private MudForm _form = null!;
    private string? _apiErrorMessage;

    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;
    [Inject]
    private IKeyInterceptorService? KeyInterceptorService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var user = (await AuthState).User;
        if (user!.Identity!.IsAuthenticated)
        {
            NavManager.NavigateTo("/");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var options = new KeyInterceptorOptions(
                targetClass: "mud-input-slot",
                keys: new KeyOptions(
                    key: "Enter",
                    subscribeDown: true
                    )
                );
            await KeyInterceptorService!.SubscribeAsync("formId", options, keyDown: OnEnterPressed);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task OnEnterPressed(KeyboardEventArgs args)
    {
        await RegisterAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task RegisterAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            var (token, errors) = await ApiClient.RegisterAndGetTokenAsync(_registrationModel);

            if (token != null)
            {
                NavManager.NavigateTo("/registration-confirmation");
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
    public async ValueTask DisposeAsync()
    {
        if (KeyInterceptorService is not null)
        {
            await KeyInterceptorService.UnsubscribeAsync("formId");
            KeyInterceptorService = null;
        }
        GC.SuppressFinalize(this);
    }
}