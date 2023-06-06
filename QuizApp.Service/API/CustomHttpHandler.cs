using Blazored.LocalStorage;

namespace QuizApp.Service.API;

public class CustomHttpHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    public CustomHttpHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwtToken = await _localStorageService.GetItemAsync<string>("jwt-access-token", cancellationToken);
        if (!string.IsNullOrEmpty(jwtToken))
        {
            request.Headers.Add("Authorization", $"Bearer {jwtToken}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}