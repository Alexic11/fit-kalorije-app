using Fit.Models;
using Fit.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fit.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Korisnik> Korisniks { get; set; }
        private Korisnik _selectedKorisnik;
        public Korisnik SelectedKorisnik
        {
            get => _selectedKorisnik;
            set
            {
                _selectedKorisnik = value;
                OnPropertyChanged(nameof(SelectedKorisnik));
            }
        }
        public ICommand LoginCommand { get; set; }
        public FitAppContext context { get; set; }

        public LoginViewModel()
        {
            context = new FitAppContext();
            var korisniks = context.Korisniks.Include(k => k.IdRoleNavigation).ToList();
            Korisniks = new ObservableCollection<Korisnik>(korisniks);

            SelectedKorisnik = new Korisnik();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string enteredPassword = passwordBox.Password;

                if (!string.IsNullOrWhiteSpace(SelectedKorisnik.KorisnickoIme) && !string.IsNullOrWhiteSpace(enteredPassword))
                {
                    var dbKorisnik = Korisniks.FirstOrDefault(k => k.KorisnickoIme == SelectedKorisnik.KorisnickoIme);
                    if (dbKorisnik != null)
                    {
                        string hashedPass = HashPassword(enteredPassword);
                        if (hashedPass == dbKorisnik.Lozinka)
                        {
                            CurrentUser.Role = dbKorisnik.IdRoleNavigation.Naziv;

                            if (dbKorisnik.IdRole == 1)
                            {
                                var adminWindow = new AdminDashboardWindow(dbKorisnik.IdKorisnik);
                                adminWindow.Show();
                            }
                            else
                            {
                                var userWindow = new DashboardWindow(dbKorisnik.IdKorisnik);
                                userWindow.Show();
                            }

                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window is MainWindow)
                                {
                                    window.Close();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ShowMessage("InvalidPasswordMessage");
                        }
                    }
                    else
                    {
                        ShowMessage("IncorrectUsername");
                    }
                }
                else
                {
                    ShowMessage("FillAllFieldsMessage");
                }
            }
        }

        private void ShowMessage(string resourceKey)
        {
            string message = Application.Current.Resources.Contains(resourceKey)
                ? (string)Application.Current.Resources[resourceKey]
                : "Message not found";
            string title = Application.Current.Resources.Contains("NotificationTitle")
                ? (string)Application.Current.Resources["NotificationTitle"]
                : "Notification";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
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
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
