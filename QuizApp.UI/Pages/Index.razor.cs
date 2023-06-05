using Microsoft.AspNetCore.Components;
using QuizApp.Model.Entities;
using QuizApp.Service.Interface.APIClient;

namespace QuizApp.UI.Pages;

public partial class Index
{
    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    private List<QuizBase>? _quizzes;

    private bool _loading = true;


    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        _quizzes = await ApiClient.GetQuizBasesAsync();
        _loading = false;
    }

    private void GoToQuiz(QuizBase quiz)
    {
        NavManager.NavigateTo($"/quiz/{quiz.Id}");
    }
}
