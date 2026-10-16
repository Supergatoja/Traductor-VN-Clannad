using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Extenciones.FilePiker;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.Lectura.Masiva;
using SeenExtractor.Data.Models.Sustitución;
using System.Text;

namespace SeenExtractor.Data.Models.Files
{
    public class ActionFiles
    {
        private PickFile _File;
        private ReadFile _ReadFile;
        private ExportDialogs _ExportDialogs;
        private ConvertFile _ConvertFile;
        private ReadMasivSeen _ReadMasivSeen;
        private ReadTraducción _ReadTraducción;
        private ReplaceDialogs _ReplaceDialogs;
        private Sustituir _ReplaceCharacters;

        public ActionFiles(PickFile file, ReadFile readFile, ExportDialogs exportDialogs, ConvertFile convertFile, ReadMasivSeen readMasivSeen, ReadTraducción traducción, ReplaceDialogs replaceDialogs, Sustituir sustituir)
        {
            _File = file;
            _ReadFile = readFile;
            _ExportDialogs = exportDialogs;
            _ConvertFile = convertFile;
            _ReadMasivSeen = readMasivSeen;
            _ReadTraducción = traducción;
            _ReplaceDialogs = replaceDialogs;
            _ReplaceCharacters = sustituir;
        }
        public async Task<Dictionary<string[],List<LinesSeen>>> LeerDialogos()
        {
            if ((bool)_File.PickFileSeen().ReadOriginal())
            {
                var file = _File.PickFileSeen().OpenFile();
                var dialogs = await Task.Run(() => _ReadFile.ReadDialog(file));
                return new Dictionary<string[], List<LinesSeen>> () { { new string[] { _File.PickFileSeen().FileName, _File.PickFileSeen().SafeFileName }, dialogs }};
            }
            return null;
        }

        public async Task<Dictionary<bool,string>> ExportarDialogos(List<LinesSeen> dialogos, List<LinesSeen> Odialogos)
        {
            if ((bool)_File.SaveFileSeen().Save())
            {
                var dsFile = _File.SaveFileSeen().FileName;
                _ExportDialogs.LinesSeens = dialogos;
                _ExportDialogs.OLinesSeens = Odialogos;
                return await _ExportDialogs.SaveFileFialog(dsFile, true);
            }
            return new() { { false, "Operacion cancelada"} };
        }
        public async Task<Dictionary<bool,string>> ExportarDialogos(List<LinesSeen> dialogos)
        {
            if ((bool)_File.SaveFileSeen().Save())
            {
                var dsFile = _File.SaveFileSeen().FileName;
                _ExportDialogs.LinesSeens = dialogos;
                return await _ExportDialogs.SaveFileFialog(dsFile, false);
            }
            return new() { { false, "Operacion cancelada"} };
        }

        public async Task<List<LinesSeen>> ImportarDialogos()
        {
            if ((bool)_File.PickFileSeen().ReadTranslate())
            {
                var file = _File.PickFileSeen().OpenFile();
                return await Task.Run(() => _ReadTraducción.GetDialogsTranslate(file));
            }
            return null;
        }

        public async Task LecturaMasivaDialogos()
        {
            if ((bool)_File.OpenFolderSeen().ReadDir())
            {
                var patch = _File.OpenFolderSeen().FolderName;
                await _ReadMasivSeen.GetFiles(patch);
            }


        }

        public async Task Guardar_Mezclar_Dialogos(List<LinesSeen> dialogosOriginal, List<LinesSeen> dialogosTranslate, string[] dataFile)
        {
            if ((bool)_File.OpenFolderSeen().SaveDir())
            {
                var patch = _File.OpenFolderSeen().FolderName;
                Dictionary<string,string> dic = new Dictionary<string,string>();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var cp932 = Encoding.GetEncoding(932);
                for (int i = 0; i < dialogosOriginal.Count ; i++)
                {
                    if (!dic.ContainsKey(dialogosOriginal[i].Dialog))
                    {

                        //dic[dialogosOriginal[i].Dialog] = dialogosTranslate[i].Japones ? cp932.GetString(cp932.GetBytes(dialogosTranslate[i].Dialog)) : dialogosTranslate[i].Dialog;
                        dic[dialogosOriginal[i].Dialog] = dialogosTranslate[i].Dialog;
                    }
                    
                }
                await _ReplaceDialogs.SaveFileDialogInOriginal(dic, $"{patch}\\{dataFile[1]}", dataFile[0]);
            }
        }

        public async Task SustituirCharacter(List<LinesSeen> newLinesSeens)
        {
            //await Task.Run(() => _ReplaceCharacters.SustituirCaracteres_Klee(newLinesSeens));
        }

        public async Task<List<LinesSeen>> ImportarDialogosClean()
        {
            if ((bool)_File.PickFileSeen().ReadTranslate())
            {
                var file = _File.PickFileSeen().OpenFile();
                return await Task.Run(() => _ReadTraducción.GetDialogsTranslateClean(file));
            }
            return null;
        }

        /*public async Task ConvertCP932()
        {

        }*/

    }
}
