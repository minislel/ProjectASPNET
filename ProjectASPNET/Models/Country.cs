using System;
using System.Collections.Generic;

namespace ProjectASPNET.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string? CountryName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
