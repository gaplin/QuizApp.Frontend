using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Interface.APIClient;

public interface IAPIClient
{
    Task<(string? token, Dictionary<string, string[]>? errors)> RegisterAndGetTokenAsync(RegistrationViewModel registrationModel);
}