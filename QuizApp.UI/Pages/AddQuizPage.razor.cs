﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using QuizApp.Model.ViewModels;
using QuizApp.Service.Interface.APIClient;
using QuizApp.UI.JavaScriptInterop;
using System.Runtime.Versioning;

namespace QuizApp.UI.Pages;

[SupportedOSPlatform("browser")]
public partial class AddQuizPage
{
    private readonly AddQuizViewModel _model = new();

    private MudForm _form = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private string? _apiErrorMessage;
    private void AddQuestion()
    {
        _model.Questions.Add(new());
        Interop.ScrollToElement("mainCardActions", 50);
    }
    private void RemoveQuestion(int idx)
    {
        _model.Questions.RemoveAt(idx);
    }

    private static void RemoveAnswer(AddQuestionViewModel question)
    {
        question.Answers.RemoveAt(question.Answers.Count - 1);
    }

    private static void AddAnswer(AddQuestionViewModel question)
    {
        question.Answers.Add(null);
    }
    private async Task AddQuizAsync()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            var (validationErrors, reasonPhrase) = await ApiClient.AddQuizAsync(_model);
            if (reasonPhrase is null)
            {
                Snackbar.Add("Quiz added successfully", Severity.Success);
                NavManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(reasonPhrase, Severity.Error);
                if (validationErrors is not null)
                {
                    _apiErrorMessage = "";
                    foreach (var errorMessages in validationErrors.Values)
                    {
                        foreach (var errorMessage in errorMessages)
                        {
                            _apiErrorMessage += $"{errorMessage} | ";
                        }
                    }
                }
            }
        }
    }
}