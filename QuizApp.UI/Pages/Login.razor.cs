using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Auth;
using QuizApp.Service.Interface.APIClient;
using QuizApp.Service.Validators;

namespace QuizApp.UI.Pages;

public partial class Login : IDisposable
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

    [Inject]
    private IKeyInterceptorFactory KeyInterceptorFactory { get; set; } = null!;

    private IKeyInterceptor? _keyInterceptor;

    private KeyboardEvent? OnEnterPressedHandler;

    protected override async Task OnInitializedAsync()
    {
        OnEnterPressedHandler = async (_) =>
        {
            await LoginAsync();
            await InvokeAsync(StateHasChanged);
        };
        var user = (await AuthState).User;
        if (user!.Identity!.IsAuthenticated)
        {
            NavManager.NavigateTo("/");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            _keyInterceptor = KeyInterceptorFactory.Create();
            await _keyInterceptor.Connect("formId", new KeyInterceptorOptions
            {
                TargetClass = "mud-input-slot",
                Keys = [new() { Key = "Enter", SubscribeDown = true }]
            });
            _keyInterceptor.KeyDown += OnEnterPressedHandler; 
        }
        await base.OnAfterRenderAsync(firstRender);
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

    protected virtual void Dispose(bool disposing)
    {
        if (_keyInterceptor is not null)
        {
            if (disposing)
            {
                _keyInterceptor!.KeyDown -= OnEnterPressedHandler;
                _keyInterceptor!.Dispose();
            }
            _keyInterceptor = null;
        }
    }

    ~Login()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
