using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Service.API;
using QuizApp.Service.Auth;
using QuizApp.Service.Interface.APIClient;

namespace QuizApp.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddHttpClient<IAPIClient, APIClient>(client =>
            client.BaseAddress = new Uri("https://localhost:7125/"))
            .AddHttpMessageHandler<CustomHttpHandler>();
        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();
        services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
        services.AddScoped<CustomHttpHandler>();
        return services;
    }
}