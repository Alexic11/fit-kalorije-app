using Fit.Models;
using Fit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fit.ViewModels;
using System.Windows;

namespace Fit.Views
{
    /// <summary>
    /// Interaction logic for UnosCiljaWindow.xaml
    /// </summary>
    public partial class UnosCiljaWindow : Window
    {
        public UnosCiljaWindow(int idKorisnik)
        {
            InitializeComponent();
            DataContext = new UnosCiljaViewModel(idKorisnik);
        }
    }
}
