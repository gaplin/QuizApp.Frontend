using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuizApp.Service;
using QuizApp.UI;
using System.Runtime.InteropServices.JavaScript;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMudServices()
    .AddServices();

#pragma warning disable CA1416 // Validate platform compatibility
_ = await JSHost.ImportAsync("Interop", "../js/interop.js");
#pragma warning restore CA1416 // Validate platform compatibility

await builder.Build().RunAsync();
