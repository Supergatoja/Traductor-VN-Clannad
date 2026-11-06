using System.Windows.Controls;
using static SeenExtractor.Data.Models.Themes.ListThemes;
using static SeenExtractor.Data.Models.OSVersion.GetOSInfo;
using static SeenExtractor.Data.Models.Extenciones.Theme.ApplyTheme;
using System.Windows;

namespace SeenExtractor.Data.Views.ViewTheme
{
    /// <summary>
    /// Lógica de interacción para Themes.xaml
    /// </summary>
    public partial class Themes : UserControl
    {
        public Themes()
        {
            InitializeComponent();
            CheckVersionOS();
        }

        private void CheckVersionOS()
        {
            try
            {
                if (OSBuild >= 22000)
                    cmbThemes.ItemsSource = ThemesW11;
                else
                    if (OSMajor >= 10)
                        cmbThemes.ItemsSource = ThemesW10;
                    else
                        if (OSBuild >= 7600 && OSBuild <= 9600)
                            cmbThemes.ItemsSource = ThemesW10;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void AplicarTheme_Click(object sender, RoutedEventArgs e)
        {
            Theme(App.Current.MainWindow, cmbThemes.SelectedValue);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
