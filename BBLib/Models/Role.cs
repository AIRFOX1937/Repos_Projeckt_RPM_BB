using System;
using System.Collections.Generic;

namespace BBLib.Models;

public partial class Role
{
    public int IdR { get; set; }

    public string? NameR { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
