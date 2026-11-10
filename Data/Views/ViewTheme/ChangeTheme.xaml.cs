using System.Windows;
using System.Windows.Input;

namespace SeenExtractor.Data.Views.ViewTheme
{
    /// <summary>
    /// Lógica de interacción para ChangeTheme.xaml
    /// </summary>
    public partial class ChangeTheme : Window
    {
        public ChangeTheme()
        {
            InitializeComponent();
            ViewThemes.Visibility = Visibility.Visible;
        }


        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
