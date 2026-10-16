using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace SeenExtractor.Data.Models.Lectura
{
    public partial class ReadFile
    {
        /*public async Task<GetLines> PickFile()
        {
            if ((bool)_File.PickFileSeen().ShowDialog())
            {
                return await Task.Run(() => ReadDialog(_File.PickFileSeen().OpenFile()));
            }
            //if ((bool)filePicker.ShowDialog())
            {
                var file = filePicker.OpenFile();
                return await Task.Run(() => ReadDialog(file));
            //}
            return null;
        }*/
        private RellenarList _GetList;
        public ReadFile(RellenarList rellenarList)
        {
            _GetList = rellenarList;
        }

        public async Task<List<LinesSeen>> ReadDialog(Stream file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingCP932 = Encoding.GetEncoding(932);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            //List<LinesSeen> lines = new List<LinesSeen>();
            
                //return _GetList.UseRegexToList(file);
                return _GetList.UseRegexComillasCP932ToList(file);
            
        }

        public async Task<List<LinesSeen>> ReadDialog(string file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingCP932 = Encoding.GetEncoding(932);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            //List<LinesSeen> lines = new List<LinesSeen>();
            
            using (FileStream streamReader = new(file, FileMode.Open, FileAccess.Read))
            {
                return _GetList.UseRegexComillasCP932ToList(streamReader);
            }
        }


    }
}
