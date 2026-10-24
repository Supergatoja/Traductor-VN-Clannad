using System.IO;

namespace SeenExtractor.Data.Models.Lectura
{
    public partial class ReadFile(RellenarList rellenarList)
    {

        public async Task<List<LinesSeen>> ReadDialog(Stream file) => rellenarList.UseRegexComillasCP932ToList(file);

        public async Task<List<LinesSeen>> ReadDialog(string file)
        {
            using FileStream streamReader = new(file, FileMode.Open, FileAccess.Read);
            return rellenarList.UseRegexComillasCP932ToList(streamReader);
        }


    }
}
