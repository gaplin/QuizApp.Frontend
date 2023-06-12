using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Interface.APIClient;
using QuizApp.Service.Validators;

namespace QuizApp.UI.Pages;

public partial class Registration : IDisposable
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
    private IKeyInterceptorFactory KeyInterceptorFactory { get; set; } = null!;

    private IKeyInterceptor? _keyInterceptor;

    private KeyboardEvent? OnEnterPressedHandler;

    protected override async Task OnInitializedAsync()
    {
        OnEnterPressedHandler = async (_) =>
        {
            await RegisterAsync();
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
        if (firstRender)
        {
            _keyInterceptor = KeyInterceptorFactory.Create();
            await _keyInterceptor.Connect("formId", new KeyInterceptorOptions
            {
                TargetClass = "mud-input-slot",
                Keys = new List<KeyOptions> { new() { Key = "Enter", SubscribeDown = true } }
            });
            _keyInterceptor.KeyDown += OnEnterPressedHandler;
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task RegisterAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            var (token, errors) = await ApiClient.RegisterAndGetTokenAsync(_registrationModel);

            if(token != null)
            {
                NavManager.NavigateTo("/registration-confirmation");
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

    protected virtual void Dispose(bool disposing)
    {
        if (_keyInterceptor is not null)
        {
            if (disposing)
            {
                _keyInterceptor.KeyDown -= OnEnterPressedHandler;
                _keyInterceptor.Dispose();
            }

            _keyInterceptor = null;
        }
    }

    ~Registration()
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