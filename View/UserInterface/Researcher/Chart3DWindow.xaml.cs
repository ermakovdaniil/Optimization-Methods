using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using ChartDirector;

using OptimizatonMethods.Models;


namespace View.UserInterface.Researcher.Charts
{
    /// <summary>
    ///     Логика взаимодействия для Chart3DWindow.xaml
    /// </summary>
    public partial class Chart3DWindow : UserControl
    {
        private readonly List<Point3D> _dataList;
        
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
                createChart();
            }
        }

        // 3D view angles
        private double m_elevationAngle;
        private bool m_isDragging;

        // Keep track of mouse drag
        private int m_lastMouseX;
        private int m_lastMouseY;
        private double m_rotationAngle;

        public Chart3DWindow()
        {
            InitializeComponent();
        }

        //public Chart3DWindow(List<Point3D> dataList, Task task)
        //{
        //    _dataList = dataList;
        //    _task = task;
        //    InitializeComponent();

        //    // 3D view angles
        //    m_elevationAngle = 30;
        //    m_rotationAngle = 45;

        //    // Keep track of mouse drag
        //    m_isDragging = false;
        //    m_lastMouseX = -1;
        //    m_lastMouseY = -1;

        //    // Draw the chart
        //    //WPFChartViewer1.updateViewPort(true, false);
        //}

        private void Chart3DWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //createChart(WPFChartViewer1);
        }

        //Name of demo module
        public string getName()
        {
            return "Surface Chart (2)";
        }

        //Number of charts produced in this demo module
        public int getNoOfCharts()
        {
            return 1;
        }

        //Main code for creating chart.
        public void createChart()
        {
            ////double[] dataX = {-18, -17, -16, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7,};
            ////double[] dataY = {-8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8,};
            //double[] dataX = { -3, -2, -1, 0, 1, 2, 3 };
            //double[] dataY = { -2, -1, 0, 1, 2, 3, 4, 5, 6 };

            //var dataZ = new double[dataX.Length * dataY.Length];
            //var k = 0;
            //var math = new MathModel(_task);

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
            var mathModel = new MathModel(_task);

            for (double i =  mathModel.t1min - step; i < mathModel.t1max + step; i += step)
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

            // Create a SurfaceChart object of size 680 x 580 pixels
            var c = new SurfaceChart(1299, 865);

            // Set the center of the plot region at (310, 280), and set width x depth x height to
            // 320 x 320 x 240 pixels
            c.setPlotRegion(510, 380, 420, 420, 540);

            // Set the elevation and rotation angles to 30 and 45 degrees
            c.setViewAngle(m_elevationAngle, m_rotationAngle);

            if (m_isDragging && DrawFrameOnRotate.IsChecked.Value)
            {
                c.setShadingMode(Chart.RectangularFrame);
            }

            // Set the data to use to plot the chart
            c.setData(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());

            // Spline interpolate data to a 80 x 80 grid for a smooth surface
            c.setInterpolation(80, 80);

            // Use semi-transparent black (c0000000) for x and y major surface grid lines. Use
            // dotted style for x and y minor surface grid lines.
            var majorGridColor = unchecked((int) 0xc0000000);
            var minorGridColor = c.dashLineColor(majorGridColor, Chart.DotLine);
            c.setSurfaceAxisGrid(majorGridColor, majorGridColor, minorGridColor, minorGridColor);

            // Add XY projection
            c.addXYProjection();

            // Set contour lines to semi-transparent white (0x7fffffff)
            c.setContourColor(0x7fffffff);

            // Add a color axis (the legend) in which the left center is anchored at (620, 250). Set
            // the length to 200 pixels and the labels on the right side.
            c.setColorAxis(1000, 400, Chart.Left, 400, Chart.Right);

            // Set the x, y and z axis titles using 12 pt Arial Bold font
            c.xAxis().setTitle("Температура в змеевике, °C", "Arial Bold", 12);
            c.yAxis().setTitle("Температура в рубашке, °C", "Arial Bold", 12);
            c.zAxis().setTitle("Себестоимость, у.е.", "Arial Bold", 12);

            //c.yAxis().setAutoScale(-4, 12, 1);
            //c.zAxis().setAutoScale(0, 0);
            //c.zAxis().setLinearScale(0, 1200, 100);

            //c.zAxis().setAutoScale(0, 1200, 100);

            // Output the chart
            WPFChartViewer1.Chart = c;

            //include tool tip for the chart
            WPFChartViewer1.ImageMap = c.getHTMLImageMap("clickable", "",
                                                "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
        }




        private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
        {
            // Update the chart if necessary
            if (e.NeedUpdateChart)
            {
                createChart();
            }
        }

        private void WPFChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
        {
            var mouseX = WPFChartViewer1.ChartMouseX;
            var mouseY = WPFChartViewer1.ChartMouseY;

            // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (m_isDragging)
                {
                    // The chart is configured to rotate by 90 degrees when the mouse moves from 
                    // left to right, which is the plot region width (360 pixels). Similarly, the
                    // elevation changes by 90 degrees when the mouse moves from top to buttom,
                    // which is the plot region height (270 pixels).
                    m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                    m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                    WPFChartViewer1.updateViewPort(true, false);
                }

                // Keep track of the last mouse position
                m_lastMouseX = mouseX;
                m_lastMouseY = mouseY;
                m_isDragging = true;
            }
        }

        private void WPFChartViewer1_MouseUpChart(object sender, MouseButtonEventArgs e)
        {
            m_isDragging = false;
            WPFChartViewer1.updateViewPort(true, false);
        }
    }
}
