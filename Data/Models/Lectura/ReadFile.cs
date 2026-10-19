using System.IO;

namespace SeenExtractor.Data.Models.Lectura
{
    public partial class ReadFile
    {
        private RellenarList _GetList;
        public ReadFile(RellenarList rellenarList)
        {
            _GetList = rellenarList;
        }

        public async Task<List<LinesSeen>> ReadDialog(Stream file) =>_GetList.UseRegexComillasCP932ToList(file);

        public async Task<List<LinesSeen>> ReadDialog(string file)
        {
            using FileStream streamReader = new(file, FileMode.Open, FileAccess.Read);
            return _GetList.UseRegexComillasCP932ToList(streamReader);
        }


    }
}
