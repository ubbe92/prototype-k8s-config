// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using System.Net.NetworkInformation;
using Rebelbetting.Orleans.LoadGenerator.http;
using Rebelbetting.Orleans.LoadGenerator.parsing;
using Rebelbetting.Orleans.LoadGenerator.plots;
using Rebelbetting.Orleans.LoadGenerator.tests;
using ScottPlot;
using ScottPlot.Plottables;

// Http requests
// https://stackoverflow.com/questions/4015324/send-http-post-request-in-net

// Plots
// https://scottplot.net/quickstart/console/

var port = 8080;
var ip = "http://192.168.0.23";

// ------------------------------------------------------------------------------
// Parse odds csv file before running tests
// ------------------------------------------------------------------------------
var pathToOddsFile = "./csv_files/Odds.csv";
var delimiter = ";";

if (!File.Exists(pathToOddsFile))
{
    Console.WriteLine($"Could not find csv file!");
    Environment.Exit(1);
}

var csvFileReader = new CsvFileReader<OddsDto>(pathToOddsFile, delimiter);
var oddsDtos = csvFileReader.Records.ToArray();
var odds = csvFileReader.ConvertOddsDtoToContractOdds(oddsDtos);
csvFileReader.CleanUp();

// ------------------------------------------------------------------------------
// Performance tests init
// ------------------------------------------------------------------------------
var orleansPerformanceTests = new OrleansPerformanceTests(ip, port);
var numThreads = 6;
var numIterations = 10;
var factor = 1;
var chunkSize = 50;
var numBoxes = 6;

double[] tp = Enumerable.Range(0, numBoxes).Select(x => (double) x).ToArray();
string[] tl = new string[numBoxes];
for (int i = 0; i < numBoxes; i++)
{
    tl[i] = $"{chunkSize * factor}";
    factor *= 2;
}
bool showMarker = true;
var xLabel = "Batch sizes";
var yLabel = "Latency (ms)";
var saveLocation = "";
var title = "";

// response = orleansPerformanceTests.Initialize();
// Console.WriteLine($"Response init: {response}");
// ------------------------------------------------------------------------------
// Performance tests
// ------------------------------------------------------------------------------
saveLocation = "./figures/MakePopulationPlotMapOddsOneWorker.png";
title = $"Mapping latency of {odds.Length} odds sent in batches of various sizes from {1} worker(s). Each test was repeated {numIterations} times";
orleansPerformanceTests.MakePopulationPlotMapOddsOneWorker(odds, numIterations, chunkSize, numBoxes, tp, tl, 
    showMarker, xLabel, yLabel, saveLocation, title);

saveLocation = "./figures/MakePopulationPlotMapOddsNWorkers.png";
title = $"Mapping latency of {odds.Length} odds sent in parallel batches of various sizes from {numThreads} worker(s). Each test was repeated {numIterations} times";
orleansPerformanceTests.MakePopulationPlotMapOddsNWorkers(odds, numIterations, chunkSize, numBoxes, tp, tl, 
    showMarker, xLabel, yLabel, saveLocation, title, numThreads);

// for (int i = 0; i < numIterations; i++)
// {
//     var result = orleansPerformanceTests.MapOddsOneWorker(odds, 50);
//     Console.WriteLine($"MapOddsOneWorker:\t{result.Result}\tms");
// }

// for (int i = 0; i < numIterations; i++)
// {
//     var result = orleansPerformanceTests.MapOddsNWorkers(odds, 50, numThreads);
//     Console.WriteLine($"MapOddsNWorkers:\t{result.Result}\tms using: {numThreads} threads");
// }


// List<double[]> ld = new List<double[]>();
// ld.Add(dataY);
// ld.Add(dataY1);
// pg.CreatePopulationPlot("En titel", "./figures/pop.png", ld, 
//     [0, 1], ["8", "16"], true, "xLabel", "yLabel");