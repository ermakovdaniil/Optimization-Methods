using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using ChartDirector;

using OptimizatonMethods.Models;
using DataAccess.Models;
using OptimizationMethods.Methods;

namespace View.UserInterface.Researcher.Charts
{
    /// <summary>
    ///     Логика взаимодействия для Chart3DWindow.xaml
    /// </summary>
    public partial class Chart3DControl : UserControl
    {
        #region Properties

        private Variant _variant;

        public Variant Variant
        {
            get
            {
                return _variant;
            }
            set
            {
                _variant = value;
                createChart();
            }
        }
        private readonly List<Point3D> _dataList;
        private ContourLayer contourLayer;
        public ThreeDChart baseChart;
        public string imgMap;
        private SurfaceChart chart;

        private double m_elevationAngle;
        private bool m_isDragging;

        private int m_lastMouseX;
        private int m_lastMouseY;
        private double m_rotationAngle;

        #endregion


        #region Functions


        #region Constructors

        public Chart3DControl()
        {
            InitializeComponent();
        }

        #endregion

        public void createChart()
        {
            var dataX = new List<double>();
            var dataY = new List<double>();
            //var method = new MathModel(Variant);
            var step = 1;

            for (double i = Variant.T1min - step; i < Variant.T1max + step; i += step)
            {
                dataX.Add(i);
            }

            for (double i = Variant.T2min - step; i < Variant.T2max + step; i += step)
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
                    dataZ[j * dataX.Count + i] = Variant.Function(dataX[i], dataY[j]);
                }
            }
            chart = new SurfaceChart(1299, 865);
            chart.setPlotRegion(510, 380, 420, 420, 540);
            chart.setViewAngle(m_elevationAngle, m_rotationAngle);
            if (m_isDragging && DrawFrameOnRotate.IsChecked.Value)
            {
                chart.setShadingMode(Chart.RectangularFrame);
            }
            chart.setData(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
            chart.setInterpolation(80, 80);
            var majorGridColor = unchecked((int) 0xc0000000);
            var minorGridColor = chart.dashLineColor(majorGridColor, Chart.DotLine);
            chart.setSurfaceAxisGrid(majorGridColor, majorGridColor, minorGridColor, minorGridColor);
            chart.addXYProjection();
            chart.setContourColor(0x7fffffff);
            chart.setColorAxis(1000, 400, Chart.Left, 400, Chart.Right);
            chart.xAxis().setTitle("Температура в змеевике, °C", "Arial Bold", 12);
            chart.yAxis().setTitle("Температура в рубашке, °C", "Arial Bold", 12);
            chart.zAxis().setTitle("Себестоимость, у.е.", "Arial Bold", 12);
            WPFChartViewer1.Chart = chart;
            WPFChartViewer1.ImageMap = chart.getHTMLImageMap("clickable", "", "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
        }

        private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
            {
                createChart();
            }
        }

        private void WPFChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
        {
            var mouseX = WPFChartViewer1.ChartMouseX;
            var mouseY = WPFChartViewer1.ChartMouseY;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (m_isDragging)
                {
                    m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                    m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                    WPFChartViewer1.updateViewPort(true, false);
                }
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

        #endregion
    }
}