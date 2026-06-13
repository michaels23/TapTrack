using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using TapTrack.Shared.Models;
using TapTrack.Shared.Services;

namespace TapTrack.GoogleSheets;

public class GoogleSheetsButtonProvider : IButtonProvider
{
    private readonly string _spreadsheetId;
    private readonly string _range;

    public GoogleSheetsButtonProvider(string spreadsheetId, string range = "Sheet1!A:B")
    {
        _spreadsheetId = spreadsheetId;
        _range = range;
    }

    public async Task<ButtonDefinition[]> GetButtonsAsync(CancellationToken cancellationToken = default)
    {
        // Using Application Default Credentials or service account JSON via environment variable is recommended.
        GoogleCredential credential = await GoogleCredential.GetApplicationDefaultAsync(cancellationToken);

        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "TapTrack-GoogleSheets-Provider"
        });

        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(_spreadsheetId, _range);
        ValueRange response = await request.ExecuteAsync(cancellationToken);
        var values = response.Values;

        if (values == null || values.Count == 0)
            return Array.Empty<ButtonDefinition>();

        var list = new List<ButtonDefinition>(values.Count);
        foreach (var row in values)
        {
            string label = row.Count > 0 ? row[0]?.ToString() ?? string.Empty : string.Empty;
            string color = row.Count > 1 ? row[1]?.ToString() ?? "primary" : "primary";
            if (!string.IsNullOrWhiteSpace(label))
            {
                list.Add(new ButtonDefinition(label, color));
            }
        }

        return list.ToArray();
    }
}
