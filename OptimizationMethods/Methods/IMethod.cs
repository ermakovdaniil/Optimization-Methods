﻿using DataAccess.Models;

namespace OptimizationMethods.Methods
{
    public interface IMethod
    {
        public double Function(double t1, double t2);
        public bool Conditions(double t1, double t2);
        public CalculationResults Calculate(Method method, DataAccess.Models.Task task);
    }
}
