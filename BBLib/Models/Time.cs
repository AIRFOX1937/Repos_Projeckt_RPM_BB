using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Time
{
    public int IdT { get; set; }

    public string NameT { get; set; } = null!;

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();
}
