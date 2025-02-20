using System.Net.Http.Json;

namespace Rebelbetting.Orleans.LoadGenerator.http;

public class HttpRequestService
{
    private readonly HttpClient _httpClient;

    public HttpRequestService(string baseUrl, int port)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{baseUrl}:{port}")
        };
    }

    public async Task<string> InitiateCsvProcessing()
    {
        var url = _httpClient.BaseAddress + "createcsvgrains";
        var values = new Dictionary<string, string> {};
        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetEventGrain(int grainId)
    {
        var url = _httpClient.BaseAddress + "eventgrain/" + grainId;
        return await _httpClient.GetStringAsync(url);
    }
    
    public async Task<string> GetEventNameGrain(string grainNameId)
    {
        var url = _httpClient.BaseAddress + "eventnamegrain/" + grainNameId;
        return await _httpClient.GetStringAsync(url);
    }
    
    public async Task<string> GetParticipantGrain(int grainId)
    {
        var url = _httpClient.BaseAddress + "participantgrain/" + grainId;
        return await _httpClient.GetStringAsync(url);
    }
    
    public async Task<string> GetParticipantNameGrain(string grainNameId)
    {
        var url = _httpClient.BaseAddress + "participantnamegrain/" + grainNameId;
        return await _httpClient.GetStringAsync(url);
    }
}