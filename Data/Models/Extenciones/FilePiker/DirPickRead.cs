using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class DirPickRead
    {
        public static bool? ReadDir(this OpenFolderDialog openFolderDialog)
        {
            openFolderDialog.InitialDirectory = "E:\\Seen Clannad\\Archivos Originales";
            openFolderDialog.DefaultDirectory = "E:\\Seen Clannad\\Archivos Originales";
            return openFolderDialog.ShowDialog();
        }
        public static bool? SaveDir(this OpenFolderDialog openFolderDialog)
        {
            openFolderDialog.InitialDirectory = "E:\\Seen Clannad\\Generacion";
            openFolderDialog.DefaultDirectory = "E:\\Seen Clannad\\Generacion";
            return openFolderDialog.ShowDialog();
        }
    }
}
