using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Chet
{
    public int IdC { get; set; }

    public string NameC { get; set; } = null!;

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();
}
