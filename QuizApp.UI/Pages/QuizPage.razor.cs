using Microsoft.AspNetCore.Components;
using MudBlazor;
using QuizApp.Model.Entities;
using QuizApp.Service.Interface.APIClient;
using System.Timers;
using Timer = System.Timers.Timer;

namespace QuizApp.UI.Pages;

public partial class QuizPage : IDisposable
{
    [Parameter]
    public string Id { get; set; } = null!;

    [Inject]
    private IAPIClient ApiClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    private Quiz? _quiz;

    private int _questionIndex, _score;

    private readonly Timer _timer = new(100);

    bool disposedValue;

    private double _progressBarStatus;
    private DateTime _lastTimerEvent;

    protected override async Task OnInitializedAsync()
    {
        await InitQuiz();
        _timer.Elapsed += OnElapsedEvent;
        _lastTimerEvent = DateTime.Now;
        _timer.Start();
    }

    private async Task InitQuiz()
    {
        var (quiz, errorMessage) = await ApiClient.GetQuizAsync(Id);
        if (quiz is not null) _quiz = quiz;
        else
        {
            Snackbar.Add(errorMessage, Severity.Error);
        }
        _score = _questionIndex = 0;
        _progressBarStatus = 0;
    }

    private void AnswerClicked(int index)
    {
        if(index == _quiz!.Questions[_questionIndex].CorrectAnswer)
        {
            ++_score;
        }
        _progressBarStatus = 0;
        ++_questionIndex;
    }

    private void GoToHomePage()
    {
        NavManager.NavigateTo("/");
    }

    private async Task TryAgain()
    {
        await InitQuiz();
    }

    private void OnElapsedEvent(object? source, ElapsedEventArgs e)
    {
        var elapsedTime = (e.SignalTime - _lastTimerEvent).TotalSeconds;
        _progressBarStatus += elapsedTime / 8 * 100;
        _lastTimerEvent = e.SignalTime;
        if(_progressBarStatus >= 100)
        {
            ++_questionIndex;
            _progressBarStatus = 0;
        }
        InvokeAsync(StateHasChanged);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _timer.Elapsed -= OnElapsedEvent;
                _timer.Dispose();
            }

            disposedValue = true;
        }
    }

   

    ~QuizPage()
    {
         Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
