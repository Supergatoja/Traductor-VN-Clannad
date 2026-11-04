using SeenExtractor.Data.Models.Dialogos.Search;
using SeenExtractor.Data.Models.Lectura;
using System.Windows;
using System.Windows.Input;

namespace SeenExtractor.Data.Views.Search
{
    /// <summary>
    /// Lógica de interacción para Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public string? Prueba { get; set; }
        private readonly ActionsSearch Actions = new();
        private readonly List<LinesSeen> _NewLineSeens;
        private readonly List<LinesSeen> _LineasSeens;
        private readonly bool Mode;
        //private List<LinesSeen> newLinesSeens;
        public SearchDialogModel _Dialogos;

        public Search(List<LinesSeen> linesSeens, List<LinesSeen> newLinesSeens)
        {
            InitializeComponent();
            _NewLineSeens = newLinesSeens;
            _LineasSeens = linesSeens;
            this.Title = "Buscar";
            Mode = false;
            ViewBuscar.Visibility = Visibility.Visible;
            Buscar.DataContext = new Dictionary<string, object>() { { "Window", this }, { "Dialogos Originales", linesSeens }, { "Dialogos Nuevos", newLinesSeens } };
            ConectarEventosBotones();
        }

        public Search(List<LinesSeen> newLinesSeens)
        {
            InitializeComponent();
            _NewLineSeens = newLinesSeens;
            this.Title = "Remplazar";
            Mode = true;
            ViewRemplazar.Visibility = Visibility.Visible;
            Remplazar.DataContext = new Dictionary<string, object>() { { "Window", this }, { "Dialogos", newLinesSeens } };
            ConectarEventosBotones();
        }

        
        private void ConectarEventosBotones()
        {
            btnBuscar.Click += (s, e) => CambiarVista(false);
            btnRemplazar.Click += (s, e) => CambiarVista(true);
        }

        
        private void CambiarVista(bool esRemplazar)
        {
            if (esRemplazar)
            {
                ViewBuscar.Visibility = Visibility.Collapsed;
                ViewRemplazar.Visibility = Visibility.Visible;
                this.Title = "Remplazar";
            }
            else
            {
                ViewRemplazar.Visibility = Visibility.Collapsed;
                ViewBuscar.Visibility = Visibility.Visible;
                this.Title = "Buscar";
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MainBorder.Opacity = 1.0;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            MainBorder.Opacity = 0.5;
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
