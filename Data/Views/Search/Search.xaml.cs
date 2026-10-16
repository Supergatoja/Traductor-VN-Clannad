using SeenExtractor.Data.Models.Dialogos.Search;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Views.Replace;
using System.Windows;

namespace SeenExtractor.Data.Views.Search
{
    /// <summary>
    /// Lógica de interacción para Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public string? Prueba { get; set; }
        private ActionsSearch Actions = new();
        private List<LinesSeen> _NewLineSeens;
        private List<LinesSeen> _LineasSeens;
        private bool Mode;
        private List<LinesSeen> newLinesSeens;
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
        }

        public Search(List<LinesSeen> newLinesSeens)
        {
            InitializeComponent();
            _NewLineSeens = newLinesSeens;
            this.Title = "Remplazar";
            Mode = true;
            ViewRemplazar.Visibility = Visibility.Visible;
            Remplazar.DataContext = new Dictionary<string, object>() { { "Window", this }, { "Dialogos", newLinesSeens } };
        }

    }
}
