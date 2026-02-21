using System.Net.Http.Json;

namespace ScanEventWorker.Api;

public class ScanApiClient
{
    private readonly HttpClient _http;

    public ScanApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ScanApiResponse?> GetEvents(long fromEventId, int limit, CancellationToken ct)
    {
        var url = $"?FromEventId={fromEventId}&Limit={limit}";
        return await _http.GetFromJsonAsync<ScanApiResponse>(url, ct);
    }
}