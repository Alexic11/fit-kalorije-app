using Fit.Models;
using Fit.Security;
using Fit.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fit.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Korisnik> Korisniks
        {
            get;
            set;
        }

        private Korisnik _selectedKorisnik =
            new();

        public Korisnik SelectedKorisnik
        {
            get => _selectedKorisnik;
            set
            {
                _selectedKorisnik = value;

                OnPropertyChanged(
                    nameof(SelectedKorisnik));
            }
        }

        public ICommand LoginCommand { get; set; }

        public FitAppContext context { get; set; }

        public LoginViewModel()
        {
            context = new FitAppContext();

            var korisniks =
                context.Korisniks
                    .Include(k => k.IdRoleNavigation)
                    .ToList();

            Korisniks =
                new ObservableCollection<Korisnik>(
                    korisniks);

            SelectedKorisnik =
                new Korisnik();

            LoginCommand =
                new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            string enteredPassword =
                passwordBox.Password;

            if (string.IsNullOrWhiteSpace(
                    SelectedKorisnik.KorisnickoIme) ||
                string.IsNullOrWhiteSpace(
                    enteredPassword))
            {
                ShowMessage(
                    "FillAllFieldsMessage");

                return;
            }

            var dbKorisnik =
                Korisniks.FirstOrDefault(
                    k => string.Equals(
                        k.KorisnickoIme,
                        SelectedKorisnik.KorisnickoIme,
                        StringComparison.OrdinalIgnoreCase));

            if (dbKorisnik == null)
            {
                ShowMessage(
                    "IncorrectUsername");

                return;
            }

            bool validPassword =
                PasswordHasher.VerifyPassword(
                    enteredPassword,
                    dbKorisnik.Lozinka);

            if (!validPassword)
            {
                ShowMessage(
                    "InvalidPasswordMessage");

                return;
            }

            /*
             * Ako korisnik još ima stari SHA-256 hash,
             * poslije uspješnog logina ga automatski
             * migriramo na PBKDF2.
             */
            if (PasswordHasher.NeedsRehash(
                    dbKorisnik.Lozinka))
            {
                dbKorisnik.Lozinka =
                    PasswordHasher.HashPassword(
                        enteredPassword);

                context.SaveChanges();
            }

            CurrentUser.Role =
                dbKorisnik.IdRoleNavigation.Naziv;

            if (dbKorisnik.IdRole == 1)
            {
                var adminWindow =
                    new AdminDashboardWindow(
                        dbKorisnik.IdKorisnik);

                adminWindow.Show();
            }
            else
            {
                var userWindow =
                    new DashboardWindow(
                        dbKorisnik.IdKorisnik);

                userWindow.Show();
            }

            foreach (
                Window window
                in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void ShowMessage(
            string resourceKey)
        {
            string message =
                Application.Current.Resources.Contains(
                    resourceKey)
                    ? (string)
                        Application.Current.Resources[
                            resourceKey]
                    : "Message not found";

            string title =
                Application.Current.Resources.Contains(
                    "NotificationTitle")
                    ? (string)
                        Application.Current.Resources[
                            "NotificationTitle"]
                    : "Notification";

            MessageBox.Show(
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler?
            PropertyChanged;

        protected void OnPropertyChanged(
            string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
        }
    }
}