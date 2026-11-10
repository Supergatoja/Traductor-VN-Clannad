using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class DirPickRead
    {
        public static bool? ReadDir(this OpenFolderDialog openFolderDialog)
        {
            openFolderDialog.InitialDirectory = DirOpenOriginal;
            openFolderDialog.DefaultDirectory = DirOpenOriginal;
            return openFolderDialog.ShowDialog();
        }
        public static bool? SaveDir(this OpenFolderDialog openFolderDialog)
        {
            openFolderDialog.InitialDirectory = DirOpenGeneracio;
            openFolderDialog.DefaultDirectory = DirOpenGeneracion;
            return openFolderDialog.ShowDialog();
        }
    }
}
