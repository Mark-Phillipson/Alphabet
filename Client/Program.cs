using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Modal;
using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AutoMapper;
using Client.Repositories;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
var config = builder.Configuration;

// Correct the JSON file path to use a relative URL for HTTP access
builder.Services.Configure<JsonRepositoryOptions>(options =>
{
    options.JsonFilePath = "Prompts.json"; // Corrected to relative URL for HTTP access
});

builder.Services.AddScoped<IPromptDataService, PromptDataService>();

// Add AutoMapper to the service collection
builder.Services.AddAutoMapper(typeof(Program));

// Fix the PromptRepository registration
builder.Services.AddScoped<IPromptRepository>(provider =>
{
    var options = provider.GetRequiredService<IOptions<JsonRepositoryOptions>>();
    var mapper = provider.GetRequiredService<IMapper>();
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new PromptRepository(options, mapper, httpClient);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
