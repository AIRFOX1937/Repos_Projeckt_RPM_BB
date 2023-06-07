using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Otcheti
{
    public int IdO { get; set; }

    public int IdD { get; set; }

    public int IdU { get; set; }

    public string Ssilka { get; set; } = null!;

    public DateTime DateO { get; set; }

    public virtual Doki IdDNavigation { get; set; } = null!;

    public virtual User IdUNavigation { get; set; } = null!;
}
