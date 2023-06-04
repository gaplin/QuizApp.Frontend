using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.Service.API;
using QuizApp.Service.Interface.APIClient;

namespace QuizApp.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddHttpClient<IAPIClient, APIClient>(client =>
            client.BaseAddress = new Uri("https://localhost:7125/"));
        return services;
    }
}