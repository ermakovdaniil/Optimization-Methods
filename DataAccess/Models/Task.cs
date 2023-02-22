using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Task
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public double? Alpha { get; set; }

    public double? Beta { get; set; }

    public double? Nu { get; set; }

    public double? MassConsumption { get; set; }

    public double? Pressure { get; set; }

    public long? Speed { get; set; }

    public double? Price { get; set; }

    public double? Accuracy { get; set; }
}
