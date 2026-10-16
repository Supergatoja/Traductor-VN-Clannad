using SeenExtractor.Data.Models.RegexConfig;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SeenExtractor.Data.Models.Lectura
{
    public class ReadTraducción
    {
        ConfigRegex _Regex;
        public ReadTraducción(ConfigRegex read)
        {
            _Regex = read;
        }
        public async Task<List<LinesSeen>> GetDialogsTranslate(Stream file)
        {
            var dialogs = new List<LinesSeen>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            Encoding encodingCP932 = Encoding.GetEncoding(932);
            using (var reader = new StreamReader(file, encodingANSI))
            {
                string? line;
                int numLine, limite;
                int i = 1;

                line = reader.ReadToEnd();
                var extractDialog = _Regex.ExtractTranslateTwo();
                var extractDialogTwo = _Regex.ExtractTranslateTwoTwo();
                var resultss = extractDialog.Matches(line);
                if (resultss.Count > 0)
                {
                    foreach (Match results in resultss)
                    {
                        if (results.Groups.Count > 1)
                        {
                            var numLineS = results.Groups[1].Value;
                            var nombre = encodingCP932.GetString(encodingANSI.GetBytes(results.Groups[2].Value));
                            var newLine = results.Groups[3].Value;
                            var limiteS = results.Groups[4].Value;
                            int.TryParse(limiteS, out limite);
                            //1 2 4
                            int.TryParse(numLineS, out numLine);
                            
                            var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                            if (matchLine != null)
                            {
                                matchLine.Dialog += $"\n{newLine}";
                            }
                            else dialogs.Add(new() { Line = numLine, Character = nombre, Dialog = newLine, Length = limite });

                            //var translateLine = matchLine != null ? matchLine : new() { Line = numLine , Dialog = newLine, Length = newLine.Length };
                            var pedro = "";

                        }

                    }
                }
                else
                {
                    resultss = extractDialogTwo.Matches(line);
                    foreach (Match results in resultss)
                    {
                        if (results.Groups.Count > 1)
                        {
                            var numLineS = results.Groups[1].Value;
                            var nombre = encodingCP932.GetString(encodingANSI.GetBytes(results.Groups[2].Value));
                            var newLine = results.Groups[3].Value;
                            var limiteS = results.Groups[4].Value;
                            int.TryParse(limiteS, out limite);
                            //1 2 4
                            int.TryParse(numLineS, out numLine);
                            var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                            if (matchLine != null)
                            {
                                matchLine.Dialog += $"\n{newLine}";
                            }
                            else dialogs.Add(new() { Line = numLine, Character = nombre, Dialog = newLine, Length = limite });

                            //var translateLine = matchLine != null ? matchLine : new() { Line = numLine , Dialog = newLine, Length = newLine.Length };
                            var pedro = "";

                        }
                    }
                }
            }

            return dialogs;
        }

        public async Task<List<LinesSeen>> GetDialogsTranslateClean(Stream file)
        {
            var dialogs = new List<LinesSeen>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            using (var reader = new StreamReader(file, encodingANSI))
            {
                string? line;
                int numLine, limite;
                int i = 1;
                line = reader.ReadToEnd();

                var extractDialog = _Regex.ExtractTranslate();
                var results = extractDialog.Matches(line);
                foreach (Match item in results)
                {
                    if (item.Groups.Count > 1)
                    {
                        var numLineS = item.Groups[1].Value;
                        var newLine = item.Groups[2].Value;
                        var limiteS = item.Groups[3].Value;
                        int.TryParse(limiteS, out limite);
                        //1 2 4
                        int.TryParse(numLineS, out numLine);
                        var matchLine = dialogs.FirstOrDefault(Line => Line.Line == numLine);
                        if (matchLine != null)
                        {
                            matchLine.Dialog += $"\n{newLine}";
                        }
                        else dialogs.Add(new() { Line = numLine, Character = "", Dialog = newLine, Length = limite });

                        //var translateLine = matchLine != null ? matchLine : new() { Line = numLine , Dialog = newLine, Length = newLine.Length };
                        var pedro = "";

                    }
                }

            }

            return dialogs;
        }
    }
}
