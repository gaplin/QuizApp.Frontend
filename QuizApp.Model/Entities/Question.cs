namespace QuizApp.Model.Entities;

public class Question
{
    public string Text { get; set; } = null!;
    public List<string> Answers { get; set; } = null!;
    public int CorrectAnswer { get; set; }
}