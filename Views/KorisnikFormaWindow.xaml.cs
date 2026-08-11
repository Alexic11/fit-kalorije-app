using Fit.Models;
using Fit.Security;
using System.Windows;

namespace Fit.Views
{
    public partial class KorisnikFormaWindow : Window
    {
        public KorisnikFormaWindow(Korisnik korisnik)
        {
            InitializeComponent();
            DataContext = korisnik;
        }

        private void Sacuvaj_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(
                    LozinkaBox.Password))
            {
                if (DataContext is Korisnik korisnik)
                {
                    korisnik.Lozinka =
                        PasswordHasher.HashPassword(
                            LozinkaBox.Password);
                }
            }

            DialogResult = true;
        }
    }
}