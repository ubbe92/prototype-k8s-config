using System.Runtime.Intrinsics.X86;
using ScottPlot;
using ScottPlot.Plottables;

namespace Rebelbetting.Orleans.LoadGenerator.plots;

public class PlotGenerator
{
    public int Width { get; set; } = 400;
    public int Height { get; set; } = 300;

    public Box CreateBox(double pos, double boxMin, double boxMax, double whiMin, double whiMax, double boxMiddle)
    {
        Box box = new()
        {
            Position = pos,
            BoxMin = boxMin,
            BoxMax = boxMax,
            WhiskerMin = whiMin,
            WhiskerMax = whiMax,
            BoxMiddle = boxMiddle,
        };
        
        return box;
    }

    public void CreatePopulationPlot(string title, string saveFileLocation, List<double[]> listOfValues, 
        double[] tickPositions, string[] tickLabels, bool showMarker, string xLabel, string yLabel)
    {
        Plot plot = new();
        plot.Title(title);
        plot.Legend.IsVisible = true;
        plot.Legend.Alignment = Alignment.UpperCenter;
        plot.Legend.ShadowColor = Colors.Blue.WithOpacity(.2);
        plot.Legend.ShadowOffset = new(10, 10);
        plot.Legend.FontName = Fonts.Serif;
        plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
        plot.Axes.Bottom.TickLabelStyle.FontSize = 16;
        plot.Axes.Left.TickLabelStyle.FontSize = 16;
        plot.XLabel(xLabel);
        plot.YLabel(yLabel);

        for (int i = 0; i < listOfValues.Count; i++)
        {
            var values = listOfValues[i];
            var pop = plot.Add.Population(values, i);
            
            pop.Bar.IsVisible = false;
            pop.Marker.IsVisible = showMarker;
            pop.Box.IsVisible = true;
            pop.Box.FillColor = pop.Marker.MarkerLineColor.WithAlpha(.5);
            pop.BoxAlignment = HorizontalAlignment.Center;

            // If I want the line to be the mean value
            // pop.BoxValueConfig = PopulationSymbol.BoxValueConfigurator_MeanStdErrStDev;
        }

        // If I want to hide the grid
        // plot.HideGrid();
        plot.SavePng(saveFileLocation, Width, Height);
    }

    public void CreateBoxPlot(string title, string saveFileLocation, List<Box> boxes, double[] tickPositions, string[] tickLabels)
    {
        Plot plot = new();
        plot.Title(title);
        plot.Legend.IsVisible = true;
        plot.Legend.Alignment = Alignment.UpperCenter;
        plot.Legend.ShadowColor = Colors.Blue.WithOpacity(.2);
        plot.Legend.ShadowOffset = new(10, 10);
        plot.Legend.FontName = Fonts.Serif;
        plot.Axes.Bottom.SetTicks(tickPositions, tickLabels);
        
        // plot.Axes.SetLimits(128, 1024, 10, 110);
        
        var boxPlot = plot.Add.Boxes(boxes);
        
        plot.SavePng(saveFileLocation, Width, Height);
    }
    
    
}