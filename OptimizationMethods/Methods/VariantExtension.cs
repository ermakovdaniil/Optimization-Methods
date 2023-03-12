using DataAccess.Models;
using System;

namespace OptimizationMethods.Methods
{
    public static class VariantExtension
    {
        public static double Function(this Variant variant, double t1, double t2)
        {
            return variant.Price * variant.Alpha * (variant.MassConsumption * variant.Mu * (Math.Pow(t2 - t1, variant.Speed) + (Math.Pow(variant.Beta * variant.Alpha - t1, variant.Speed))));
        }

        public static bool Conditions(this Variant variant, double t1, double t2)
        {
            return t1 >= variant.T1min && t1 <= variant.T1max && t2 >= variant.T2min && t2 <= variant.T2max && t1 + 0.5 * t2 <= variant.TempCondition;
        }

        public static bool FirstCondition(this Variant variant, double t1, double t2)
        {
            return t1 >= variant.T1min && t1 <= variant.T1max && t2 >= variant.T2min && t2 <= variant.T2max;
        }

        public static bool SecondCondition(this Variant variant, double t1, double t2)
        {
            return t1 + 0.5 * t2 <= variant.TempCondition;
        }
    }
}
