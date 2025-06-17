using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class Aktivnost
{
    public int IdAktivnost { get; set; }

    public string Naziv { get; set; } = null!;

    //public int KalorijePoMinuti { get; set;
    public int TrajanjeUMinutama { get; set; }  // novo polje

    public DateTime DatumVrijeme { get; set; }

    public int IdTipaAktivnosti { get; set; }

    public int IdKorisnika { get; set; }

    public virtual Korisnik IdKorisnikaNavigation { get; set; } = null!;

    public virtual TipAktivnosti IdTipaAktivnostiNavigation { get; set; } = null!;
}
