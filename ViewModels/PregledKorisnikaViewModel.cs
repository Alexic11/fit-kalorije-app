using Fit.Models;
using Fit.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    public class PregledKorisnikaViewModel : INotifyPropertyChanged
    {
        private readonly FitAppContext _context;

        private ObservableCollection<Korisnik> _korisnici;
        public ObservableCollection<Korisnik> Korisnici
        {
            get => _korisnici;
            set
            {
                _korisnici = value;
                OnPropertyChanged(nameof(Korisnici));
            }
        }

        public ICommand DodajCommand { get; set; }
        public ICommand IzmijeniCommand { get; set; }
        public ICommand ObrisiCommand { get; set; }

        private Korisnik _selektovaniKorisnik;
        public Korisnik SelektovaniKorisnik
        {
            get => _selektovaniKorisnik;
            set
            {
                _selektovaniKorisnik = value;
                OnPropertyChanged(nameof(SelektovaniKorisnik));

                (IzmijeniCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ObrisiCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public PregledKorisnikaViewModel()
        {
            _context = new FitAppContext();
            LoadKorisnici();

            DodajCommand = new RelayCommand(DodajKorisnika);
            IzmijeniCommand = new RelayCommand(IzmijeniKorisnika, () => SelektovaniKorisnik != null);
            ObrisiCommand = new RelayCommand(ObrisiKorisnika, () => SelektovaniKorisnik != null);
        }

        private void DodajKorisnika()
        {
            var novi = new Korisnik();
            var forma = new KorisnikFormaWindow(novi);
            if (forma.ShowDialog() == true)
            {
                if (!string.IsNullOrWhiteSpace(novi.Lozinka))
                {
                    novi.Lozinka = HashPassword(novi.Lozinka);
                }

                _context.Korisniks.Add(novi);
                _context.SaveChanges();
                LoadKorisnici();
            }
        }

        private void IzmijeniKorisnika()
        {
            if (SelektovaniKorisnik == null) return;

            var kopija = new Korisnik
            {
                IdKorisnik = SelektovaniKorisnik.IdKorisnik,
                Ime = SelektovaniKorisnik.Ime,
                Prezime = SelektovaniKorisnik.Prezime,
                KorisnickoIme = SelektovaniKorisnik.KorisnickoIme,
                Lozinka = SelektovaniKorisnik.Lozinka,
                IdRole = SelektovaniKorisnik.IdRole
            };

            var forma = new KorisnikFormaWindow(kopija);
            if (forma.ShowDialog() == true)
            {
                var original = _context.Korisniks.Find(kopija.IdKorisnik);
                if (original != null)
                {
                    original.Ime = kopija.Ime;
                    original.Prezime = kopija.Prezime;
                    original.KorisnickoIme = kopija.KorisnickoIme;
                    original.Lozinka = kopija.Lozinka;
                    original.IdRole = kopija.IdRole;

                    _context.SaveChanges();
                    LoadKorisnici();
                }
            }
        }

        private void ObrisiKorisnika()
        {
            if (SelektovaniKorisnik == null) return;

            string message = GetResourceString("ConfirmDeleteUser", "Da li ste sigurni da želite obrisati korisnika?");
            string title = GetResourceString("DeleteTitle", "Brisanje");

            var potvrda = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (potvrda == MessageBoxResult.Yes)
            {
                _context.Korisniks.Remove(SelektovaniKorisnik);
                _context.SaveChanges();
                LoadKorisnici();
            }
        }

        private string GetResourceString(string key, string defaultValue)
        {
            if (Application.Current.Resources.Contains(key))
                return (string)Application.Current.Resources[key];
            return defaultValue;
        }

        private void LoadKorisnici()
        {
            var lista = _context.Korisniks.Include(k => k.IdRoleNavigation).ToList();
            Korisnici = new ObservableCollection<Korisnik>(lista);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hash);
            }
        }
    }
}
