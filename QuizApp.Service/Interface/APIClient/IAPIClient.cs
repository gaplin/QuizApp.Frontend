using QuizApp.Model.Entities;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Interface.APIClient;

public interface IAPIClient
{
    Task<Quiz?> GetQuizAsync(string id, bool shuffle = true);
    Task<List<QuizBase>> GetQuizBasesAsync();
    Task<(string? token, Dictionary<string, string[]>? errors)> LogInAndGetTokenAsync(LoginViewModel loginModel);
    Task<(string? token, Dictionary<string, string[]>? errors)> RegisterAndGetTokenAsync(RegistrationViewModel registrationModel);
}