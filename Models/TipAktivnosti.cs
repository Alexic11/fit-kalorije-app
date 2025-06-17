using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class TipAktivnosti
{
    public int IdTipAktivnosti { get; set; }

    public string Naziv { get; set; } = null!;

    public int KalorijePoMinuti { get; set; }  // dodato

    public virtual ICollection<Aktivnost> Aktivnosts { get; set; } = new List<Aktivnost>();
}
