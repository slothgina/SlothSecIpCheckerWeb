using System.Text.Json;

public class AbuseIpService : IAbuseIpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AbuseIpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AbuseIpResult?> CheckIpAsync(string ip)
    {
        var client = _httpClientFactory.CreateClient("ipchecker");

        var response = await client.GetAsync($"check?ipAddress={ip}&maxAgeInDays=90");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<AbuseIpResult>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
