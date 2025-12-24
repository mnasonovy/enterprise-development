using LibraryClient;
using LibraryClient.OpenApis;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = builder.HostEnvironment.BaseAddress;
Console.WriteLine($"📍 Client base: {baseAddress}");

// HttpClient для API
var httpClient = new HttpClient();

// Определяем адрес API
string apiBaseAddress;

if (builder.HostEnvironment.IsDevelopment())
{
    // В Aspire/локальной разработке
    // Заменяем порт клиента на порт API (обычно 7000)
    var uri = new Uri(baseAddress);
    if (uri.Host == "localhost" || uri.Host == "127.0.0.1")
    {
        apiBaseAddress = $"{uri.Scheme}://localhost:7000";
    }
    else
    {
        apiBaseAddress = baseAddress;
    }
}
else
{
    // В продакшене - используем тот же хост
    apiBaseAddress = baseAddress;
}

Console.WriteLine($"🔗 API base: {apiBaseAddress}");
httpClient.BaseAddress = new Uri(apiBaseAddress);

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped<LibraryApiWrapper>();

await builder.Build().RunAsync();
