using TapTrack.Shared.Services;
using TapTrack.Web.Components;
using TapTrack.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Optionally configure spreadsheet id and range via configuration
var spreadsheetId = builder.Configuration["GoogleSheets:SpreadsheetId"] ?? "";
var spreadsheetRange = builder.Configuration["GoogleSheets:Range"] ?? "Sheet1!A:B";

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add device-specific services used by the TapTrack.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

// Register the Google Sheets button provider if a spreadsheet id is configured
if (!string.IsNullOrEmpty(spreadsheetId))
{
    builder.Services.AddSingleton<TapTrack.Shared.Services.IButtonProvider, TapTrack.Web.Services.GoogleSheetsButtonProvider>(sp =>
        new TapTrack.Web.Services.GoogleSheetsButtonProvider(spreadsheetId, spreadsheetRange));
}
else
{
    // Fallback: empty provider to avoid nulls (could add another implementation)
    builder.Services.AddSingleton<TapTrack.Shared.Services.IButtonProvider, TapTrack.Web.Services.EmptyButtonProvider>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(TapTrack.Shared._Imports).Assembly);

app.Run();
