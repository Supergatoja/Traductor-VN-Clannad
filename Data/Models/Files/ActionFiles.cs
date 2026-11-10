using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Extenciones.FilePiker;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.Lectura.Masiva;

namespace SeenExtractor.Data.Models.Files
{
    public class ActionFiles(PickFile _File, ReadFile readFile, ExportDialogs exportDialogs, ReadTraducción traducción, ReplaceDialogs replaceDialogs)
    {
        public async Task<Dictionary<string[],List<LinesSeen>>?> LeerDialogos()
        {
            if ((bool)_File.PickFileSeen().ReadOriginal())
            {
                var file = _File.PickFileSeen().OpenFile();
                var dialogs = await Task.Run(() => readFile.ReadDialog(file));
                return new Dictionary<string[], List<LinesSeen>>() { { new string[] { _File.PickFileSeen().FileName, _File.PickFileSeen().SafeFileName }, dialogs } };
            }
            return null;
        }

        public async Task<Dictionary<bool,string>> ExportarDialogos(List<LinesSeen> dialogos, List<LinesSeen> Odialogos)
        {
            if ((bool)_File.SaveFileSeen().Save())
            {
                var dsFile = _File.SaveFileSeen().FileName;
                exportDialogs.LinesSeens = dialogos;
                exportDialogs.OLinesSeens = Odialogos;
                return await exportDialogs.SaveFileFialog(dsFile, true);
            }
            return new() { { false, "Operacion cancelada"} };
        }
        public async Task<Dictionary<bool,string>> ExportarDialogos(List<LinesSeen> dialogos)
        {
            if ((bool)_File.SaveFileSeen().Save())
            {
                var dsFile = _File.SaveFileSeen().FileName;
                exportDialogs.LinesSeens = dialogos;
                return await exportDialogs.SaveFileFialog(dsFile, false);
            }
            return new() { { false, "Operacion cancelada"} };
        }

        public async Task<List<LinesSeen>?> ImportarDialogos()
        {
            if ((bool)_File.PickFileSeen().ReadTranslate())
            {
                var file = _File.PickFileSeen().OpenFile();
                return await Task.Run(() => ReadTraducción.GetDialogsTranslate(file));
            }
            return null;
        }

        public async Task LecturaMasivaDialogos()
        {
            if ((bool)_File.OpenFolderSeen().ReadDir())
            {
                var patch = _File.OpenFolderSeen().FolderName;
                await ReadMasivSeen.GetFiles(patch);
            }


        }

        public async Task Guardar_Mezclar_Dialogos(List<LinesSeen> dialogosOriginal, List<LinesSeen> dialogosTranslate, string[] dataFile)
        {
            if ((bool)_File.OpenFolderSeen().SaveDir())
            {
                var patch = _File.OpenFolderSeen().FolderName;
                var dic = new Dictionary<string,string>();
                for (int i = 0; i < dialogosOriginal.Count ; i++)
                {
                    if (!dic.ContainsKey(dialogosOriginal[i].Dialog))
                    {
                        dic[dialogosOriginal[i].Dialog] = dialogosTranslate[i].Dialog;
                    }
                    
                }
                await replaceDialogs.SaveFileDialogInOriginal(dic, $"{patch}\\{dataFile[1]}", dataFile[0]);
            }
        }

        public static async Task SustituirCharacter(List<LinesSeen> newLinesSeens)
        {
            //await Task.Run(() => _ReplaceCharacters.SustituirCaracteres_Klee(newLinesSeens));
        }


    }
}
