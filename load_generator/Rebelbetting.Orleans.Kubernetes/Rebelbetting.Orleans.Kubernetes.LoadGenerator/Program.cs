// See https://aka.ms/new-console-template for more information
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

if (commandLineArgs.Contains("-explanation"))
{
    Console.WriteLine($"Usage: dotnet run <PopulateOrleans> <Port> <IP> [-explanation]");
    Console.WriteLine($"\t<PopulateOrleans> : Should be 'true' if orleans should be populated with events and participant grains otherwise 'false'.");
    Console.WriteLine($"\t<Port>            : The port used by remote Orleans client, usually 8080.");
    Console.WriteLine($"\t<IP>              : The IP of a remote Orleans clients in an Orleans cluster.");
    Console.WriteLine($"\t<PathToOddsCsv>   : Odds.csv file path.");
    Console.WriteLine($"\t<Delimiter>       : Which delimiter to use when parsing Odds.csv, either ';' or ','.");
    Environment.Exit(0);
}

if (commandLineArgs.Length == 6)
{
    populateOrleans = Convert.ToBoolean(commandLineArgs[1]);
    port = int.Parse(commandLineArgs[2]);
    ip = commandLineArgs[3];
    pathToOddsFile = commandLineArgs[4];
    delimiter = commandLineArgs[5];

    Console.WriteLine($"populate?   >> '{populateOrleans}'");
    Console.WriteLine($"port        >> '{port}'");
    Console.WriteLine($"IP          >> '{ip}'");
    Console.WriteLine($"path        >> '{pathToOddsFile}'");
    Console.WriteLine($"delimiter   >> '{delimiter}'");
}
else
{
    Console.WriteLine($"Usage: dotnet run <PopulateOrleans> <Port> <IP> <PathToOddsCsv> <Delimiter> [-explanation]");
    Console.WriteLine($"Example:\n\tdotnet run false 8080 http://192.168.0.23 ./csv_files/Odds.csv ,");
    Environment.Exit(1);
}


// ------------------------------------------------------------------------------
// Parse odds csv file before running tests
// ------------------------------------------------------------------------------
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
    