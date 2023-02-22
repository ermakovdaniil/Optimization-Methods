using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Method
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public byte[] Active { get; set; } = null!;
}
