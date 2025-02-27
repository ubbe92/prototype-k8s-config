// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using Rebelbetting.Orleans.LoadGenerator.http;
using Rebelbetting.Orleans.LoadGenerator.plots;
using ScottPlot;
using ScottPlot.Plottables;

// Http requests
// https://stackoverflow.com/questions/4015324/send-http-post-request-in-net

// Plots
// https://scottplot.net/quickstart/console/

double[] dataX = { 1, 2, 3, 4, 5 };
double[] dataY = { 1, 4, 9, 16, 25 };

double[] dataX1 = { 3, 6, 3, 2, 8 };
double[] dataY1 = { 9, 15, 3, 12, 5 };

ScottPlot.Plot myPlot = new();
myPlot.Title("HELLO");
var p1 = myPlot.Add.Scatter(dataX, dataY);
p1.LegendText = "p1";

var p2 = myPlot.Add.Scatter(dataX1, dataY1);
p2.LegendText = "p2";

myPlot.Legend.IsVisible = true;
myPlot.Legend.Alignment = Alignment.UpperCenter;

myPlot.Legend.ShadowColor = Colors.Blue.WithOpacity(.2);
myPlot.Legend.ShadowOffset = new(10, 10);

myPlot.Legend.FontName = Fonts.Serif;
myPlot.ShowLegend();

// This has to be run from the terminal. If i run it through Rider it wants to save the image to:
//
// '/Users/antondacklin/Documents/gitlab/prototype-k8s-config/load_generator/Rebelbetting.Orleans.LoadGenerator/
// Rebelbetting.Orleans.Kubernetes.LoadGenerator/bin/Debug/net9.0/plots/quickstart.png'
myPlot.SavePng("./figures/quickstart.png", 400, 300);
Console.WriteLine("Hello, World!");

var pg = new PlotGenerator();
var boxes = new List<Box>();
Random rnd = new Random();
double[] tickPositions = { 0, 1, 2, 3 }; // Positions for your boxplots
string[] tickLabels = { "128", "256", "512", "1024" }; // Your custom labels
for (int i = 0; i < 4; i++)
{
    var b = pg.CreateBox(i, rnd.Next(10, 13), rnd.Next(15, 20), rnd.Next(3, 8), rnd.Next(25, 33), rnd.Next(13, 15));
    boxes.Add(b);
}

pg.Width = 800;
pg.Height = 600;
pg.CreateBoxPlot("EN TITEL", "./figures/box.png", boxes, tickPositions, tickLabels);

List<double[]> ld = new List<double[]>();
ld.Add(dataY);
ld.Add(dataY1);
pg.CreatePopulationPlot("En titel", "./figures/pop.png", ld, 
    [0, 1], ["8", "16"], true, "xLabel", "yLabel");

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


// var port = 8081;
// var url = "http://127.0.0.1";

// var port = 8080;
// var url = "http://192.168.0.23";
// var httpRequestService = new HttpRequestService(url, port);
// // var initResp = httpRequestService.InitiateCsvProcessing();
// // Console.WriteLine($"response: {initResp.Result}");
//
// int grainId = 732488;
// var resp = httpRequestService.GetEventGrain(grainId);
// Console.WriteLine($"response: {resp.Result}");
//
// string grainNameId = "event:Handball:I Liga (Poland)";
// resp = httpRequestService.GetEventNameGrain(grainNameId);
// Console.WriteLine($"response: {resp.Result}");
//
// grainId = 4866250;
// resp = httpRequestService.GetParticipantGrain(grainId);
// Console.WriteLine($"response: {resp.Result}");
//
// grainNameId = "participant:Handball:Croatia";
// resp = httpRequestService.GetParticipantNameGrain(grainNameId);
// Console.WriteLine($"response: {resp.Result}");