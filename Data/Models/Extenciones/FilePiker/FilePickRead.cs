using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class FilePickRead
    {
        public static bool? ReadOriginal(this OpenFileDialog openFileDialog)
        {
            openFileDialog.InitialDirectory = "E:\\Seen Clannad\\Archivos Originales";
            openFileDialog.DefaultDirectory = "E:\\Seen Clannad\\Archivos Originales";
            return openFileDialog.ShowDialog();
        }
    
        public static bool? ReadTranslate(this OpenFileDialog openFileDialog)
        {
            openFileDialog.InitialDirectory = "E:\\Seen Clannad\\Traduccion";
            openFileDialog.DefaultDirectory = "E:\\Seen Clannad\\Traduccion";
            return openFileDialog.ShowDialog();
        }
    }
}
