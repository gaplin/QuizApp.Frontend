namespace QuizApp.Model.ViewModels;

public class AddQuestionViewModel
{
    public string? Text { get; set; }
    public List<string?> Answers { get; set; } = new List<string?> { null, null };
    public int CorrectAnswer { get; set; }
}