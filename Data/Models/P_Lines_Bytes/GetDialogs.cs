using static SeenExtractor.Data.Characters.Get_Characters;
using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.RegexConfig;

namespace SeenExtractor.Data.Models.P_Lines_Bytes
{
    internal static class GetDialogs
    {
        //private static readonly Dictionary<string, string> Characteres = GetCharacteres;
        public static Dialogos_Conjuntos? Conjuntos = new();

        private static void CheckDialogoConjunto(ref int NumLine, out List<LinesSeen> lines)
        {
            if (Conjuntos.Character != "【】")
            {
                var lst = new List<LinesSeen>() { new() { Line = NumLine,
                                                                                    Character = "TiTULO ???",
                                                                                    Dialog = Conjuntos.Character,
                                                                                    Length = Conjuntos.Character.LengthBytesCP932() } };
                NumLine++;
                Conjuntos.Clean();
                lines = lst;
            }
            else
            { Conjuntos.Clean(); lines = []; }

        }


        public static List<LinesSeen> GetMulti(ref int NumLine, string Texto, int Type)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            _ = Type switch
            {
                0 => CheckCharacter(Texto),
                1 => Conjuntos.Dialog = Texto,
                _ => string.Empty
            };

            if (Conjuntos.Dialog == null)
            {
                lines.Add(new LinesSeen() { Line = NumLine, Length = NumLine });
                return lines;
            }
            else
            {
                if (Conjuntos.Character == null)
                    lines.Add(new LinesSeen() { Line = NumLine, Character = "Elección en pantalla", Dialog = Conjuntos.Dialog, Length = Conjuntos.Dialog.LengthBytesCP932() });
                else
                    lines.Add(new LinesSeen() { Line = NumLine, Character = Conjuntos.Character, Dialog = Conjuntos.Dialog, Length = Conjuntos.Dialog.LengthBytesCP932() });
                Conjuntos.Dialog = null;
                Conjuntos.Character = null;
                return lines;
            }
        }



        public static List<LinesSeen> GetPersonaje(ref int numLine, string texto)
        {
            CheckTipo(ref numLine, out List<LinesSeen> lines);
            var resultado = ConfigRegex.ExtractSelect().Match(texto);
            var Texto = resultado.Groups[1].Value;
            var PersonajeText = ConfigRegex.ExtractPersonajeBytes().Match(texto);
            var personaje = PersonajeText.Groups[0].Value;
            lines.Add(new LinesSeen() { Line = numLine, Character = GetCharacteres.TryGetValue(personaje, out string? value) ? value : personaje, Dialog = Texto, Length = Texto.LengthBytesCP932() });
            return lines;
        }



        public static List<LinesSeen> GetMoCo(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Monologo ó Concatenación", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }



        public static List<LinesSeen> GetAccion(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Acción ???", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }

        public static List<LinesSeen> GetInformacion(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Información", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }




        public static List<LinesSeen> GetLogro(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Logro", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }



        public static List<LinesSeen> GetGuia(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Palabra guía", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }



        public static List<LinesSeen> GetTitulo(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);
            lines.Add(new LinesSeen() { Line = NumLine, Character = "Titulo", Dialog = texto, Length = texto.LengthBytesCP932() });
            return lines;
        }



        public static List<LinesSeen> GetEleccion(ref int NumLine, string texto)
        {
            CheckTipo(ref NumLine, out List<LinesSeen> lines);

            var elecciones = texto.Split('∞');
            foreach (var eleccion in elecciones)
            {
                if (!string.IsNullOrEmpty(eleccion))
                {
                    var Bytes = eleccion.LengthBytesCP932();

                    lines.Add(new LinesSeen() { Line = NumLine, Character = "Elección", Dialog = eleccion, Length = Bytes });
                    NumLine++;
                }
            }
            NumLine--;
            return lines;
        }



        private static string CheckCharacter(string value)
        {
            var Texto = string.IsNullOrEmpty(value) ? "【】" : value;
            Conjuntos.Character = Texto;
            return Texto;
        }

        private static void CheckTipo(ref int NumLine, out List<LinesSeen> lines)
        {
            if (!string.IsNullOrEmpty(Conjuntos.Character))
                CheckDialogoConjunto(ref NumLine, out lines);
            else lines = [];
        }

    }
}
