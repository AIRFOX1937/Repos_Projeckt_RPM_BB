using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Doki
{
    public int IdD { get; set; }

    public int IdU { get; set; }

    public int IdV { get; set; }

    public string NameD { get; set; } = null!;

    public string SsilkaD { get; set; } = null!;

    public int FlagD { get; set; }
    public int IdDi { get; set; }

    public virtual User IdUNavigation { get; set; } = null!;
    public virtual Disc IdDiNavigation { get; set; } = null!;

    public virtual Vidi IdVNavigation { get; set; } = null!;

    public virtual ICollection<Otcheti> Otchetis { get; set; } = new List<Otcheti>();
}
