using System.Windows;

namespace Fit.Views
{
    public partial class AdminDashboardWindow : Window
    {
        public int AdminId { get; set; }

        public AdminDashboardWindow(int adminId)
        {
            InitializeComponent();
            AdminId = adminId;
            // Možeš da koristiš AdminId ako bude trebalo kasnije za logiku

            btnPregledKorisnika.Click += BtnPregledKorisnika_Click;

        }

        private void BtnPregledKorisnika_Click(object sender, RoutedEventArgs e)
        {
            var prozor = new PregledKorisnikaView();
            prozor.ShowDialog();
        }

    }
}
