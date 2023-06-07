using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Nedeli
{
    public int IdN { get; set; }

    public string NameN { get; set; } = null!;

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();
}
