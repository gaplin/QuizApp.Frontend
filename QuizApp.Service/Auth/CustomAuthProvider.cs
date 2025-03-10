﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace QuizApp.Service.Auth;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    public CustomAuthProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwtToken = await _localStorageService.GetItemAsync<string>("jwt-access-token");
        if (string.IsNullOrEmpty(jwtToken))
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }
        var claims = new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "jwtAuth");
        var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims.FindFirst(JwtRegisteredClaimNames.Exp)!.Value));
        if (exp < DateTime.UtcNow)
        {
            await _localStorageService.RemoveItemAsync("jwt-access-token");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        else
        {
            return new AuthenticationState(new ClaimsPrincipal(claims));
        }
    }

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;
        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        return claims;
    }
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public void NotifyAuthState()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
