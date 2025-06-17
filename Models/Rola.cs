using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class Rola
{
    public int IdRola { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Korisnik> Korisniks { get; set; } = new List<Korisnik>();
}
