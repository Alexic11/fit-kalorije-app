using Fit.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fit.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            if (DataContext is RegistracijaViewModel vm)
            {
                vm.RegistrationSuccessful += Vm_RegistrationSuccessful;
            }
        }

        private void Vm_RegistrationSuccessful()
        {
            this.Close();
        }
    }

}
