using System;
using System.Collections.Generic;

namespace Fit.Models;

public partial class Korisnik
{
    public int IdKorisnik { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public string KorisnickoIme { get; set; } = null!;

    public string Lozinka { get; set; } = null!;

    public int IdRole { get; set; }

    public virtual ICollection<Aktivnost> Aktivnosts { get; set; } = new List<Aktivnost>();

    public virtual ICollection<Cilj> Ciljs { get; set; } = new List<Cilj>();

    public virtual Rola IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Obrok> Obroks { get; set; } = new List<Obrok>();
}
