using Fit.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Fit.ViewModels
{
    class RegistracijaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Korisnik> Korisniks { get; set; }
        public Korisnik NewKorisnik { get; set; }
        public ICommand RegisterCommand { get; set; }
        public FitAppContext context { get; set; }

        public event Action RegistrationSuccessful;

        private string _lozinka;
        public string Lozinka
        {
            get => _lozinka;
            set
            {
                if (_lozinka != value)
                {
                    _lozinka = value;
                    OnPropertyChanged(nameof(Lozinka));
                }
            }
        }

        public RegistracijaViewModel()
        {
            context = new FitAppContext();
            var korisniks = context.Korisniks.Include(k => k.IdRoleNavigation).ToList();
            Korisniks = new ObservableCollection<Korisnik>(korisniks);

            NewKorisnik = new Korisnik();
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(NewKorisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(NewKorisnik.Lozinka))
            {
                string fillMessage = Application.Current.Resources.Contains("FillAllFieldsMessage2")
                    ? (string)Application.Current.Resources["FillAllFieldsMessage2"]
                    : "Molimo unesite korisničko ime i lozinku.";

                string notifTitle = Application.Current.Resources.Contains("NotificationTitle2")
                    ? (string)Application.Current.Resources["NotificationTitle2"]
                    : "Obavještenje";

                MessageBox.Show(fillMessage, notifTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isUsernameExists = false;
            foreach (var korisnik in Korisniks)
            {
                if (korisnik.KorisnickoIme.Equals(NewKorisnik.KorisnickoIme, StringComparison.OrdinalIgnoreCase))
                {
                    isUsernameExists = true;
                    break;
                }
            }

            if (isUsernameExists)
            {
                string existsMessage = Application.Current.Resources.Contains("UsernameExistsMessage")
                    ? (string)Application.Current.Resources["UsernameExistsMessage"]
                    : "Korisničko ime već postoji.";

                string notifTitle = Application.Current.Resources.Contains("NotificationTitle2")
                    ? (string)Application.Current.Resources["NotificationTitle2"]
                    : "Obavještenje";

                MessageBox.Show(existsMessage, notifTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashedPass = HashPassword(NewKorisnik.Lozinka);
            NewKorisnik.Lozinka = hashedPass;
            NewKorisnik.IdRole = 2; // pretpostavka: 2 = običan korisnik
            context.Korisniks.Add(NewKorisnik);
            context.SaveChanges();

            NewKorisnik = new Korisnik();
            OnPropertyChanged(nameof(NewKorisnik));

            string successMessage = Application.Current.Resources.Contains("SuccessfulRegistration")
                ? (string)Application.Current.Resources["SuccessfulRegistration"]
                : "Registracija uspješna!";

            string title = Application.Current.Resources.Contains("NotificationTitle2")
                ? (string)Application.Current.Resources["NotificationTitle2"]
                : "Obavještenje";

            MessageBox.Show(successMessage, title, MessageBoxButton.OK, MessageBoxImage.Information);

            RegistrationSuccessful?.Invoke();
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
