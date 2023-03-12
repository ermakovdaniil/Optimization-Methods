using DataAccess.Models;
using OptimizatonMethods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OptimizationMethods.Methods
{
    public static class BoxMethod
    {
        private static double _step;
        private static double _t1min;
        private static double _t1max;
        private static double _t2min;
        private static double _t2max;
        private static double _eps;
        private static Point3D _worstVertex;
        private static Point3D _bestVertex;
        private static Point3D _center;
        private static List<Point3D> _complex;
        private static int vertexCount = 3;
        private static Variant _variant;

        public static void Calculate(Variant variant, out CalculationResults results)
        {
            _eps = variant.Precision;
            _complex = GetInitialComplex();
            _variant = variant;
            do
            {
                _worstVertex = _complex.MaxBy(p => p.Z);
                _bestVertex = _complex.MinBy(p => p.Z);
                _center = GetGravityCenterWithoutWorstVertex();
                var b = GetB();
                if (b < _eps)
                {
                    break;
                }

                var newVertex = GetNewVertexInsteadWorst();

                while (!_variant.SecondCondition(newVertex.X, newVertex.Y))
                {
                    HalfShiftVertex1ToVertex2(newVertex, _center);
                }

                while (newVertex.Z > _worstVertex.Z)
                {
                    HalfShiftVertex1ToVertex2(newVertex, _bestVertex);
                }

                _complex.Remove(_worstVertex);
                _complex.Add(newVertex);
            } while (GetB() >= _eps);


            //return _center;


            //var minPrice = points3D.Select(p => p.Z).Min();
            //var t1 = points3D.Find(x => x.Z == minPrice).X;
            //var t2 = points3D.Find(x => x.Z == minPrice).Y;
            results = new CalculationResults { Price = _center.Z, T1 = _center.X, T2 = _center.Y, Points3D = new ObservableCollection<Point3D>(points3D) };
        }

        // Формирование исходного комплекса
        private static List<Point3D> GetInitialComplex()
        {
            var allPoints = GetRandomPoints();

            while (allPoints.All(p => !_variant.Conditions(p.X, p.Y)))
            {
                allPoints = GetRandomPoints();
            }

            var fixedPoints = allPoints.Where(p => _variant.Conditions(p.X, p.Y)).ToList();
            var unfixedPoints = allPoints.Where(p => !fixedPoints.Contains(p)).ToList();

            while (unfixedPoints.Count != 0)
            {
                var unfixedPoint = unfixedPoints[0];

                while (_variant.Conditions(unfixedPoint.X, unfixedPoint.Y))
                {
                    ShiftVertex(unfixedPoint, fixedPoints);
                }

                unfixedPoints.Remove(unfixedPoint);
                fixedPoints.Add(unfixedPoint);
            }

            return fixedPoints;
        }

        // Смещение вершины к центру вершин комплекса
        private static void ShiftVertex(Point3D vertex, List<Point3D> fixedPoints)
        {
            vertex.X = 0.5 * (vertex.X + 1 / fixedPoints.Count * fixedPoints.Sum(p => p.X));
            vertex.Y = 0.5 * (vertex.Y + 1 / fixedPoints.Count * fixedPoints.Sum(p => p.Y));
            vertex.Z = _variant.Function(vertex.X, vertex.Y);
        }

        // Создание случайных вершин комплекса
        private static List<Point3D> GetRandomPoints()
        {
            var rnd = new Random();
            var complex = new List<Point3D>();

            for (var i = 0; i < vertexCount; i++)
            {
                var t1 = _t1min + rnd.NextDouble() * (_t1max - _t1min);
                var t2 = _t2min + rnd.NextDouble() * (_t2max - _t2min);
                var vertex = new Point3D(t1, t2, _variant.Function(t1, t2));
                complex.Add(vertex);
            }
            return complex;
        }

        // Определение координат центра Комплекса с отброшенной «наихудшей» вершиной
        private static Point3D GetGravityCenterWithoutWorstVertex()
        {
            var t1 = 1.0 / (vertexCount - 1) * (_complex.Sum(p => p.X) - _worstVertex.X);
            var t2 = 1.0 / (vertexCount - 1) * (_complex.Sum(p => p.X) - _worstVertex.X);
            var center = new Point3D(t1, t2, _variant.Function(t1, t2));
            return center;
        }

        // Расчёт среднего расстояния от центра Комплекса до худшей и лучшей вершин
        private static double GetB()
        {
            double sum = 0;
            sum += Math.Abs(_center.X - _worstVertex.X) + Math.Abs(_center.X - _bestVertex.X);
            sum += Math.Abs(_center.Y - _worstVertex.Y) + Math.Abs(_center.Y - _bestVertex.Y);
            return (1.0 / 4.0) * sum;
        }

        // Расчёт новой вершины
        private static Point3D GetNewVertexInsteadWorst()
        {
            var t1 = 2.3 * _center.X - 1.3 * _worstVertex.X;
            var t2 = 2.3 * _center.Y - 1.3 * _worstVertex.Y;
            var newPoint = new Point3D(t1, t2, _variant.Function(t1, t2));

            if(newPoint.X < _t1min)
            {
                newPoint.X = _t1min + _eps;
            }
            if (newPoint.X > _t1max)
            {
                newPoint.X = _t1max - _eps;
            }
            if (newPoint.Y < _t2min)
            {
                newPoint.Y = _t2min + _eps;
            }
            if (newPoint.Y > _t2max)
            {
                newPoint.Y = _t2max - _eps;
            }
            return newPoint;
        }

        // Смещение вершины к центру комплекса на половину расстояния
        private static void HalfShiftVertex1ToVertex2(Point3D vertex1, Point3D vertex2)
        {
            vertex1.X = 0.5 * (vertex1.X + vertex2.X);
            vertex1.Y = 0.5 * (vertex1.Y + vertex2.Y);
            vertex1.Z = _variant.Function(vertex1.X, vertex1.Y);
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
