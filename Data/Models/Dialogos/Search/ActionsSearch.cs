using SeenExtractor.Data.Models.Lectura;
using System.Windows;

namespace SeenExtractor.Data.Models.Dialogos.Search
{
    public class ActionsSearch
    {
        public ActionsSearch()
        {
            
        }

        public SearchDialogModel Search_Dialog(string value, List<LinesSeen> lineasSeens, List<LinesSeen> newLineSeens)
        {
            var searchO = new SearchDialog().Search(value, lineasSeens);
            var searchN = new SearchDialog().Search(value, newLineSeens);
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2 , CurrentItem = 0};
        }

        public SearchDialogModel Search_Character(string value, List<LinesSeen> lineasSeens, List<LinesSeen> newLineSeens)
        {
            var searchO = new SearchDialog().Search_Character(value, lineasSeens);
            var searchN = new SearchDialog().Search_Character(value, newLineSeens);
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        public void Replace_Dialog(Dictionary<string, string> value, List<LinesSeen> linesSeens)
        {
            var resultado = new SearchDialog().Replace(value, linesSeens);
            Task.WhenAll(resultado);
            if (resultado.Result > 0)
                MessageBox.Show($"Dialogos afectados {resultado.Result}");
            else MessageBox.Show("No hay coincidencias");
        }

        internal SearchDialogModel Search_Dialog(string value, List<LinesSeen> linesSeens1, List<LinesSeen> newLineSeens2, bool v)
        {

            var searchO = new SearchDialog().Search(value.ToUpper(), Mayusculas(linesSeens1));
            var searchN = new SearchDialog().Search(value.ToUpper(), Mayusculas(newLineSeens2));
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        internal SearchDialogModel Search_Character(string value, List<LinesSeen> linesSeens1, List<LinesSeen> newLineSeens2, bool v)
        {

            var searchO = new SearchDialog().Search_Character(value.ToUpper(), Mayusculas(linesSeens1));
            var searchN = new SearchDialog().Search_Character(value.ToUpper(), Mayusculas(newLineSeens2));
            Task.WhenAll(
                searchO, searchN
                );

            var Lists = searchO.Result.ToList();
            Lists.AddRange(searchN.Result);
            var listalimpia2 = Lists.DistinctBy(dialogo => dialogo.Line).ToList();
            var ListLimpia = Lists.Select(match => match.Line).Distinct().ToList();

            return new() { Searches = listalimpia2, CurrentItem = 0 };
        }

        internal List<LinesSeen> Mayusculas(List<LinesSeen> linesSeens)
        {
            List<LinesSeen> list = new();
            foreach (var line in linesSeens)
            {
                list.Add(new LinesSeen() { Line = line.Line, Character = line.Character.ToUpper() , Dialog = line.Dialog.ToUpper(), Length = line.Length });
            }
            return list;
        } 

    }
}
