using QuizApp.Model.Entities;
using QuizApp.Model.Responses;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Interface.APIClient;
using System.Net.Http.Json;

namespace QuizApp.Service.API;

internal class APIClient : IAPIClient
{
    private readonly HttpClient _httpClient;

    public APIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(string? token, Dictionary<string, string[]>? errors)> RegisterAndGetTokenAsync(RegistrationViewModel registrationModel)
    {
        var response = await _httpClient.PostAsJsonAsync("/users", registrationModel);
        if(response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return (token, null);
        }
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();
            return (null, problemDetails!.Errors);
        }
        return (null, null);
    }

    public async Task<(string? token, Dictionary<string, string[]>? errors)> LogInAndGetTokenAsync(LoginViewModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return (token, null);
        }
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();
            return (null, problemDetails!.Errors);
        }
        return (null, null);
    }

    public async Task<List<QuizBase>> GetQuizBasesAsync()
    {
        var quizzes = await _httpClient.GetFromJsonAsync<List<QuizBase>>("/quizzes/baseinfo");
        return quizzes ?? new();
    }

    public async Task<Quiz?> GetQuizAsync(string id, bool shuffle = true)
    {
        var quiz = 
            await _httpClient.GetFromJsonAsync<Quiz>($"/quizzes/{id}?{nameof(shuffle)}={shuffle}");
        return quiz;
    }
}