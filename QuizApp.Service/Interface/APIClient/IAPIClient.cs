using QuizApp.Model.Entities;
using QuizApp.Model.ViewModels;

namespace QuizApp.Service.Interface.APIClient;

public interface IAPIClient
{
    Task<(Dictionary<string, string[]>? validationErrors, string? reasonPhrase)> AddQuizAsync(AddQuizViewModel addQuizModel);
    Task<(Quiz?, string?)> GetQuizAsync(string id, bool shuffle = true);
    Task<(List<QuizBase>?, string?)> GetQuizBasesAsync();
    Task<(string? token, Dictionary<string, string[]>? errors)> LogInAndGetTokenAsync(LoginViewModel loginModel);
    Task<(string? token, Dictionary<string, string[]>? errors)> RegisterAndGetTokenAsync(RegistrationViewModel registrationModel);
}