using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static SeenExtractor.Data.Models.Extenciones.Theme.ApplyTheme;
using static SeenExtractor.Data.Models.OSVersion.GetOSInfo;
using static SeenExtractor.Data.Models.Themes.ListThemes;

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
            if (cmbThemes.SelectedIndex != 0 && OSBuild <= 22000)
                Theme(App.Current.MainWindow, cmbThemes.SelectedValue, GetColor());
            else
                Theme(App.Current.MainWindow, cmbThemes.SelectedValue);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PickColor_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (txtO is null) //No se por que pero debo usarlo o se muere al abrir la pantalla con SliderO en 255
                return;

            var t = (byte)SliderO.Value;
            var r = (byte)SliderR.Value;
            var g = (byte)SliderG.Value;
            var b = (byte)SliderB.Value;

            txtO.Text = t.ToString();
            txtR.Text = r.ToString();
            txtG.Text = g.ToString();
            txtB.Text = b.ToString();

            Color selectedColor = Color.FromArgb(t, r, g, b);
            ColorPreviewBox.Background = new SolidColorBrush(selectedColor);

            txtHex.Text = selectedColor.ToString();
        }

        private void cmbThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbThemes.SelectedIndex != 0 && OSBuild <= 22000)
            {
                PickColor.IsEnabled = true;
                PickColor.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            else
            {
                PickColor.IsEnabled = false;
                PickColor.Background = new SolidColorBrush(Color.FromArgb(20, 31, 31, 31));
            }
        }

        private uint GetColor()
        {
            var t = (byte)SliderO.Value;
            var r = (byte)SliderR.Value;
            var g = (byte)SliderG.Value;
            var b = (byte)SliderB.Value;

            return ((uint)t << 24) | ((uint)b << 16) | ((uint)g << 8) | r; //Junto los valores con operadores de bytes, ademas de convertirlo al formato que pide user32.dll (Invertir R y B)
        }

    }
}
