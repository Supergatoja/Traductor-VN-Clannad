using SeenExtractor.Data.Models.Lectura;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeenExtractor.Data.Models.Sustitución
{
    public static class Sustituir
    {
        public static void SustituirCaracteres_Klee(this Dictionary<string,string> newLinesSeens)
        {
            foreach (var item in newLinesSeens.Keys.ToList())
            {
                newLinesSeens[item] = newLinesSeens[item].Replace('Á', '#').Replace('É', '%')
                                                                            .Replace('Í', '\'').Replace('Ó', '<')
                                                                            .Replace('Ú', '>').Replace('Ü', '|')
                                                                            .Replace('Ñ', ']').Replace('á', '$')
                                                                            .Replace('é', '&').Replace('í', '@')
                                                                            .Replace('ó', '=').Replace('ú', '[')
                                                                            .Replace('ü', '}').Replace('ñ', '^')
                                                                            .Replace('¿', '`').Replace('¡', '{');
            }
        }

    }
}
