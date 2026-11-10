using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class FilePickRead
    {
        public static bool? ReadOriginal(this OpenFileDialog openFileDialog)
        {
            openFileDialog.InitialDirectory = DirOpenOriginal;
            openFileDialog.DefaultDirectory = DirOpenOriginal;
            return openFileDialog.ShowDialog();
        }
    
        public static bool? ReadTranslate(this OpenFileDialog openFileDialog)
        {
            openFileDialog.InitialDirectory = DirOpenTraduccion;
            openFileDialog.DefaultDirectory = DirOpenTraduccion;
            return openFileDialog.ShowDialog();
        }
    }
}
