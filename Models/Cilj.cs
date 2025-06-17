using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class Cilj
{
    public int IdCilj { get; set; }

    public int DnevniLimitKalorija { get; set; }

    public int IdKorisnik { get; set; }

    public virtual Korisnik IdKorisnikNavigation { get; set; } = null!;
}
