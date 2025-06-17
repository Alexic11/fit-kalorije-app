using Fit.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    public class UnosCiljaViewModel : INotifyPropertyChanged
    {
        private int _dnevniLimitKalorija;
        public int DnevniLimitKalorija
        {
            get => _dnevniLimitKalorija;
            set
            {
                if (_dnevniLimitKalorija != value)
                {
                    _dnevniLimitKalorija = value;
                    OnPropertyChanged(nameof(DnevniLimitKalorija));
                }
            }
        }

        public ICommand SacuvajCiljCommand { get; }

        public FitAppContext Context { get; set; }

        public int IdTrenutnogKorisnika { get; set; }

        private Cilj? _postojeciCilj;

        public UnosCiljaViewModel(int korisnikId)
        {
            IdTrenutnogKorisnika = korisnikId;
            Context = new FitAppContext();
            SacuvajCiljCommand = new RelayCommand(SacuvajCilj);

            // Učitaj postojeći cilj ako postoji
            _postojeciCilj = Context.Ciljs.FirstOrDefault(c => c.IdKorisnik == IdTrenutnogKorisnika);
            if (_postojeciCilj != null)
            {
                DnevniLimitKalorija = _postojeciCilj.DnevniLimitKalorija;
            }
        }

        private void SacuvajCilj()
        {
            if (DnevniLimitKalorija <= 0)
            {
                MessageBox.Show(GetLocalizedString("PorukaUnesiteValidanBroj"), GetLocalizedString("NaslovGreska"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_postojeciCilj != null)
            {
                // Ažuriraj postojeći cilj
                _postojeciCilj.DnevniLimitKalorija = DnevniLimitKalorija;
                Context.SaveChanges();
                MessageBox.Show(GetLocalizedString("PorukaCiljAzuriran"), GetLocalizedString("NaslovObavestenje"), MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Kreiraj novi cilj
                var noviCilj = new Cilj
                {
                    DnevniLimitKalorija = DnevniLimitKalorija,
                    IdKorisnik = IdTrenutnogKorisnika
                };

                Context.Ciljs.Add(noviCilj);
                Context.SaveChanges();
                MessageBox.Show(GetLocalizedString("PorukaCiljSacuvan"), GetLocalizedString("NaslovObavestenje"), MessageBoxButton.OK, MessageBoxImage.Information);

                _postojeciCilj = noviCilj;
            }
        }

        private string GetLocalizedString(string key)
        {
            if (Application.Current.Resources.Contains(key))
                return Application.Current.Resources[key] as string ?? string.Empty;
            return string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
