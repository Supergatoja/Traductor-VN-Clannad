using System.IO;
using System.Text;


namespace SeenExtractor.Data.Models.Lectura
{
    public partial class ReadFile
    {
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
            
                return _GetList.UseRegexComillasCP932ToList(file);
            
        }

        public async Task<List<LinesSeen>> ReadDialog(string file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingCP932 = Encoding.GetEncoding(932);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            
            
            using (FileStream streamReader = new(file, FileMode.Open, FileAccess.Read))
            {
                return _GetList.UseRegexComillasCP932ToList(streamReader);
            }
        }


    }
}
