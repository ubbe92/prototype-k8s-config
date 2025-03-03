using System.Collections.Concurrent;
using System.Diagnostics;
using Rebelbetting.Orleans.LoadGenerator.http;
using Rebelbetting.Orleans.LoadGenerator.parsing;
using Rebelbetting.Orleans.LoadGenerator.plots;

namespace Rebelbetting.Orleans.LoadGenerator.tests;

public class OrleansPerformanceTests
{
    private readonly string _ip;
    private readonly int _port;

    public OrleansPerformanceTests(string ip, int port)
    {
        _ip = ip;
        _port = port;
        // _httpRequestService = new HttpRequestService(ip, port);
    }

    public string Initialize()
    {
        var httpRequestService = new HttpRequestService(_ip, _port);
        var response = httpRequestService.InitiateCsvProcessing();
        return response.Result;
    }

    private async Task<double> MapOddsOneWorker(ContractOdds[] odds, int chunkSize)
    {
        var httpRequestService = new HttpRequestService(_ip, _port);
        var chunks = odds.Chunk(chunkSize);

        var watch = Stopwatch.StartNew();
        foreach (var chunk in chunks)
        {
            var response = await httpRequestService.ProcessOdds(chunk);
        };
        watch.Stop();
        
        return watch.ElapsedMilliseconds;
    }
    
    private async Task<double> MapOddsNWorkers(ContractOdds[] odds, int chunkSize, int numThreads = 1)
    {
        var chunks = odds.Chunk(chunkSize);

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = numThreads 
        };
        
        var httpServices = new ConcurrentDictionary<int, HttpRequestService>();
        var watch = Stopwatch.StartNew();
        await Parallel.ForEachAsync(chunks, parallelOptions, async (record, token) =>
        {
            // var httpRequestService = new HttpRequestService(_ip, _port);
            var httpRequestService = httpServices.GetOrAdd(Environment.CurrentManagedThreadId, _ => new HttpRequestService(_ip, _port));
            var response = await httpRequestService.ProcessOdds(record);
        });
        watch.Stop();
        
        return watch.ElapsedMilliseconds;
    }

    public void MakePopulationPlotMapOddsOneWorker(ContractOdds[] odds, int numIterations, int chunkSize, int numBoxes,
        double[] tickPositions, string[] tickLabels, bool showMarker, string xLabel, string yLabel, string saveLocation,
        string title)
    {
        var allTestTimes = new List<double[]>();
        var factor = 1;

        Console.WriteLine($"MakePopulationPlotMapOddsOneWorker test");
        for (var i = 0; i < numBoxes; i++)
        {
            var testTimes = new List<double>();
            for (var j = 0; j < numIterations; j++)
            {
                var elapsedTime = MapOddsOneWorker(odds, chunkSize * factor);
                testTimes.Add(elapsedTime.Result);
            }
            factor *= 2;
            allTestTimes.Add(testTimes.ToArray());
            Console.WriteLine($"{((((double) i + 1) / numBoxes) * 100):#} % complete.");
        }
        var pg = new PlotGenerator();
        pg.Width = 1200;
        pg.Height = 800;
        pg.CreatePopulationPlot(title, saveLocation, allTestTimes, tickPositions, tickLabels, showMarker, xLabel, yLabel);
    }
    
    public void MakePopulationPlotMapOddsNWorkers(ContractOdds[] odds, int numIterations, int chunkSize, int numBoxes,
        double[] tickPositions, string[] tickLabels, bool showMarker, string xLabel, string yLabel, string saveLocation,
        string title, int numThreads)
    {
        var allTestTimes = new List<double[]>();
        var factor = 1;
        
        Console.WriteLine($"MakePopulationPlotMapOddsNWorkers test");
        for (var i = 0; i < numBoxes; i++)
        {
            var testTimes = new List<double>();
            for (var j = 0; j < numIterations; j++)
            {
                var elapsedTime = MapOddsNWorkers(odds, chunkSize * factor, numThreads);
                testTimes.Add(elapsedTime.Result);
            }
            factor *= 2;
            allTestTimes.Add(testTimes.ToArray());
            Console.WriteLine($"{((((double) i + 1) / numBoxes) * 100):#} % complete.");
        }
        var pg = new PlotGenerator();
        pg.Width = 1200;
        pg.Height = 800;
        pg.CreatePopulationPlot(title, saveLocation, allTestTimes, tickPositions, tickLabels, showMarker, xLabel, yLabel);
    }
}
