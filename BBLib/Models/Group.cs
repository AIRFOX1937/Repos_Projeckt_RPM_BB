using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Group
{
    public int IdG { get; set; }

    public string? NumG { get; set; }

    public virtual ICollection<GD> GDs { get; set; } = new List<GD>();

    public virtual ICollection<Raspi> Raspis { get; set; } = new List<Raspi>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
