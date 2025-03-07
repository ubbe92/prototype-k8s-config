// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic.CompilerServices;
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
var populateOrleans = false;
var port = 8080;
var ip = "http://192.168.0.23";
var pathToOddsFile = "./csv_files/Odds.csv";
// var pathToOddsFile = "/Users/antondacklin/Documents/master-thesis-data/RBData/odds/OddsCache.csv";
var delimiter = ",";
var numThreads = 1;

if (commandLineArgs.Contains("-explanation"))
{
    Console.WriteLine($"Usage: dotnet run <PopulateOrleans> <Port> <IP> [-explanation]");
    Console.WriteLine($"\t<PopulateOrleans> : Should be 'true' if orleans should be populated with events and participant grains otherwise 'false'.");
    Console.WriteLine($"\t<Port>            : The port used by remote Orleans client, usually 8080.");
    Console.WriteLine($"\t<IP>              : The IP of a remote Orleans clients in an Orleans cluster.");
    Console.WriteLine($"\t<PathToOddsCsv>   : Odds.csv file path.");
    Console.WriteLine($"\t<Delimiter>       : Which delimiter to use when parsing Odds.csv, either ';' or ','.");
    Console.WriteLine($"\t<NumThreads>      : The number of threads to use (Simulates the number of workers).");
    Environment.Exit(0);
}

if (commandLineArgs.Length == 7)
{
    populateOrleans = Convert.ToBoolean(commandLineArgs[1]);
    port = int.Parse(commandLineArgs[2]);
    ip = commandLineArgs[3];
    pathToOddsFile = commandLineArgs[4];
    delimiter = commandLineArgs[5];
    numThreads = Convert.ToInt32(commandLineArgs[6]);

    Console.WriteLine($"populate?   >> '{populateOrleans}'");
    Console.WriteLine($"port        >> '{port}'");
    Console.WriteLine($"IP          >> '{ip}'");
    Console.WriteLine($"path        >> '{pathToOddsFile}'");
    Console.WriteLine($"delimiter   >> '{delimiter}'");
}
else
{
    Console.WriteLine($"Usage: dotnet run <PopulateOrleans> <Port> <IP> <PathToOddsCsv> <Delimiter> <NumThreads> [-explanation]");
    Console.WriteLine($"Example:\n\tdotnet run false 8080 http://192.168.0.23 ./csv_files/Odds.csv , 6");
    Environment.Exit(1);
}

if (!File.Exists(pathToOddsFile))
{
    Console.WriteLine($"Could not find csv file!");
    Environment.Exit(1);
}

// ------------------------------------------------------------------------------
// Parse odds csv file before running tests
// ------------------------------------------------------------------------------
var csvFileReader = new CsvFileReader<OddsDto>(pathToOddsFile, delimiter);
var oddsDtos = csvFileReader.Records.ToArray();
var odds = csvFileReader.ConvertOddsDtoToContractOdds(oddsDtos);
csvFileReader.CleanUp();
Console.WriteLine($"length of csv: {odds.Length} at location: '{pathToOddsFile}'");

// ------------------------------------------------------------------------------
// Performance tests init
// ------------------------------------------------------------------------------
var orleansPerformanceTests = new OrleansPerformanceTests(ip, port);
// var numIterations = 10;
// var chunkSize = 50;
// var numBoxes = 6;

var numIterations = 1;
var chunkSize = 100;
var numBoxes = 1;

double[] tp = Enumerable.Range(0, numBoxes).Select(x => (double) x).ToArray();
string[] tl = new string[numBoxes];
var factor = 1;
for (int i = 0; i < numBoxes; i++)
{
    tl[i] = $"{chunkSize * factor}";
    factor *= 2;
}
var showMarker = true;
var xLabel = "Batch sizes";
var yLabel = "Latency (ms)";
var saveLocation = "";
var title = "";

if (populateOrleans)
{
    var response = orleansPerformanceTests.Initialize();
    Console.WriteLine($"Response init: {response}");
}

// TEST TIMER
// var chunks = odds.Chunk(2).ToArray();
// await orleansPerformanceTests.MapOddsOneWorker(chunks[0], 2);
// Console.WriteLine($"Done!");
// return;

var watch = Stopwatch.StartNew();

// ------------------------------------------------------------------------------
// Performance tests
// ------------------------------------------------------------------------------
// saveLocation = "./figures/MakePopulationPlotMapOddsOneWorker.png";
// title = $"Mapping latency of {odds.Length} odds sent in batches of various sizes from {1} worker(s). Each test was repeated {numIterations} times";
// orleansPerformanceTests.MakePopulationPlotMapOddsOneWorker(odds, numIterations, chunkSize, numBoxes, tp, tl, 
//     showMarker, xLabel, yLabel, saveLocation, title);

saveLocation = "./figures/MakePopulationPlotMapOddsNWorkers.png";
title = $"Mapping latency of {odds.Length} odds sent in parallel batches of various sizes from {numThreads} worker(s). Each test was repeated {numIterations} times";
orleansPerformanceTests.MakePopulationPlotMapOddsNWorkers(odds, numIterations, chunkSize, numBoxes, tp, tl, 
    showMarker, xLabel, yLabel, saveLocation, title, numThreads);
    
    
watch.Stop();

Console.WriteLine($"All tests took: {watch.ElapsedMilliseconds} ms");