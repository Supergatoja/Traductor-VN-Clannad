using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Encodings;
using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;
using System.Text.RegularExpressions;

namespace SeenExtractor.Data.Models.Lectura
{
    public class ReadTraducción
    {
        public static async Task<List<LinesSeen>> GetDialogsTranslate(Stream file)
        {
            var dialogs = new List<LinesSeen>();

            using (var reader = new StreamReader(file, GetEncodings.ANSII()))
            {
                string? line;
                //int i = 1;

                line = reader.ReadToEnd();
                var resultss = ConfigRegex.ExtractTranslateTwo().Matches(line);
                if (resultss.Count > 0)
                {
                    foreach (Match results in resultss)
                    {
                        if (results.Groups.Count > 1)
                        {
                            var numLineS = results.Groups[1].Value;
                            var nombre = results.Groups[2].Value.ConvertToCP932();
                            var newLine = results.Groups[3].Value;
                            var limiteS = results.Groups[4].Value;

                            _ = int.TryParse(limiteS, out int limite);

                            _ = int.TryParse(numLineS, out int numLine);
                            
                            var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                            if (matchLine != null)
                            {
                                matchLine.Dialog += $"\n{newLine}";
                            }
                            else dialogs.Add(new() { Line = numLine, Character = nombre, Dialog = newLine, Length = limite });

                        }

                    }
                }
                else
                {
                    resultss = ConfigRegex.ExtractTranslateTwoTwo().Matches(line);
                    foreach (Match results in resultss)
                    {
                        if (results.Groups.Count > 1)
                        {
                            var numLineS = results.Groups[1].Value;
                            var nombre = results.Groups[2].Value.ConvertToCP932();
                            var newLine = results.Groups[3].Value;
                            var limiteS = results.Groups[4].Value;
                            _ = int.TryParse(limiteS, out int limite);

                            _ = int.TryParse(numLineS, out int numLine);
                            var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                            if (matchLine != null)
                            {
                                matchLine.Dialog += $"\n{newLine}";
                            }
                            else dialogs.Add(new() { Line = numLine, Character = nombre, Dialog = newLine, Length = limite });

                        }
                    }
                }
            }

            return dialogs;
        }

        
         //* Metodo desactualizado, no requerido por el momento
        /*
        public async Task<List<LinesSeen>> GetDialogsTranslateClean(Stream file)
        {
            var dialogs = new List<LinesSeen>();

            using (var reader = new StreamReader(file, GetEncodings.ANSII()))
            {
                string? line;
                int numLine, limite;
                int i = 1;
                line = reader.ReadToEnd();

                var results = ConfigRegex.ExtractTranslate().Matches(line);
                foreach (Match item in results)
                {
                    if (item.Groups.Count > 1)
                    {
                        var numLineS = item.Groups[1].Value;
                        var newLine = item.Groups[2].Value;
                        var limiteS = item.Groups[3].Value;
                        int.TryParse(limiteS, out limite);
                        
                        int.TryParse(numLineS, out numLine);
                        var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                        if (matchLine != null)
                        {
                            matchLine.Dialog += $"\n{newLine}";
                        }
                        else dialogs.Add(new() { Line = numLine, Character = "", Dialog = newLine, Length = limite });

                        
                    }
                }

            }

            return dialogs;
        }

        */

    }
}
