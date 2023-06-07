using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Vidi
{
    public int IdV { get; set; }

    public string NameV { get; set; } = null!;

    public virtual ICollection<Doki> Dokis { get; set; } = new List<Doki>();

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();
}
