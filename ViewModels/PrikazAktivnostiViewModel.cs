using Fit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    public class PrikazAktivnostiViewModel : INotifyPropertyChanged
    {
        private readonly FitAppContext _context = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string naziv) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naziv));

        public int IdTrenutnogKorisnika { get; set; }

        private ObservableCollection<Aktivnost> _sveAktivnosti;
        public ObservableCollection<Aktivnost> SveAktivnosti
        {
            get => _sveAktivnosti;
            set
            {
                _sveAktivnosti = value;
                OnPropertyChanged(nameof(SveAktivnosti));
            }
        }

        private ObservableCollection<Aktivnost> _filteredAktivnosti;
        public ObservableCollection<Aktivnost> FilteredAktivnosti
        {
            get => _filteredAktivnosti;
            set
            {
                _filteredAktivnosti = value;
                OnPropertyChanged(nameof(FilteredAktivnosti));
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

        private Aktivnost _selektovanaAktivnost;
        public Aktivnost SelektovanaAktivnost
        {
            get => _selektovanaAktivnost;
            set
            {
                _selektovanaAktivnost = value;
                OnPropertyChanged(nameof(SelektovanaAktivnost));
                (ObrisiCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand ObrisiCommand { get; }

        public PrikazAktivnostiViewModel(int korisnikId)
        {
            IdTrenutnogKorisnika = korisnikId;

            var aktivnosti = _context.Aktivnosts
                .Include(a => a.IdTipaAktivnostiNavigation)
                .Where(a => a.IdKorisnika == IdTrenutnogKorisnika)
                .OrderByDescending(a => a.DatumVrijeme)
                .ToList();

            SveAktivnosti = new ObservableCollection<Aktivnost>(aktivnosti);
            FilteredAktivnosti = new ObservableCollection<Aktivnost>(SveAktivnosti);

            ObrisiCommand = new RelayCommand(ObrisiAktivnost, () => SelektovanaAktivnost != null);
        }

        private void PrimijeniFilter()
        {
            var filtrirane = SveAktivnosti.Where(a =>
                (string.IsNullOrWhiteSpace(FilterNaziv) || a.Naziv.ToLower().Contains(FilterNaziv.ToLower())) &&
                (string.IsNullOrWhiteSpace(FilterTip) || (a.IdTipaAktivnostiNavigation != null && a.IdTipaAktivnostiNavigation.Naziv.ToLower().Contains(FilterTip.ToLower())))
            ).OrderByDescending(a => a.DatumVrijeme);

            FilteredAktivnosti.Clear();
            foreach (var aktivnost in filtrirane)
                FilteredAktivnosti.Add(aktivnost);
        }

        private void ObrisiAktivnost()
        {
            if (SelektovanaAktivnost == null) return;

            // Poruka iz resursnog fajla
            string message = Application.Current.Resources.Contains("ConfirmDeleteActivity")
                ? (string)Application.Current.Resources["ConfirmDeleteActivity"]
                : "Are you sure you want to delete this activity?";

            string title = Application.Current.Resources.Contains("ConfirmationTitle")
                ? (string)Application.Current.Resources["ConfirmationTitle"]
                : "Confirmation";

            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _context.Aktivnosts.Remove(SelektovanaAktivnost);
                _context.SaveChanges();

                SveAktivnosti.Remove(SelektovanaAktivnost);
                FilteredAktivnosti.Remove(SelektovanaAktivnost);
                SelektovanaAktivnost = null;
            }
        }
    }
}
