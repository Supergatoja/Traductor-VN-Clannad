using SeenExtractor.Data.Models.Dialogos.Search;
using SeenExtractor.Data.Models.Lectura;
using System.Windows;
using System.Windows.Controls;

namespace SeenExtractor.Data.Views.Search
{
    /// <summary>
    /// Lógica de interacción para ViewSearch.xaml
    /// </summary>
    public partial class ViewSearch : UserControl
    {
        Dictionary<string, object> Data;
        private readonly ActionsSearch Actions = new();
        public ViewSearch()
        {
            InitializeComponent();
        }


        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            ((Window)Data["Window"]).Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchDialogModel result;
            if ((bool)chbCharacter.IsChecked)
            {
                if ((bool)chbSensible.IsChecked)
                    result = ActionsSearch.Search_CharacterS(valor.Text, (List<LinesSeen>)Data["Dialogos Originales"], (List<LinesSeen>)Data["Dialogos Nuevos"]);
                else result = ActionsSearch.Search_Character(valor.Text, (List<LinesSeen>)Data["Dialogos Originales"], (List<LinesSeen>)Data["Dialogos Nuevos"]);
            }
            else{
                if ((bool)chbSensible.IsChecked)
                    result = ActionsSearch.Search_DialogS(valor.Text, (List<LinesSeen>)Data["Dialogos Originales"], (List<LinesSeen>)Data["Dialogos Nuevos"]);
                else result = ActionsSearch.Search_Dialog(valor.Text, (List<LinesSeen>)Data["Dialogos Originales"], (List<LinesSeen>)Data["Dialogos Nuevos"]);
            }

            if (result.Searches.Count > 0)
            {
                int matchIndex = result.GetMatch();

                var searchWindow = (Search)Data["Window"];
                if (searchWindow.Owner is MainWindow mainWindow)
                {
                    mainWindow.OriginalDialogs.SelectedIndex = matchIndex;
                    mainWindow.OriginalDialogs.ScrollIntoView(mainWindow.OriginalDialogs.SelectedItem);
                    mainWindow.Searches = result;
                    mainWindow.Focus();
                }
            }
            else
            {
                MessageBox.Show("Sin Coincidencias", "Busqueda", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Data = (Dictionary<string, object>)DataContext;
        }
    }
}
