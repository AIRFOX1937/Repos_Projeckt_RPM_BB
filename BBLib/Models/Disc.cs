using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Disc
{
    public int IdD { get; set; }

    public string NameD { get; set; }

    public virtual ICollection<GD> GDs { get; set; } = new List<GD>();

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();

    public virtual ICollection<UD> UDs { get; set; } = new List<UD>();
    public virtual ICollection<Doki> Dokis { get; set; } = new List<Doki>();
}
