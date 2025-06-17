using Fit.Models;
using System.Text;
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

        private void Sacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LozinkaBox.Password))
            {
                if (DataContext is Korisnik korisnik)
                {
                    korisnik.Lozinka = HashPassword(LozinkaBox.Password);
                }
            }

            DialogResult = true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }


    }
}
