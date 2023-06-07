using Microsoft.AspNetCore.Components;
using MudBlazor;
using QuizApp.Model.Entities;
using QuizApp.Service.Interface.APIClient;

namespace QuizApp.UI.Pages;

public partial class Index
{
    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private List<QuizBase>? _quizzes;

    private bool _loading = true;


    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        var (quizzes, errorMessage) = await ApiClient.GetQuizBasesAsync();
        if (quizzes is not null)
        {
            _quizzes = quizzes;
            _quizzes.Sort((x, y) => x.Title.CompareTo(y.Title));
        }
        else
        {
            Snackbar.Add(errorMessage, Severity.Error);
        }
        _loading = false;
    }

    private void GoToQuiz(QuizBase quiz)
    {
        NavManager.NavigateTo($"/quiz/{quiz.Id}");
    }
}
