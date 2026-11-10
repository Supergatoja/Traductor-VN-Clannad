using SeenExtractor.Data.Models.Lectura;
using System.Windows;

namespace SeenExtractor.Data.Models.Dialogos.Search
{
    public class ActionsSearch
    {
        public static SearchDialogModel Search_DialogS(string value, List<LinesSeen> lineasSeens, List<LinesSeen> newLineSeens)
        {
            var searchO = SearchDialog.Search(value, lineasSeens);
            var searchN = SearchDialog.Search(value, newLineSeens);
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2 , CurrentItem = 0};
        }

        public static SearchDialogModel Search_CharacterS(string value, List<LinesSeen> lineasSeens, List<LinesSeen> newLineSeens)
        {
            var searchO = SearchDialog.Search_Character(value, lineasSeens);
            var searchN = SearchDialog.Search_Character(value, newLineSeens);
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        public static void Replace_Dialog(Dictionary<string, string> value, List<LinesSeen> linesSeens)
        {
            var resultado = SearchDialog.Replace(value, linesSeens);
            Task.WhenAll(resultado);
            if (resultado.Result > 0)
                MessageBox.Show($"Dialogos afectados {resultado.Result}");
            else MessageBox.Show("No hay coincidencias");
        }

        internal static SearchDialogModel Search_Dialog(string value, List<LinesSeen> linesSeens1, List<LinesSeen> newLineSeens2)
        {

            var searchO = SearchDialog.Search(value.ToUpper(), Mayusculas(linesSeens1));
            var searchN = SearchDialog.Search(value.ToUpper(), Mayusculas(newLineSeens2));
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        internal static SearchDialogModel Search_Character(string value, List<LinesSeen> linesSeens1, List<LinesSeen> newLineSeens2)
        {

            var searchO = SearchDialog.Search_Character(value.ToUpper(), Mayusculas(linesSeens1));
            var searchN = SearchDialog.Search_Character(value.ToUpper(), Mayusculas(newLineSeens2));
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        internal static List<LinesSeen> Mayusculas(List<LinesSeen> linesSeens)
        {
            try
            {
                List<LinesSeen> list = [];
                foreach (var line in linesSeens)
                {
                    list.Add(new LinesSeen() { Line = line.Line, Character = line.Character.ToUpper(), Dialog = line.Dialog.ToUpper(), Length = line.Length });
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        } 

    }
}
