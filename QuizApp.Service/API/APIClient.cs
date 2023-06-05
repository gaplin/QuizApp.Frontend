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
}