using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Variant
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public double Alpha { get; set; }

    public double Beta { get; set; }

    public double Mu { get; set; }

    public double MassConsumption { get; set; }

    public double Pressure { get; set; }

    public long Speed { get; set; }

    public double Price { get; set; }

    public double Precision { get; set; }

    public double T1min { get; set; }

    public double T1max { get; set; }

    public double T2min { get; set; }

    public double T2max { get; set; }

    public double TempCondition { get; set; }
}
