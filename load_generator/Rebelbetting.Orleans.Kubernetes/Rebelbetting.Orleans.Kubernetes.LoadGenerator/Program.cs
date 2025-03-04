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

var commandLineArgs =  Environment.GetCommandLineArgs();
bool populateOrleans = false;

if (commandLineArgs.Length == 2)
{
    Console.WriteLine($"args len: {commandLineArgs.Length}, arg[1]: {commandLineArgs[1]}");
    populateOrleans = Convert.ToBoolean(commandLineArgs[1]);
}

var port = 8080;
var ip = "http://192.168.0.23";

// ------------------------------------------------------------------------------
// Parse odds csv file before running tests
// ------------------------------------------------------------------------------
var pathToOddsFile = "./csv_files/Odds.csv";
// var pathToOddsFile = "/Users/antondacklin/Documents/master-thesis-data/RBData/odds/OddsCache.csv";
var delimiter = ",";

if (!File.Exists(pathToOddsFile))
{
    Console.WriteLine($"Could not find csv file!");
    Environment.Exit(1);
}

var csvFileReader = new CsvFileReader<OddsDto>(pathToOddsFile, delimiter);
var oddsDtos = csvFileReader.Records.ToArray();
var odds = csvFileReader.ConvertOddsDtoToContractOdds(oddsDtos);
csvFileReader.CleanUp();
Console.WriteLine($"length of csv: {odds.Length} at location: '{pathToOddsFile}'");

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

if (populateOrleans)
{
    var response = orleansPerformanceTests.Initialize();
    Console.WriteLine($"Response init: {response}");
}

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
    