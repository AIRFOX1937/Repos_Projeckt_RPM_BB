using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Raspi
{
    public int IdR { get; set; }

    public int IdG { get; set; }

    public int IdC { get; set; }

    public int IdN { get; set; }

    public int IdT { get; set; }

    public string? DateR { get; set; }

    public int IdD { get; set; }

    public int IdV { get; set; }

    public string AudR { get; set; } = null!;

    public int ZdR { get; set; }

    public int IdU { get; set; }

    public virtual Chet IdCNavigation { get; set; } = null!;

    public virtual Disc IdDNavigation { get; set; } = null!;

    public virtual Group IdGNavigation { get; set; } = null!;

    public virtual Nedeli IdNNavigation { get; set; } = null!;

    public virtual Time IdTNavigation { get; set; } = null!;

    public virtual User IdUNavigation { get; set; } = null!;

    public virtual Vidi IdVNavigation { get; set; } = null!;
}
