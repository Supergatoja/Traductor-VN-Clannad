using SeenExtractor.Data.Models.Lectura;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeenExtractor.Data.Models.Sustitución
{
    public class Sustituir
    {
        public void Reverce(List<LinesSeen> newLinesSeens)
        {
            foreach (var item in newLinesSeens)
            {
                item.Dialog = item.Dialog.Replace('À', 'á')
                                                  .Replace('È', 'é').Replace('Ì', 'í')
                                                  .Replace('Ò', 'ó').Replace('Ù', 'ú')
                                                  .Replace('Û', 'ü').Replace('Ð', 'ñ');
            }
        }
        
        public void SustituirCaracteres_Klee(Dictionary<string,string> newLinesSeens)
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
        
        public void SustituirCaracteres_KleeTMP(Dictionary<string,string> newLinesSeens)
        {
            foreach (var item in newLinesSeens.Keys.ToList())
            {
                newLinesSeens[item] = newLinesSeens[item].Replace('Á', '#').Replace('É', '%')
                                                                            .Replace('Í', '\'').Replace('Ó', '<')
                                                                            .Replace('Ú', '>').Replace('Ü', '|')
                                                                            .Replace('Ñ', ']').Replace('À', '$')
                                                                            .Replace('È', '&').Replace('Ì', '@')
                                                                            .Replace('Ò', '=').Replace('Ù', '[')
                                                                            .Replace('Û', '}').Replace('Ð', '^')
                                                                            .Replace('¿', '`').Replace('¡', '{');
            }
        }

    }
}
