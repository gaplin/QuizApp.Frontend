﻿using QuizApp.Model.Entities;
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
            var token = await response.Content.ReadFromJsonAsync<string>();
            return (token, null);
        }
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();
            return (null, problemDetails!.Errors);
        }
        return (null, null);
    }

    public async Task<(List<QuizBase>?, string?)> GetQuizBasesAsync()
    {
        var response = await _httpClient.GetAsync("/quizzes/baseinfo");
        if(response.IsSuccessStatusCode)
        {
            var quizzes = await response.Content.ReadFromJsonAsync<List<QuizBase>>();
            return (quizzes, null);
        }
        var errorMessage = response.ReasonPhrase;
        return (null, errorMessage);
    }

    public async Task<(Quiz?, string?)> GetQuizAsync(string id, bool shuffle = true)
    {
        var response = await _httpClient.GetAsync($"/quizzes/{id}?{nameof(shuffle)}={shuffle}");
        if (response.IsSuccessStatusCode)
        {
            var quiz = await response.Content.ReadFromJsonAsync<Quiz>();
            return (quiz, null);
        }
        var errorMessage = response.ReasonPhrase;
        return (null, errorMessage);
    }

    public async Task<(Dictionary<string, string[]>? validationErrors, string? reasonPhrase)> AddQuizAsync(AddQuizViewModel addQuizModel)
    {
        var response = await _httpClient.PostAsJsonAsync("/quizzes", addQuizModel);
        if(response.IsSuccessStatusCode)
        {
            return (null, null);
        }
        var reasonPhrase = response.ReasonPhrase;
        if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsWithErrors>();
            return (problemDetails!.Errors, reasonPhrase);
        }
        return (null, reasonPhrase);
    }
}