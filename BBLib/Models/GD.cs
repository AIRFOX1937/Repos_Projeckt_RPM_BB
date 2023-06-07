using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class GD
{
    public int IdGD { get; set; }

    public int IdG { get; set; }

    public int IdD { get; set; }

    public virtual Disc IdDNavigation { get; set; } = null!;

    public virtual Group IdGNavigation { get; set; } = null!;
}
