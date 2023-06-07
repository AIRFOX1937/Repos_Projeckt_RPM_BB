using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Otcheti1
{
    public int IdO { get; set; }

    public string NameD { get; set; } = null!;

    public string FioU { get; set; } = null!;

    public string NumG { get; set; } = null!;

    public string Ssilka { get; set; } = null!;

    public DateTime DateO { get; set; }
}
