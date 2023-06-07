using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BBLib.Models;

public partial class UD
{
    public int IdUD { get; set; }

    public int IdU { get; set; }

    public int IdD { get; set; }

    public virtual Disc IdDNavigation { get; set; } = null!;

    public virtual User IdUNavigation { get; set; } = null!;

    [IgnoreDataMember]
    public string? FioU { get; set; }

    [IgnoreDataMember]
    public string? NameD { get; set; }
}
