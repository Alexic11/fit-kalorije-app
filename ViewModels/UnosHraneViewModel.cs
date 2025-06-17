using Fit.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    public class UnosHraneViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly FitAppContext _context = new();

        public int IdTrenutnogKorisnika { get; set; }

        public ObservableCollection<TipObroka> TipoviObroka { get; set; }

        private TipObroka? _selektovaniTipObroka;
        public TipObroka? SelektovaniTipObroka
        {
            get => _selektovaniTipObroka;
            set
            {
                _selektovaniTipObroka = value;
                OnPropertyChanged(nameof(SelektovaniTipObroka));
            }
        }

        private string _naziv = string.Empty;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }

        private string _kalorije = string.Empty;
        public string Kalorije
        {
            get => _kalorije;
            set
            {
                _kalorije = value;
                OnPropertyChanged(nameof(Kalorije));
            }
        }

        private string _proteini = string.Empty;
        public string Proteini
        {
            get => _proteini;
            set
            {
                _proteini = value;
                OnPropertyChanged(nameof(Proteini));
            }
        }

        private string _ugljeniHidrati = string.Empty;
        public string UgljeniHidrati
        {
            get => _ugljeniHidrati;
            set
            {
                _ugljeniHidrati = value;
                OnPropertyChanged(nameof(UgljeniHidrati));
            }
        }

        private string _masti = string.Empty;
        public string Masti
        {
            get => _masti;
            set
            {
                _masti = value;
                OnPropertyChanged(nameof(Masti));
            }
        }

        private string _masa = string.Empty;
        public string Masa
        {
            get => _masa;
            set
            {
                _masa = value;
                OnPropertyChanged(nameof(Masa));
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

        private string _vrijemeUnosa = string.Empty;
        public string VrijemeUnosa
        {
            get => _vrijemeUnosa;
            set
            {
                _vrijemeUnosa = value;
                OnPropertyChanged(nameof(VrijemeUnosa));
            }
        }

        public ICommand DodajHranuCommand { get; }

        public UnosHraneViewModel(int korisnikId)
        {
            IdTrenutnogKorisnika = korisnikId;

            TipoviObroka = new ObservableCollection<TipObroka>(_context.TipObrokas.ToList());
            DodajHranuCommand = new RelayCommand(DodajHranu);

            DatumUnosa = DateTime.Today;
            VrijemeUnosa = DateTime.Now.ToString("HH:mm");

            if (TipoviObroka.Count > 0)
                SelektovaniTipObroka = TipoviObroka[0];
        }

        private void DodajHranu()
        {
            if (SelektovaniTipObroka == null ||
                string.IsNullOrWhiteSpace(Naziv) ||
                !int.TryParse(Kalorije, out int kalorije) ||
                !int.TryParse(Proteini, out int proteini) ||
                !int.TryParse(UgljeniHidrati, out int ugljeniHidrati) ||
                !int.TryParse(Masti, out int masti) ||
                !int.TryParse(Masa, out int masa) ||
                !DatumUnosa.HasValue)
            {
                MessageBox.Show(GetLocalizedString("PorukaPopuniSvePodatke"), GetLocalizedString("NaslovGreska"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TimeSpan.TryParse(VrijemeUnosa, out TimeSpan vrijeme))
            {
                MessageBox.Show(GetLocalizedString("PorukaNeispravnoVrijeme"), GetLocalizedString("NaslovGreska"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime datumVrijemeUnosa = DatumUnosa.Value.Date + vrijeme;

            var noviUnos = new Obrok
            {
                Naziv = Naziv,
                Kalorije = kalorije,
                Proteini = proteini,
                Ugljenihidrati = ugljeniHidrati,
                Masti = masti,
                Masa = masa,
                IdTipObroka = SelektovaniTipObroka.IdTipObroka,
                IdKorisnik = IdTrenutnogKorisnika,
                DatumVrijeme = datumVrijemeUnosa
            };

            _context.Obroks.Add(noviUnos);
            _context.SaveChanges();

            MessageBox.Show(GetLocalizedString("PorukaObrokSacuvan"), GetLocalizedString("NaslovObavestenje"), MessageBoxButton.OK, MessageBoxImage.Information);

            // Reset forme na default vrednosti
            Naziv = string.Empty;
            Kalorije = string.Empty;
            Proteini = string.Empty;
            UgljeniHidrati = string.Empty;
            Masti = string.Empty;
            Masa = string.Empty;

            if (TipoviObroka.Count > 0)
                SelektovaniTipObroka = TipoviObroka[0];
            else
                SelektovaniTipObroka = null;

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
