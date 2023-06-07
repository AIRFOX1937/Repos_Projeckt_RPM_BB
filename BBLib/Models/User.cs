using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BBLib.Models;

public partial class User
{
    public int IdU { get; set; }

    public string? LoginU { get; set; }

    public string? PassU { get; set; }

    public int? RoleU { get; set; }

    public string? FioU { get; set; }

    public int? GroupS { get; set; }

    public virtual ICollection<Doki> Dokis { get; set; } = new List<Doki>();

    public virtual Group? GroupSNavigation { get; set; }

    public virtual ICollection<Otcheti> Otchetis { get; set; } = new List<Otcheti>();

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();

    public virtual Role? RoleUNavigation { get; set; }

    public virtual ICollection<UD> UDs { get; set; } = new List<UD>();  
}
