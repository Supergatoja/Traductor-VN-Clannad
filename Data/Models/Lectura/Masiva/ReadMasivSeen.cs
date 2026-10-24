using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using System.IO;
using System.Windows;

namespace SeenExtractor.Data.Models.Lectura.Masiva
{
    public static class ReadMasivSeen
    {
        public static async Task GetFiles(string path)
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

        public static async Task CreateFiles(string path, string namefile, string pathDS)
        {
            var dialogos = await new ReadFile(new RellenarList(new StreamFile())).ReadDialog(path);
            var exportDialogos = new ExportDialogs()
            {
                LinesSeens = dialogos
            };
            await exportDialogos.SaveFileFialog($"{pathDS}{namefile}", false);
        }


    }
}
