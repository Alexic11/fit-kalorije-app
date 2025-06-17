using System.Windows;
using Fit.Views;

namespace Fit.Views
{
    public partial class DashboardWindow : Window
    {
        private int _idKorisnik;

        public DashboardWindow(int idKorisnik)
        {
            InitializeComponent();
            _idKorisnik = idKorisnik;

            // Dodavanje handlera za dugmad
            btnUnosCilja.Click += BtnUnosCilja_Click;
            btnUnosHrane.Click += BtnUnosHrane_Click;
            btnUnosAktivnosti.Click += BtnUnosAktivnosti_Click;
            btnPrikazUnosa.Click += BtnPrikazUnosa_Click;
            btnBilans.Click += BtnBilans_Click;
            btnPregledAktivnosti.Click += BtnPregledAktivnosti_Click;

        }

        private void BtnUnosCilja_Click(object sender, RoutedEventArgs e)
        {
            var unosCiljaWindow = new UnosCiljaWindow(_idKorisnik);
            unosCiljaWindow.ShowDialog();
        }

        private void BtnUnosHrane_Click(object sender, RoutedEventArgs e)
        {
            var unosHraneWindow = new UnosHraneWindow(_idKorisnik);
            unosHraneWindow.ShowDialog();
        }

        private void BtnUnosAktivnosti_Click(object sender, RoutedEventArgs e)
        {
            var unosAktivnostiWindow = new UnosAktivnostiWindow(_idKorisnik);
            unosAktivnostiWindow.ShowDialog();
        }

        private void BtnPrikazUnosa_Click(object sender, RoutedEventArgs e)
        {
            var pregledUnosaWindow = new PrikazUnosaWindow(_idKorisnik);
            pregledUnosaWindow.ShowDialog();
        }

        private void BtnBilans_Click(object sender, RoutedEventArgs e)
        {
            var bilansWindow = new BilansWindow(_idKorisnik);
            bilansWindow.ShowDialog();
        }

        private void BtnPregledAktivnosti_Click(object sender, RoutedEventArgs e)
        {
            var pregledAktivnostiWindow = new PregledAktivnostiWindow(_idKorisnik);
            pregledAktivnostiWindow.ShowDialog();
        }
    }
}
