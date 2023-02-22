using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ChartDirector;
using OptimizatonMethods.Models;


namespace View.UserInterface.Researcher.Charts
{
    /// <summary>
    ///     Логика взаимодействия для Chart2DWindow.xaml
    /// </summary>
    public partial class Chart2DWindow : UserControl
    {        
        private readonly List<Point3D> _dataList = new();
        private ContourLayer contourLayer;
    
        private Task _task;

        public Task Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
            }
        }

        public Chart2DWindow() 
        { 
            InitializeComponent(); 
        }  

        public Chart2DWindow(List<Point3D> dataList, Task task)
        {
            _dataList = dataList;
            _task = task;
            InitializeComponent();
        }

        public void drawChart(Task task)
        {
            //double[] dataX = { -3, -2, -1, 0, 1, 2, 3 };
            //double[] dataY = { -2, -1, 0, 1, 2, 3, 4, 5, 6 }; 
            //var dataZ = new double[dataX.Length * dataY.Length];
            //var k = 0;
            //var math = new MathModel(task);

            //for (var i = 0; i < dataX.Length; i++)
            //{
            //    for (var j = 0; j < dataY.Length; j++)
            //    {
            //        if (Math.Abs(dataY[j] - dataX[i]) < 2)
            //        {
            //            dataZ[k] = -1;
            //            k++;
            //        }
            //        else
            //        {
            //            if (math.Function(dataX[i], dataY[j]) < 1000)
            //            {
            //                dataZ[k] = math.Function(dataX[i], dataY[j]);
            //            }
            //            else
            //            {
            //                dataZ[k] = 1000;
            //            }
            //            k++;
            //        }
            //    }
            //}

            var dataX = new List<double>();
            var dataY = new List<double>();
            var step = 1;
            var mathModel = new MathModel(task);

            for (double i = mathModel.t1min - step; i < mathModel.t1max + step; i += step)
            {
                dataX.Add(i);
            }
            for (double i = mathModel.t2min - step; i < mathModel.t2max + step; i += step)
            {
                dataY.Add(i);
            }
            var dataZ = new List<double>();

            for (int i = 0; i < dataX.Count; i++)
            {
                for (int j = 0; j < dataY.Count; j++)
                {
                    dataZ.Add(0);
                }
            }
            var k = 0;

            for (int i = 0; i < dataX.Count; i++)
            {
                for (int j = 0; j < dataY.Count; j++)
                {
                    dataZ[j * dataX.Count + i] = mathModel.Function(dataX[i], dataY[j]);
                }
            }

            // Create a XYChart object of size 575 x 525 pixels
            var c = new XYChart(1240, 855);

            // Set the plotarea at (75, 30) and of size 450 x 450 pixels. Use semi-transparent black
            // (80000000) dotted lines for both horizontal and vertical grid lines
            var p = c.setPlotArea(120, 30, 1050, 750, -1, -1, -1, c.dashLineColor(unchecked((int) 0xaf000000), Chart.DotLine), -1);

            // Set the chart and axis titles
            c.addTitle("2D график линий равных значений целевой функции",
                       "Arial Bold", 15);

            c.xAxis().setTitle("Температура в змеевике, °C", "Arial Bold Italic", 10);
            c.yAxis().setTitle("Температура в рубашке, °C", "Arial Bold Italic", 10);

            // Put the y-axis on the right side of the chart
            c.setYAxisOnRight();

            // Set x-axis and y-axis labels to use Arial Bold font
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // When auto-scaling, use tick spacing of 40 pixels as a guideline
            //c.xAxis().setLinearScale(-20, 10, 1);
            //c.yAxis().setLinearScale(4, 19, 1);

            // Add a contour layer using the given data
            contourLayer = c.addContourLayer(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
            contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");

            contourLayer.setZBounds(0);

            // Move the grid lines in front of the contour layer
            c.getPlotArea().moveGridBefore(contourLayer);

            // Add a vertical color axis at x = 0 at the same y-position as the plot area.
            var cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft,
                                                  p.getHeight(), Chart.Right);

            // Use continuous gradient coloring (as opposed to step colors)
            cAxis.setColorGradient(true);

            // Add a title to the color axis using 12 points Arial Bold Italic font
            cAxis.setTitle("Себестоимость, у.е.", "Arial Bold Italic", 10);

            // Set color axis labels to use Arial Bold font
            cAxis.setLabelStyle("Arial", 10);

            // Set the chart image to the WinChartViewer
            WPFChartViewer1.Chart = c;

            // Tooltip for the contour chart
            WPFChartViewer1.ImageMap = c.getHTMLImageMap("", "",
                                                "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");

            // Initializse the crosshair position to the center of the chart

            // Draw the cross section and crosshair
        }

        private void Chart2DWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //drawChart(WPFChartViewer1);

            // Extended the plot area mouse event region to make it easier to drag the crosshair
            //WPFChartViewer1.setPlotAreaMouseMargin(100);
        }
    }
}
