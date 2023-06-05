using Microsoft.AspNetCore.Components;
using QuizApp.Model.Entities;
using QuizApp.Service.Interface.APIClient;

namespace QuizApp.UI.Pages;

public partial class QuizPage
{
    [Parameter]
    public string Id { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    private Quiz? _quiz;

    private bool _notFound;

    protected override async Task OnInitializedAsync()
    {
        _quiz = await ApiClient.GetQuizAsync(Id);
        if(_quiz is null)
        {
            _notFound = true;
        }
    }
}
