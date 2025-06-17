using Fit.Views;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fit
{
    public partial class MainWindow : Window
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public MainWindow()
        {
            InitializeComponent();
            // Na startu učitaj srpski kao podrazumevani jezik
            SetLanguageDictionary("Languages/StringResources.sr.xaml");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        // --- Teme ---
        private void SetLightTheme(object sender, RoutedEventArgs e)
        {
            var theme = Theme.Create(BaseTheme.Light, SwatchHelper.Lookup[MaterialDesignColor.LightGreen], SwatchHelper.Lookup[MaterialDesignColor.Lime]);
            _paletteHelper.SetTheme(theme);

            // Promeni pozadinu glavnog Grid-a za svetlu temu
            MainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5f5f5"));
        }

        private void SetDarkTheme(object sender, RoutedEventArgs e)
        {
            var theme = Theme.Create(BaseTheme.Dark, SwatchHelper.Lookup[MaterialDesignColor.DeepPurple], SwatchHelper.Lookup[MaterialDesignColor.Indigo]);
            _paletteHelper.SetTheme(theme);

            // Promeni pozadinu glavnog Grid-a za tamnu temu
            MainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202020"));
        }

        private void SetColorfulTheme(object sender, RoutedEventArgs e)
        {
            var theme = Theme.Create(BaseTheme.Light, SwatchHelper.Lookup[MaterialDesignColor.Pink], SwatchHelper.Lookup[MaterialDesignColor.DeepPurple]);
            _paletteHelper.SetTheme(theme);

            // Promeni pozadinu glavnog Grid-a za šarenu temu
            MainGrid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE4E1"));
        }

        // --- Jezici ---
        private void SetSerbianLanguage(object sender, RoutedEventArgs e)
        {
            SetLanguageDictionary("Languages/StringResources.sr.xaml");
        }

        private void SetEnglishLanguage(object sender, RoutedEventArgs e)
        {
            SetLanguageDictionary("Languages/StringResources.en.xaml");
        }

        private void SetLanguageDictionary(string dictionaryPath)
        {
            // Ukloni prethodni jezički ResourceDictionary ako postoji
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var langDict = dictionaries.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("StringResources"));
            if (langDict != null)
                dictionaries.Remove(langDict);

            // Dodaj novi
            var newDict = new ResourceDictionary() { Source = new System.Uri(dictionaryPath, System.UriKind.Relative) };
            dictionaries.Add(newDict);
        }
    }
}
