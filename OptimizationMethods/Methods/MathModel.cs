using DataAccess.Models;
using System;

namespace OptimizationMethods.Methods
{
    public class MathModel : IMathModel
    {
        //private readonly Variant _variant;
        //private readonly double _epsilon;
        //private readonly double _k = 10;
        //// private double _n = 2;
        //private readonly double _r = 2;
        //private double _step;
        //private readonly double alpha;
        //private readonly double beta;
        //private readonly double mu;
        //private readonly double g;
        //private readonly double a;
        //private readonly double n;
        //private readonly double price;
        //private double t1min;
        //private double t1max;
        //private double t2min;
        //private double t2max;
        //private readonly double tempCondition;

        //public MathModel() { }

        //public MathModel(Variant variant)
        //{
        //    _variant = variant;
        //    price = (double)_variant.Price;
        //    alpha = (double)_variant.Alpha;
        //    beta = (double)_variant.Beta;
        //    mu = (double)_variant.Mu;
        //    g = (double)_variant.MassConsumption;
        //    a = (double)_variant.Pressure;
        //    n = (double)_variant.Speed;
        //    t1min = (double)_variant.T1min;
        //    t1max = (double)_variant.T1max;
        //    t2min = (double)_variant.T2min;
        //    t2max = (double)_variant.T2max;
        //    _epsilon = (double)_variant.Precision;
        //    tempCondition = (double)_variant.TempCondition;
        //}

        //public double Function(double t1, double t2)
        //{
        //    return price * alpha * (g * mu * (Math.Pow(t2 - t1, n) + (Math.Pow(beta * a - t1, n))));
        //}

        //public bool Conditions(double t1, double t2)
        //{
        //    return t1 >= t1min && t1 <= t1max && t2 >= t2min && t2 <= t2max && t1 + 0.5 * t2 <= tempCondition;
        //}

        public CalculationResults Calculate(Method method, Variant variant)
        {
            CalculationResults results = new CalculationResults();
            switch (method.Id)
            {
                case 1:
                    ScanMethod.Calculate(variant, out results);
                    break;
                case 2:
                    //BoxMethod.Calculate(variant, out results);
                    break;
                default:
                    break;
            }
            return results;
        }
    }
}