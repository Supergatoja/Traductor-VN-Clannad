using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class FilePickSave
    {
        public static bool? Save(this SaveFileDialog saveFileDialog)
        {
            saveFileDialog.InitialDirectory = DirOpenTraduccion;
            saveFileDialog.DefaultDirectory = DirOpenTraduccion;
            return saveFileDialog.ShowDialog();
        }
    }
}
