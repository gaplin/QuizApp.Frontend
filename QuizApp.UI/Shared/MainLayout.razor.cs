using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Service.Auth;

namespace QuizApp.UI.Shared;

public partial class MainLayout
{
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = null!;

    [Inject]
    private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

    private async Task LogOut()
    {
        await LocalStorageService.RemoveItemAsync("jwt-access-token");
        ((CustomAuthProvider)AuthStateProvider).NotifyAuthState();
    }
}