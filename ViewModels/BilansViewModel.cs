using Fit.Models;
using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Fit.ViewModels
{
    public class BilansViewModel : INotifyPropertyChanged
    {
        private readonly FitAppContext _context = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string naziv)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naziv));

        public int IdTrenutnogKorisnika { get; }

        // Int property-jevi (za logiku)
        private int _uneseneKalorije;
        public int UneseneKalorije
        {
            get => _uneseneKalorije;
            set
            {
                if (_uneseneKalorije != value)
                {
                    _uneseneKalorije = value;
                    OnPropertyChanged(nameof(UneseneKalorije));
                    OnPropertyChanged(nameof(UneseneKalorijeText));
                }
            }
        }

        private int _potroseneKalorije;
        public int PotroseneKalorije
        {
            get => _potroseneKalorije;
            set
            {
                if (_potroseneKalorije != value)
                {
                    _potroseneKalorije = value;
                    OnPropertyChanged(nameof(PotroseneKalorije));
                    OnPropertyChanged(nameof(PotroseneKalorijeText));
                }
            }
        }

        private int _ciljKalorija;
        public int CiljKalorija
        {
            get => _ciljKalorija;
            set
            {
                if (_ciljKalorija != value)
                {
                    _ciljKalorija = value;
                    OnPropertyChanged(nameof(CiljKalorija));
                    OnPropertyChanged(nameof(CiljKalorijaText));
                }
            }
        }

        private int _preostaloZaUnos;
        public int PreostaloZaUnos
        {
            get => _preostaloZaUnos;
            set
            {
                if (_preostaloZaUnos != value)
                {
                    _preostaloZaUnos = value;
                    OnPropertyChanged(nameof(PreostaloZaUnos));
                    OnPropertyChanged(nameof(PreostaloZaUnosText));
                }
            }
        }

        // String property-jevi za XAML binding (s dodanim "kcal" i sl.)
        public string UneseneKalorijeText => $"{UneseneKalorije} kcal";
        public string PotroseneKalorijeText => $"{PotroseneKalorije} kcal";
        public string CiljKalorijaText => $"{CiljKalorija} kcal";
        public string PreostaloZaUnosText => $"{PreostaloZaUnos} kcal";

        // Bilans kao string (npr. "Dobar" ili "Loš")
        private string _bilansStanje = "Neutralan";
        public string BilansStanje
        {
            get => _bilansStanje;
            set
            {
                if (_bilansStanje != value)
                {
                    _bilansStanje = value;
                    OnPropertyChanged(nameof(BilansStanje));
                }
            }
        }

        // Konstruktor prima ID korisnika (ili možeš napraviti default bez parametra)
        public BilansViewModel(int korisnikId = 0)
        {
            IdTrenutnogKorisnika = korisnikId;
            UcitajBilans();
        }

        private void UcitajBilans()
        {
            var danas = DateTime.Today;

            // 1. Unesene kalorije za danas
            UneseneKalorije = _context.Obroks
                .Where(o => o.IdKorisnik == IdTrenutnogKorisnika && o.DatumVrijeme.Date == danas)
                .Sum(o => (int?)o.Kalorije) ?? 0;

            // 2. Potrošene kalorije - sumiraj za svaku aktivnost trajanje * kalorije po minutu iz tipa aktivnosti
            var aktivnosti = _context.Aktivnosts
                .Include(a => a.IdTipaAktivnostiNavigation)
                .Where(a => a.IdKorisnika == IdTrenutnogKorisnika && a.DatumVrijeme.Date == danas)
                .ToList();

            PotroseneKalorije = aktivnosti.Sum(a => a.TrajanjeUMinutama * a.IdTipaAktivnostiNavigation.KalorijePoMinuti);

            // 3. Cilj kalorija za korisnika
            CiljKalorija = _context.Ciljs
                .Where(c => c.IdKorisnik == IdTrenutnogKorisnika)
                .OrderByDescending(c => c.IdKorisnik)
                .Select(c => (int?)c.DnevniLimitKalorija)
                .FirstOrDefault() ?? 0;

            // 4. Preostalo za unos (cilj - uneseno + potrošeno)
            PreostaloZaUnos = CiljKalorija - UneseneKalorije + PotroseneKalorije;

            // 5. Stanje bilansa
            BilansStanje = UneseneKalorije > CiljKalorija ? "Loš" : "Dobar";
        }

    }
}
