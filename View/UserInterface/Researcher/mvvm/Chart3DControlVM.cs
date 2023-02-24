using ChartDirector;
using DataAccess.Models;
using OptimizatonMethods.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View.UserInterface.Researcher.Charts
{
    /// <summary>
    ///     Логика взаимодействия для Chart3DWindow.xaml
    /// </summary>
    public partial class Chart3DControlVM : UserControl
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

        private double m_elevationAngle;
        private bool m_isDragging;

        private int m_lastMouseX;
        private int m_lastMouseY;
        private double m_rotationAngle;

        public void createChart()
        {
            var dataX = new List<double>();
            var dataY = new List<double>();
            var step = 1;
            //var mathModel = new MathModel(_task);

            //for (double i = mathModel.t1min - step; i < mathModel.t1max + step; i += step)
            //{
            //    dataX.Add(i);
            //}
            //for (double i = mathModel.t2min - step; i < mathModel.t2max + step; i += step)
            //{
            //    dataY.Add(i);
            //}
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
                    //dataZ[j * dataX.Count + i] = mathModel.Function(dataX[i], dataY[j]);
                }
            }

            var c = new SurfaceChart(1299, 865);
            c.setPlotRegion(510, 380, 420, 420, 540);
            c.setViewAngle(m_elevationAngle, m_rotationAngle);
            //if (m_isDragging && DrawFrameOnRotate.IsChecked.Value)
            //{
            //    c.setShadingMode(Chart.RectangularFrame);
            //}
            c.setData(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
            c.setInterpolation(80, 80);
            var majorGridColor = unchecked((int)0xc0000000);
            var minorGridColor = c.dashLineColor(majorGridColor, Chart.DotLine);
            c.setSurfaceAxisGrid(majorGridColor, majorGridColor, minorGridColor, minorGridColor);
            c.addXYProjection();
            c.setContourColor(0x7fffffff);
            c.setColorAxis(1000, 400, Chart.Left, 400, Chart.Right);
            c.xAxis().setTitle("Температура в змеевике, °C", "Arial Bold", 12);
            c.yAxis().setTitle("Температура в рубашке, °C", "Arial Bold", 12);
            c.zAxis().setTitle("Себестоимость, у.е.", "Arial Bold", 12);

            //WPFChartViewer1.Chart = c;
            //WPFChartViewer1.ImageMap = c.getHTMLImageMap("clickable", "", "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
        }

        private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
            {
                createChart();
            }
        }

        //private void WPFChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
        //{
        //    var mouseX = WPFChartViewer1.ChartMouseX;
        //    var mouseY = WPFChartViewer1.ChartMouseY;
        //    if (Mouse.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (m_isDragging)
        //        {
        //            m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
        //            m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
        //            WPFChartViewer1.updateViewPort(true, false);
        //        }

        //        // Keep track of the last mouse position
        //        m_lastMouseX = mouseX;
        //        m_lastMouseY = mouseY;
        //        m_isDragging = true;
        //    }
        //}

        //private void WPFChartViewer1_MouseUpChart(object sender, MouseButtonEventArgs e)
        //{
        //    m_isDragging = false;
        //    WPFChartViewer1.updateViewPort(true, false);
        //}
    }
}
