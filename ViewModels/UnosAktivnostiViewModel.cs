using Fit.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    public class UnosAktivnostiViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string naziv) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naziv));

        private readonly FitAppContext _context = new();

        public int IdTrenutnogKorisnika { get; set; }

        public ObservableCollection<TipAktivnosti> TipoviAktivnosti { get; set; }

        private TipAktivnosti _selektovaniTip;
        public TipAktivnosti SelektovaniTip
        {
            get => _selektovaniTip;
            set
            {
                _selektovaniTip = value;
                OnPropertyChanged(nameof(SelektovaniTip));
            }
        }

        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }

        private int _trajanjeUMinutama;
        public int TrajanjeUMinutama
        {
            get => _trajanjeUMinutama;
            set
            {
                _trajanjeUMinutama = value;
                OnPropertyChanged(nameof(TrajanjeUMinutama));
            }
        }

        private DateTime? _datumUnosa;
        public DateTime? DatumUnosa
        {
            get => _datumUnosa;
            set
            {
                _datumUnosa = value;
                OnPropertyChanged(nameof(DatumUnosa));
            }
        }

        private string _vrijemeUnosa;
        public string VrijemeUnosa
        {
            get => _vrijemeUnosa;
            set
            {
                _vrijemeUnosa = value;
                OnPropertyChanged(nameof(VrijemeUnosa));
            }
        }

        public ICommand SacuvajAktivnostCommand { get; }

        public UnosAktivnostiViewModel(int korisnikId)
        {
            IdTrenutnogKorisnika = korisnikId;
            TipoviAktivnosti = new ObservableCollection<TipAktivnosti>(_context.TipAktivnostis.ToList());
            SacuvajAktivnostCommand = new RelayCommand(SacuvajAktivnost);

            // Default vrednosti za unos
            Naziv = string.Empty;
            DatumUnosa = DateTime.Today;
            VrijemeUnosa = DateTime.Now.ToString("HH:mm");
        }

        private void SacuvajAktivnost()
        {
            // Validacija obaveznih polja
            if (SelektovaniTip == null || TrajanjeUMinutama <= 0 || string.IsNullOrWhiteSpace(Naziv) || !DatumUnosa.HasValue)
            {
                MessageBox.Show(GetLocalizedString("PorukaPopuniSve"));
                return;
            }

            // Validacija formata vremena (HH:mm)
            if (!TimeSpan.TryParse(VrijemeUnosa, out TimeSpan vrijeme))
            {
                MessageBox.Show(GetLocalizedString("PorukaNeispravnoVrijeme"));
                return;
            }

            // Kombinovanje datuma i vremena u DateTime
            DateTime datumVrijeme = DatumUnosa.Value.Date + vrijeme;

            var novaAktivnost = new Aktivnost
            {
                IdTipaAktivnosti = SelektovaniTip.IdTipAktivnosti,
                IdKorisnika = IdTrenutnogKorisnika,
                TrajanjeUMinutama = TrajanjeUMinutama,
                Naziv = Naziv,
                DatumVrijeme = datumVrijeme
            };

            _context.Aktivnosts.Add(novaAktivnost);
            _context.SaveChanges();

            // Izračun kalorija sagorelih
            int kalorije = TrajanjeUMinutama * SelektovaniTip.KalorijePoMinuti;

            string poruka = GetLocalizedString("PorukaAktivnostSacuvana");
            MessageBox.Show(string.Format(poruka, kalorije));

            // Reset forme nakon snimanja
            SelektovaniTip = null;
            TrajanjeUMinutama = 0;
            Naziv = string.Empty;
            DatumUnosa = DateTime.Today;
            VrijemeUnosa = DateTime.Now.ToString("HH:mm");
        }

        private string GetLocalizedString(string key)
        {
            if (Application.Current.Resources.Contains(key))
                return Application.Current.Resources[key] as string ?? string.Empty;
            return string.Empty;
        }
    }
}
