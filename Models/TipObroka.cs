using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class TipObroka
{
    public int IdTipObroka { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Obrok> Obroks { get; set; } = new List<Obrok>();
}
