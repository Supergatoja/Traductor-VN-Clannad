using SeenExtractor.Data.Models.Dialogos.Search;
using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using static SeenExtractor.Data.Models.Extenciones.Theme.ApplyTheme;
using static SeenExtractor.EffectW11.DWM_SYSTEMBACKDROP_TYPE;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Views.Search;
using SeenExtractor.Data.Views.ViewTheme;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SeenExtractor
{
    public partial class MainWindow : Window
    {
        private readonly ActionFiles _ActionFiles;
        public List<LinesSeen> _LinesSeens;
        public List<LinesSeen> _NewLinesSeens;
        private List<LinesSeen> _NewLinesSeensRespaldo;
        private Dictionary<string[], List<LinesSeen>> _InfoFileRead;
        private Search search;
        private ChangeTheme Themes;
        //public static readonly RoutedCommand routedCommand = new();
        public SearchDialogModel Searches;
        public MainWindow(ActionFiles actionFiles)
        {
            InitializeComponent();
            _ActionFiles = actionFiles;
            //this.SourceInitialized += (s, e) => WindowEfectAcrylic.EnableBlur(this);
            this.SourceInitialized += (s, e) => Theme(this, DWMSBT_TABBEDWINDOW);
        }

        //Boton Convertir a CP932 pero lo uso para sustitur.
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _NewLinesSeensRespaldo = _NewLinesSeens;
            Regex suspiro = Suspiro();
            foreach (var item in _NewLinesSeens)
            {
                var fsys = suspiro.Replace(item.Dialog, dialog =>
                {
                    if (dialog.Groups[0].Value != string.Empty)
                    {
                        return "*Ahhh*";
                    }
                    return dialog.Value;
                });

                item.Dialog = fsys;

            }


        }

        private async void SeenOriginal_Click(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                ProgressBar.Visibility = Visibility.Visible;
                if (_NewLinesSeens != null)
                {
                    _InfoFileRead = await _ActionFiles.LeerDialogos();
                    _LinesSeens = _InfoFileRead.Values.First();
                    NuevosDialogs.ItemsSource = _NewLinesSeens;
                    OriginalDialogs.ItemsSource = _LinesSeens;
                    DataDialogNew();
                    ProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _InfoFileRead = await _ActionFiles.LeerDialogos();
                    _LinesSeens = _InfoFileRead.Values.First();
                    OriginalDialogs.ItemsSource = _LinesSeens;
                    ProgressBar.Visibility = Visibility.Collapsed;
                }

            });


        }
        private async void DialogosNuevos_Click(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                ProgressBar.Visibility = Visibility.Visible;
                _NewLinesSeens = await _ActionFiles.ImportarDialogos();
                if (_LinesSeens != null)
                {
                    NuevosDialogs.ItemsSource = _NewLinesSeens;
                    DataDialogNew();
                    ProgressBar.Visibility = Visibility.Collapsed;

                    //Fragmento de codigo para retificar que ambas entradas sean iguales (Solo para verificacion de dialogos extraidos sean iguales a los originales leidos)

                    try
                    {
                        for (int i = 0; i < _NewLinesSeens.Count; i++)
                        {
                            var texto = _NewLinesSeens[i].Dialog.BytesANSII();
                            var trutexto = texto.ToStringCP932();
                            if (trutexto != _LinesSeens[i].Dialog)
                            {
                                MessageBox.Show($"La CTM, Linea {_NewLinesSeens[i].Line}"); NuevosDialogs.SelectedIndex = i + 1;
                                if (NuevosDialogs.SelectedItem != null)
                                {
                                    NuevosDialogs.ScrollIntoView(NuevosDialogs.SelectedItem);
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error", e.Message, MessageBoxButton.CancelTryContinue, MessageBoxImage.Error);
                    }


                }
                else
                {
                    OriginalDialogs.ItemsSource = _NewLinesSeens;
                    ProgressBar.Visibility = Visibility.Collapsed;
                }
            });


        }

        private async void ExportDialogs_Click(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                ProgressBar.Visibility = Visibility.Visible;
                var response = _LinesSeens != null ? await _ActionFiles.ExportarDialogos(_LinesSeens) : await _ActionFiles.ExportarDialogos(_NewLinesSeens);
                ProgressBar.Visibility = Visibility.Collapsed;
                if (response.TryGetValue(true, out string? value))
                    MessageBox.Show("Guardado", value, MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Guardado", response[false], MessageBoxButton.OK, MessageBoxImage.Error);

            });
        }

        private async void OpenFolderDialogs_Click(object sender, ExecutedRoutedEventArgs e)
        {
            await Dispatcher.InvokeAsync(async () =>
            {
                ProgressBar.Visibility = Visibility.Visible;
                await _ActionFiles.LecturaMasivaDialogos();
                ProgressBar.Visibility = Visibility.Collapsed;
            });
        }

        private async void LineaSelectOriginal(object sender, RoutedEventArgs e)
        {
            if (OriginalDialogs.SelectedIndex != -1 && NuevosDialogs.ItemsSource != null)
            {
                NuevosDialogs.SelectedIndex = OriginalDialogs.SelectedIndex;
                if (NuevosDialogs.SelectedItem != null)
                {
                    NuevosDialogs.ScrollIntoView(NuevosDialogs.SelectedItem);
                }

            }
            else NuevosDialogs.SelectedIndex = -1;
        }
        private async void LineaSelectNuevo(object sender, RoutedEventArgs e)
        {
            if (NuevosDialogs.SelectedIndex != -1)
            {
                OriginalDialogs.SelectedIndex = NuevosDialogs.SelectedIndex;
                OriginalDialogs.ScrollIntoView(OriginalDialogs.SelectedItem);
            }
            else OriginalDialogs.SelectedIndex = -1;

        }

        private async void Guardar_Mezclar_Click(object sender, ExecutedRoutedEventArgs e)
        {
            await _ActionFiles.Guardar_Mezclar_Dialogos(_LinesSeens, _NewLinesSeens, _InfoFileRead.Keys.First());
            var response = await _ActionFiles.ExportarDialogos(_NewLinesSeens, _LinesSeens);
            if (response.TryGetValue(true, out string? value))
                MessageBox.Show("Guardado", value, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Guardado", response[false], MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void SuistituirCaracteres_Click(object sender, RoutedEventArgs e)
        {
            await ActionFiles.SustituirCharacter(_NewLinesSeens);
        }



        //* Metodo desactualizado, no requerido por el momento
        /*
       private async void DialogosNuevosClean_Click(object sender, ExecutedRoutedEventArgs e)
       {
           await Dispatcher.InvokeAsync(async () =>
           {
               ProgressBar.Visibility = Visibility.Visible;
               _NewLinesSeens = await _ActionFiles.ImportarDialogosClean();
               if (_LinesSeens != null)
               {
                   NuevosDialogs.ItemsSource = _NewLinesSeens;
                   DataDialogNew();
                   ProgressBar.Visibility = Visibility.Collapsed;
               }
               else
               {
                   OriginalDialogs.ItemsSource = _NewLinesSeens;
                   ProgressBar.Visibility = Visibility.Collapsed;
               }
           });


       }

       */
        private void DataDialogNew()
        {
            if (OriginalDialogs.ItemsSource != null)
            {
                NuevosDialogs.Visibility = NuevosDialogs.ItemsSource != null ? Visibility.Visible : Visibility.Collapsed;
                tamañoNuevo.Width = NuevosDialogs.Visibility != Visibility.Collapsed ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
                /*tamañoOriginal.Width = OriginalDialogs.Visibility == Visibility.Collapsed ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
                OriginalDialogs.Visibility = OriginalDialogs.ItemsSource != null ? Visibility.Visible : Visibility.Collapsed;*/
            }
            /*else
            {
                OriginalDialogs.Visibility = OriginalDialogs.ItemsSource != null ? Visibility.Visible : Visibility.Collapsed;
                tamañoOriginal.Width = OriginalDialogs.Visibility != Visibility.Collapsed ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
                tamañoNuevo.Width = NuevosDialogs.Visibility == Visibility.Collapsed ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
                NuevosDialogs.Visibility = NuevosDialogs.ItemsSource != null ? Visibility.Visible : Visibility.Collapsed;
            }*/

        }

        private void Edit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).MaxLength > 0)
            {
                if (((TextBox)sender).Text.Length > ((TextBox)sender).MaxLength)
                {
                    //((TextBox)sender).BorderBrush = Brushes.Red;
                    //((TextBox)sender).BorderThickness = new Thickness(10);
                    //((TextBox)sender).Background = Brushes.Red;
                    ((TextBox)sender).Foreground = Brushes.Red;

                }
                else
                {
                    ((TextBox)sender).Foreground = Brushes.White;
                }
            }

        }

        public void NextDialog(object sender, ExecutedRoutedEventArgs e)
        {
            Searches.CurrentItem++;
            OriginalDialogs.SelectedIndex = Searches.GetMatch();
            OriginalDialogs.ScrollIntoView(OriginalDialogs.SelectedItem);

        }
        public void NextDialog_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Searches == null)
            {
                e.CanExecute = false;
            }
            else if (Searches.Next)
            {
                e.CanExecute = true;
            }
            else { e.CanExecute = false; MessageBox.Show("Busqueda", "No hay mas coincidencias", MessageBoxButton.OK, MessageBoxImage.Information); }
        }

        public void BeforeDialog(object sender, ExecutedRoutedEventArgs e)
        {
            Searches.CurrentItem--;
            OriginalDialogs.SelectedIndex = Searches.GetMatch();
            OriginalDialogs.ScrollIntoView(OriginalDialogs.SelectedItem);

        }
        public void BeforeDialog_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Searches == null)
            {
                e.CanExecute = false;
            }
            else if (Searches.Before)
            {
                e.CanExecute = true;
            }
            else { e.CanExecute = false; MessageBox.Show("Busqueda", "No hay mas coincidencias", MessageBoxButton.OK, MessageBoxImage.Information); }
            ;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            search = new(_LinesSeens, _NewLinesSeens) { Owner = this };
            search.Show();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Replace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            search = new(_NewLinesSeens) { Owner = this };
            /*if ((bool)search.ShowDialog() == true)
            {

            }*/
            search.Show();
        }

        private void CleanTranslate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Limpiar", "¿Quiere limpiar la traduccion?", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Ignore, MessageBoxOptions.ServiceNotification) == MessageBoxResult.Yes)
            {
                _NewLinesSeens = null;
            }
        }

        private void Replace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CleanTranslate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _NewLinesSeens != null;
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_NewLinesSeens != null)
            {
                foreach (var item in _NewLinesSeens)
                {
                    e.CanExecute = !item.Excede;
                    if (!(e.CanExecute))
                        break;
                }
            }
            else
                e.CanExecute = false;
        }

        private void CustomTitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeTheme_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Themes = new();
            Themes.Show();
        }

        private void ChangeTheme_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        [GeneratedRegex(@"\*Suspiro\*", RegexOptions.Compiled)]
        private static partial Regex Suspiro();
    }
}