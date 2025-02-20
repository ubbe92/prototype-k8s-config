// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using Rebelbetting.Orleans.LoadGenerator.http;

// Http requests
// https://stackoverflow.com/questions/4015324/send-http-post-request-in-net

// Plots
// https://scottplot.net/quickstart/console/

double[] dataX = { 1, 2, 3, 4, 5 };
double[] dataY = { 1, 4, 9, 16, 25 };

ScottPlot.Plot myPlot = new();
myPlot.Add.Scatter(dataX, dataY);

// This has to be run from the terminal. If i run it through Rider it wants to save the image to:
//
// '/Users/antondacklin/Documents/gitlab/prototype-k8s-config/load_generator/Rebelbetting.Orleans.LoadGenerator/
// Rebelbetting.Orleans.Kubernetes.LoadGenerator/bin/Debug/net9.0/plots/quickstart.png'
myPlot.SavePng("./plots/quickstart.png", 400, 300);

Console.WriteLine("Hello, World!");



// ---------------------------------------
// Test HTTP requests
// ---------------------------------------
// var client = new HttpClient();
// var port = 8162;
// var url = $"http://127.0.0.1:{port}/createcsvgrains";

// var values = new Dictionary<string, string> {};
//
// var content = new FormUrlEncodedContent(values);
//
// var response = await client.PostAsync(url, content);
//
// var responseString = await response.Content.ReadAsStringAsync();
//
// Console.WriteLine($"RESPONSE FROM CREATE CSV GRAINS: {responseString}");


var port = 8162;
var url = "http://127.0.0.1";
var httpRequestService = new HttpRequestService(url, port);
// var initResp = httpRequestService.InitiateCsvProcessing();
// Console.WriteLine($"response: {initResp.Result}");

int grainId = 732488;
var resp = httpRequestService.GetEventGrain(grainId);
Console.WriteLine($"response: {resp.Result}");

string grainNameId = "event:Handball:I Liga (Poland)";
resp = httpRequestService.GetEventNameGrain(grainNameId);
Console.WriteLine($"response: {resp.Result}");

grainId = 4866250;
resp = httpRequestService.GetParticipantGrain(grainId);
Console.WriteLine($"response: {resp.Result}");

grainNameId = "participant:Handball:Croatia";
resp = httpRequestService.GetParticipantNameGrain(grainNameId);
Console.WriteLine($"response: {resp.Result}");