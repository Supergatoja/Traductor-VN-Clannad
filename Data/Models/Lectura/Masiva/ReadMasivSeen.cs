using Microsoft.Win32;
using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;
using System.Windows;

namespace SeenExtractor.Data.Models.Lectura.Masiva
{
    public class ReadMasivSeen
    {
        private PickFile _Files;
        private ReadFile _ReadFile;
        private ExportDialogs _ExportDialogs;
        public ReadMasivSeen(PickFile pickFile, ReadFile readFile, ExportDialogs exportDialogs)
        {
            _Files = pickFile;
            _ReadFile = readFile;
            _ExportDialogs = exportDialogs;
        }

        public async Task GetFiles(string path)
        {
            string names = "Seen", extencion = ".txt", Resultado = string.Empty;

            var files = Directory.GetFiles($"{path}\\", $"{names}*{extencion}");
            if (files.Length > 0)
            {
                var newPath = Directory.CreateDirectory($"{path}\\Extacción\\");

                try
                {
                    await Parallel.ForEachAsync(files, async (file, _) =>
                    {
                        CreateFiles(file, Path.GetFileName(file), newPath.FullName);
                    });
                    MessageBox.Show("Extraccion masiva satisfactoria","Acción Masiva", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"A ocurrido un error.\n{e.Message}", "Acción Masiva", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            
        }

        public async Task CreateFiles(string path, string namefile, string pathDS)
        {
            var dialogos = await new ReadFile(new RellenarList(new ConfigRegex(), new StreamFile(), new P_Lines(new ConfigRegex(), new Dialogos_Conjuntos()))).ReadDialog(path);
            var exportDialogos = new ExportDialogs(new PickFile(new OpenFileDialog(), new SaveFileDialog(), new OpenFolderDialog()))
            {
                LinesSeens = dialogos
            };
            await exportDialogos.SaveFileFialog($"{pathDS}{namefile}", false);
            /*var dialogos = await _ReadFile.ReadDialog(path);
            _ExportDialogs.LinesSeens = dialogos;
            await _ExportDialogs.SaveFileFialog($"{pathDS}{namefile}");*/
        }


    }
}
