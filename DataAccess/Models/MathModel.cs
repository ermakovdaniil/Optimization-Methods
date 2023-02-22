using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace OptimizatonMethods.Models
{
    public struct CalculationResults
    {
        public double Price { get; init; }
        public double T1 { get; init; }
        public double T2 { get; init; }
        public ObservableCollection<Point3D> Points3D { get; init; }    
    }

    public class MathModel
    {
        public CalculationResults Results { get; private set; }
        private readonly Task _task;
        private readonly double _epsilon;
        private readonly double _k = 10;
        // private double _n = 2;
        private readonly double _r = 2;
        private double _step;
        private readonly double alpha;
        private readonly double beta;
        private readonly double mu;
        private readonly double g;
        private readonly double a;
        private readonly double n;
        private readonly double price;
        public double t1min;
        public double t1max;
        public double t2min;
        public double t2max;
        public readonly double tempCondition;

        public MathModel(Task task)
        {
            _task = task;
            price = (double) _task.Price;
            alpha = (double) _task.Alpha;
            beta = (double) _task.Beta;
            mu = (double) _task.Mu;
            g = (double) _task.G;
            a = (double) _task.A;
            n = (double) _task.N;
            t1min = (double) _task.T1min;
            t1max = (double) _task.T1max;
            t2min = (double) _task.T2min;
            t2max = (double) _task.T2max;
            _epsilon = (double)_task.Precision;
            tempCondition = (double) _task.TempCondition;
        }

        public int CalculationCount { get; private set; }

        public double Function(double t1, double t2)
        {
            return price * alpha * ( g * mu * (Math.Pow(t2 - t1, n) + (Math.Pow(beta * a - t1, n) ) ) );
        }

        private bool Conditions(double t1, double t2)
        {
            return t1 >= t1min && t1 <= t1max && t2 >= t2min && t2 <= t2max && t1 + 0.5 * t2 <= tempCondition;
        }

        public void Calculate()
        {
            var funcMin = double.MaxValue;
            _step = Math.Pow(_k, _r) * _epsilon;
            var points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            Point newMin;
            newMin = SearchMinOnGrid(out p3D, out values);
            t1min = newMin.X - _step;
            t2min = newMin.Y - _step;
            t1max = newMin.X + _step;
            t2max = newMin.Y + _step;
            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMin > values.Min())
            {
                newMin = SearchMinOnGrid(out p3D, out values);
                t1min = newMin.X - _step;
                t2min = newMin.Y - _step;
                t1max = newMin.X + _step;
                t2max = newMin.Y + _step;
                _step /= _k;
                funcMin = values.Min();
                points3D.AddRange(p3D);
            }
            var minPrice = points3D.Select(p => p.Z).Min();
            var t1 = points3D.Find(x => x.Z == minPrice).X;
            var t2 = points3D.Find(x => x.Z == minPrice).Y;
            Results = new CalculationResults { Price = minPrice, T1 = t1, T2 = t2, Points3D = new ObservableCollection<Point3D>(points3D) };
        }

        private Point SearchMinOnGrid(out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();
            for (var t1 = t1min; t1 <= t1max; t1 += _step)
            {
                for (var t2 = t2min; t2 <= t2max; t2 += _step)
                {
                    if (!Conditions(t1, t2))
                    {
                        continue;
                    }

                    CalculationCount++;
                    var value = Function(t1, t2);

                    if (value < 0)
                    {
                        //MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }

                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));
                }
            }
            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;
            return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }
    }
}
