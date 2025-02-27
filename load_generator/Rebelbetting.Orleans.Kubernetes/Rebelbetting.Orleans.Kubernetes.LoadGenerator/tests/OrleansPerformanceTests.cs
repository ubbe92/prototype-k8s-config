using Rebelbetting.Orleans.LoadGenerator.http;

namespace Rebelbetting.Orleans.LoadGenerator.tests;

public class OrleansPerformanceTests
{
    private string _url;
    private int _port;
    private readonly HttpRequestService _httpRequestService;

    public OrleansPerformanceTests(string url, int port)
    {
        _url = url;
        _port = port;
        _httpRequestService = new HttpRequestService(url, port);
    }
    
    
}