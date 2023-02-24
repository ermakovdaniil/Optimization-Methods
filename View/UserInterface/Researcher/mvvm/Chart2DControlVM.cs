using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ChartDirector;
using DataAccess.Models;
using OptimizationMethods.Methods;
using OptimizatonMethods.Models;
using View.Utils;

namespace View.UserInterface.Researcher.Charts
{
    /// <summary>
    ///     Логика взаимодействия для Chart2DWindow.xaml
    /// </summary>
    public partial class Chart2DControlVM : UserControl
    {
        #region Functions

        #region Constructors

        public Chart2DControlVM(List<Point3D> dataList, Task task)
        {
            _dataList = dataList;
            _task = task;
        }

        #endregion

        #endregion


        #region Properties

        private Task _task;
        public DataAccess.Models.Task Task { get; set; }
        private readonly List<Point3D> _dataList = new();
        private ContourLayer contourLayer;
        public BaseChart baseChart;
        public string imgMap;
        private XYChart chart;

        #endregion


        #region Commands

        private RelayCommand _addTask;
        public RelayCommand AddTask
        {
            get
            {
                return _addTask ??= new RelayCommand(o =>
                {
                    var dataX = new List<double>();
                    var dataY = new List<double>();
                    var method = new Methods(Task);
                    var step = 1;

                    for (double i = Task.T1min - step; i < Task.T1max + step; i += step)
                    {
                        dataX.Add(i);
                    }

                    for (double i = Task.T2min - step; i < Task.T2max + step; i += step)
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
                            dataZ[j * dataX.Count + i] = method.Function(dataX[i], dataY[j]);
                        }
                    }

                    chart = new XYChart(1240, 855);
                    var p = chart.setPlotArea(120, 30, 1050, 750, -1, -1, -1, chart.dashLineColor(unchecked((int)0xaf000000), Chart.DotLine), -1);
                    chart.addTitle("2D график линий равных значений целевой функции", "Arial Bold", 15);
                    chart.xAxis().setTitle("Температура в змеевике, °C", "Arial Bold Italic", 10);
                    chart.yAxis().setTitle("Температура в рубашке, °C", "Arial Bold Italic", 10);
                    chart.setYAxisOnRight();
                    chart.xAxis().setLabelStyle("Arial", 10);
                    chart.yAxis().setLabelStyle("Arial", 10);
                    contourLayer = chart.addContourLayer(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
                    contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");
                    contourLayer.setZBounds(0);
                    chart.getPlotArea().moveGridBefore(contourLayer);
                    var cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft, p.getHeight(), Chart.Right);
                    cAxis.setColorGradient(true);
                    cAxis.setTitle("Себестоимость, у.е.", "Arial Bold Italic", 10);
                    cAxis.setLabelStyle("Arial", 10);
                    baseChart = chart;
                    imgMap = chart.getHTMLImageMap("", "", "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");
                    //WPFChartViewer1.Chart = c;
                    //WPFChartViewer1.ImageMap = c.getHTMLImageMap("", "", "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");
                });
            }
        }

        #endregion
    }
}
