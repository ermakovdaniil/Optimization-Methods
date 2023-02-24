using OptimizatonMethods.Models;
using System.Collections.ObjectModel;


namespace DataAccess.Models
{
    public class CalculationResults
    {
        public double Price { get; set; }
        public double T1 { get; set; }
        public double T2 { get; set; }
        public ObservableCollection<Point3D> Points3D { get; set; }
    }
}