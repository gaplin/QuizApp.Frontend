using Microsoft.AspNetCore.Components;

namespace QuizApp.UI.Components;

public partial class QuizSummary
{
    [Parameter]
    public required string QuizId { get; set; }

    [Parameter]
    public required int Score { get; set; }

    [Parameter]
    public required bool Win { get; set; }

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Parameter]
    public required EventCallback TryAgainCallback { get; set; }


    private void GoToHomePage()
    {
        NavManager.NavigateTo("");
    }
}