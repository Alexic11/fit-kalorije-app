using Fit.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Fit.ViewModels
{
    public class PrikazUnosaViewModel : INotifyPropertyChanged
    {
        private readonly FitAppContext _context = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int IdTrenutnogKorisnika { get; set; }

        private ObservableCollection<Obrok> _sviObroci;
        public ObservableCollection<Obrok> SviObroci
        {
            get => _sviObroci;
            set
            {
                _sviObroci = value;
                OnPropertyChanged(nameof(SviObroci));
            }
        }

        private ObservableCollection<Obrok> _filteredObroci;
        public ObservableCollection<Obrok> FilteredObroci
        {
            get => _filteredObroci;
            set
            {
                _filteredObroci = value;
                OnPropertyChanged(nameof(FilteredObroci));
            }
        }

        private string _filterNaziv;
        public string FilterNaziv
        {
            get => _filterNaziv;
            set
            {
                _filterNaziv = value;
                OnPropertyChanged(nameof(FilterNaziv));
                PrimijeniFilter();
            }
        }

        private string _filterTip;
        public string FilterTip
        {
            get => _filterTip;
            set
            {
                _filterTip = value;
                OnPropertyChanged(nameof(FilterTip));
                PrimijeniFilter();
            }
        }

        private Obrok _selektovaniObrok;
        public Obrok SelektovaniObrok
        {
            get => _selektovaniObrok;
            set
            {
                if (_selektovaniObrok != value)
                {
                    _selektovaniObrok = value;
                    OnPropertyChanged(nameof(SelektovaniObrok));
                    (ObrisiCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand ObrisiCommand { get; }

        public PrikazUnosaViewModel(int korisnikId)
        {
            IdTrenutnogKorisnika = korisnikId;

            var obroci = _context.Obroks
                .Include(o => o.IdTipObrokaNavigation)
                .Where(o => o.IdKorisnik == IdTrenutnogKorisnika)
                .OrderByDescending(o => o.DatumVrijeme)
                .ToList();

            SviObroci = new ObservableCollection<Obrok>(obroci);
            FilteredObroci = new ObservableCollection<Obrok>(SviObroci);

            ObrisiCommand = new RelayCommand(ObrisiObrok, () => SelektovaniObrok != null);
        }

        private void PrimijeniFilter()
        {
            var filtrirani = SviObroci.Where(o =>
                (string.IsNullOrWhiteSpace(FilterNaziv) || o.Naziv.ToLower().Contains(FilterNaziv.ToLower())) &&
                (string.IsNullOrWhiteSpace(FilterTip) || (o.IdTipObrokaNavigation != null && o.IdTipObrokaNavigation.Naziv.ToLower().Contains(FilterTip.ToLower())))
            ).OrderByDescending(o => o.DatumVrijeme);

            FilteredObroci.Clear();
            foreach (var obrok in filtrirani)
                FilteredObroci.Add(obrok);
        }

        private void ObrisiObrok()
        {
            if (SelektovaniObrok == null) return;

            string message = Application.Current.Resources.Contains("ConfirmDeleteMeal")
                ? (string)Application.Current.Resources["ConfirmDeleteMeal"]
                : "Da li ste sigurni da želite obrisati ovaj obrok?";

            string title = Application.Current.Resources.Contains("ConfirmationTitle2")
                ? (string)Application.Current.Resources["ConfirmationTitle2"]
                : "Potvrda";

            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _context.Obroks.Remove(SelektovaniObrok);
                _context.SaveChanges();

                SviObroci.Remove(SelektovaniObrok);
                FilteredObroci.Remove(SelektovaniObrok);
                SelektovaniObrok = null;
            }
        }
    }
}
