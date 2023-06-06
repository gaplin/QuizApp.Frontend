namespace QuizApp.Model.ViewModels;

public class AddQuizViewModel
{
    public string? Title { get; set; }
    public string? Category { get; set; }
    public List<AddQuestionViewModel> Questions { get; set; } = new List<AddQuestionViewModel> { new() };
}