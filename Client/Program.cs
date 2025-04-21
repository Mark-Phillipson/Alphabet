using Blazored.LocalStorage;
using Blazored.Toast;
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
var config = builder.Configuration;

// Correct the JSON file path to use the wwwroot folder
builder.Services.Configure<JsonRepositoryOptions>(options =>
{
    options.JsonFilePath = Path.Combine("wwwroot", "Prompts.json");
});

builder.Services.AddScoped<IPromptDataService, PromptDataService>();

// Add AutoMapper to the service collection
builder.Services.AddAutoMapper(typeof(Program));

// Fix the PromptRepository registration
builder.Services.AddScoped<IPromptRepository>(provider =>
{
    var options = provider.GetRequiredService<IOptions<JsonRepositoryOptions>>();
    var mapper = provider.GetRequiredService<IMapper>();
    return new PromptRepository(options, mapper);
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
