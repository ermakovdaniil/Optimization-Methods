using DataAccess.Models;
using OptimizatonMethods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OptimizationMethods.Methods
{
    public static class ScanMethod
    {
        public static int CalculationCount { get; private set; }

        private static readonly double _k = 10;
        private static readonly double _r = 2;
        private static double _step;

        public static void Calculate(DataAccess.Models.Task task, out CalculationResults results)
        {
            var funcMin = double.MaxValue;
            _step = Math.Pow(_k, _r) * task.Precision;
            var points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            OptimizatonMethods.Models.Point newMin;
            newMin = SearchMinOnGrid(task, out p3D, out values);
            task.T1min = newMin.X - _step;
            task.T2min = newMin.Y - _step;
            task.T1max = newMin.X + _step;
            task.T2max = newMin.Y + _step;
            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMin > values.Min())
            {
                newMin = SearchMinOnGrid(task, out p3D, out values);
                task.T1min = newMin.X - _step;
                task.T2min = newMin.Y - _step;
                task.T1max = newMin.X + _step;
                task.T2max = newMin.Y + _step;
                _step /= _k;
                funcMin = values.Min();
                points3D.AddRange(p3D);
            }
            var minPrice = points3D.Select(p => p.Z).Min();
            var t1 = points3D.Find(x => x.Z == minPrice).X;
            var t2 = points3D.Find(x => x.Z == minPrice).Y;
            results = new CalculationResults { Price = minPrice, T1 = t1, T2 = t2, Points3D = new ObservableCollection<Point3D>(points3D) };
        }

        private static OptimizatonMethods.Models.Point SearchMinOnGrid(DataAccess.Models.Task task, out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();
            var methods = new MathModel(task);
            for (var t1 = task.T1min; t1 <= task.T1max; t1 += _step)
            {
                for (var t2 = task.T2min; t2 <= task.T2max; t2 += _step)
                {
                    if (!methods.Conditions(t1, t2))
                    {
                        continue;
                    }

                    CalculationCount++;
                    var value = methods.Function(t1, t2);

                    if (value < 0)
                    {
                        //MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }

                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));
                }
            }
            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;
            return new OptimizatonMethods.Models.Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }
    }
}
