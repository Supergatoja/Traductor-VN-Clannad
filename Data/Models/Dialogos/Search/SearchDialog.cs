using SeenExtractor.Data.Models.Lectura;

namespace SeenExtractor.Data.Models.Dialogos.Search
{
    internal class SearchDialog
    {
        public async Task<List<SearchDialogResult>> Search(string value, List<Lectura.LinesSeen> newLineSeens)
        {
            List<SearchDialogResult> model = new();
            LinesSeen line;
            if (newLineSeens != null)
            {
                foreach (var lines in newLineSeens)
                {
                    if (lines.Dialog.Contains(value))
                    {
                        model.Add(new() { Line = lines.Line, Character = lines.Character, Dialog = lines.Dialog });
                    }
                }
            }

            return model;
        }
        public async Task<List<SearchDialogResult>> Search_Character(string value, List<Lectura.LinesSeen> newLineSeens)
        {
            List<SearchDialogResult> model = new();
            LinesSeen line;
            if (newLineSeens != null)
            {
                foreach (var lines in newLineSeens)
                {
                    if (lines.Character.Contains(value))
                    {
                        model.Add(new() { Line = lines.Line, Character = lines.Character, Dialog = lines.Dialog });
                    }
                }
            }

            return model;
        }

        public async Task<int> Replace(Dictionary<string, string> value, List<LinesSeen> linesSeen)
        {
            int Rows = 0;
            foreach (var lines in linesSeen)
            {
                if (lines.Dialog.Contains(value.Keys.First()))
                {
                    lines.Dialog = lines.Dialog.Replace(value.Keys.First(), value.Values.First());
                    Rows++;
                }
            }
            return Rows;
        }

    }
}
