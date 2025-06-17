using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class Obrok
{
    public int IdObrok { get; set; }

    public string Naziv { get; set; } = null!;

    public int Proteini { get; set; }

    public int Ugljenihidrati { get; set; }

    public int Masti { get; set; }

    public int Masa { get; set; }

    public int Kalorije { get; set; }

    public int IdTipObroka { get; set; }

    public int IdKorisnik { get; set; }

    public DateTime DatumVrijeme { get; set; }

    public virtual Korisnik IdKorisnikNavigation { get; set; } = null!;

    public virtual TipObroka IdTipObrokaNavigation { get; set; } = null!;
}
