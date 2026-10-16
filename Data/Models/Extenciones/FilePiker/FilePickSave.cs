using Microsoft.Win32;

namespace SeenExtractor.Data.Models.Extenciones.FilePiker
{
    public static class FilePickSave
    {
        public static bool? Save(this SaveFileDialog saveFileDialog)
        {
            saveFileDialog.InitialDirectory = "E:\\Seen Clannad\\Traduccion";
            saveFileDialog.DefaultDirectory = "E:\\Seen Clannad\\Traduccion";
            return saveFileDialog.ShowDialog();
        }
    }
}
