using SeenExtractor.Data.Models.Dialogos.Search;
using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.Sustitución;
using System.Windows;
using System.Windows.Controls;

namespace SeenExtractor.Data.Views.Replace
{
    /// <summary>
    /// Lógica de interacción para Replace.xaml
    /// </summary>
    public partial class ViewReplace : UserControl
    {
        Dictionary<string, object> Data;
        private ActionsSearch Actions = new(); 
        public ViewReplace()
        {
            InitializeComponent();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            ((Window)Data["Window"]).Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //var result = Actions.Search_Dialog(valor.Text, ((List<LinesSeen>)Data["Dialogos Nuevos"]));
            var dialogos = ((MainWindow)(App.Current.MainWindow))._NewLinesSeens;
            
            if ((bool)Acentos.IsChecked)
            {
                Replace_Acentos(dialogos);
            }else Actions.Replace_Dialog(new Dictionary<string, string>() { { txtOriginal.Text, txtNuevo.Text } }, dialogos);
            ((Search.Search)Data["Window"]).DialogResult = true;
            ((Search.Search)Data["Window"]).Close();
        }


        private void Replace_Acentos(List<LinesSeen> dialogos)
        {
            //new Sustituir().SustituirCaracteres_Klee(dialogos);
        }

        private void UserControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Data = (Dictionary<string, object>)DataContext;
        }
    }
}
