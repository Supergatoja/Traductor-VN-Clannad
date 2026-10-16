using SeenExtractor.Data.Models.Lectura;
using System.IO;
using System.Windows;

namespace SeenExtractor.Data.Models.Escritura
{
    public class ReplaceDialogs
    {
        RellenarList _GetList;
        public ReplaceDialogs(RellenarList rellenarList)
        {
            _GetList = rellenarList;
        }

        public async Task<Dictionary<bool, string>> SaveFileDialogInOriginal(Dictionary<string, string> dialogos, string Patch, string PatchOriginal)
        {
            try
            {
                using (FileStream dsFile = new(Patch, FileMode.Create, FileAccess.Write))
                using (FileStream sr = new( PatchOriginal, FileMode.Open, FileAccess.Read))
                {
                    //await _GetList.UseRegexToMerch(sr, dsFile, dialogos);
                    await _GetList.UseRegexComillasCP932ToMerch(sr, dsFile, dialogos);
                }
                MessageBox.Show("Guardado","Guardado con exito", MessageBoxButton.OK, MessageBoxImage.Information);
                return new() { { true, "Guardado con exito" } };
            }
            catch (Exception e)
            {
                MessageBox.Show("Guardar","Ocurrio un error.\n" + e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return new() { { true, "Ocurrio un error" } };
            }


        }







    }
}
