using DataAccess.Models;
using OptimizatonMethods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OptimizationMethods.Methods
{
    public static class ScanMethod
    {
        private static readonly double _k = 10;
        private static readonly double _r = 2;
        private static double _step;
        private static double _t1min;
        private static double _t1max;
        private static double _t2min;
        private static double _t2max;

        public static void Calculate(Variant variant, out CalculationResults results)
        {
            _t1min = variant.T1min;
            _t2min = variant.T2min;
            _t1max = variant.T1max;
            _t2max = variant.T2max;

            var funcMin = double.MaxValue;
            _step = Math.Pow(_k, _r) * variant.Precision;
            var points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            OptimizatonMethods.Models.Point newMin;
            newMin = SearchMinOnGrid(variant, out p3D, out values);
            _t1min = newMin.X - _step;
            _t2min = newMin.Y - _step;
            _t1max = newMin.X + _step;
            _t2max = newMin.Y + _step;
            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMin > values.Min())
            {
                newMin = SearchMinOnGrid(variant, out p3D, out values);
                _t1min = newMin.X - _step;
                _t2min = newMin.Y - _step;
                _t1max = newMin.X + _step;
                _t2max = newMin.Y + _step;
                _step /= _k;
                funcMin = values.Min();
                points3D.AddRange(p3D);
            }
            var minPrice = points3D.Select(p => p.Z).Min();
            var t1 = points3D.Find(x => x.Z == minPrice).X;
            var t2 = points3D.Find(x => x.Z == minPrice).Y;
            results = new CalculationResults { Price = minPrice, T1 = t1, T2 = t2, Points3D = new ObservableCollection<Point3D>(points3D) };
        }

        private static OptimizatonMethods.Models.Point SearchMinOnGrid(Variant variant, out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();
            for (var t1 = _t1min; t1 <= _t1max; t1 += _step)
            {
                for (var t2 = _t2min; t2 <= _t2max; t2 += _step)
                {
                    if (!variant.Conditions(t1, t2))
                    {
                        continue;
                    }

                    var value = variant.Function(t1, t2);
                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));
                }
            }
            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;
            return new OptimizatonMethods.Models.Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }
    }
}
