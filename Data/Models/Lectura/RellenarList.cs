using SeenExtractor.Data.Characters;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;
using SeenExtractor.Data.Models.RegexConfig;
using SeenExtractor.Data.Models.Sustitución;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace SeenExtractor.Data.Models.Lectura
{
    public class RellenarList
    {
        private ConfigRegex _Regex;
        private StreamFile _FileRead;
        private P_Lines _P_Lines;
        private Dictionary<string, string> Get_Characters = new Get_Characters().GetCharacteres();

        public RellenarList(ConfigRegex configRegex, StreamFile streamFile, P_Lines p_Lines)
        {
            _Regex = configRegex;
            _FileRead = streamFile;
            _P_Lines = p_Lines;
        }

        public List<LinesSeen> UseRegexToList(Stream streamReader)
        {
            List<LinesSeen> lines = new();
            string line, endline;

            int i = 1;
            int debug = 1;
            Regex regexDialogs = _Regex.ExtractDialogCP932();
            Regex regexSelect = _Regex.ExtractSelect();
            //line = streamReader.ReadToEnd();
            /*
            while (_FileRead.ReadLineCus(streamReader, out line, out endline))
            {
                var resultado = regexComillas.Matches(line);

                foreach (Match coincidencias in resultado)
                {
                    if (i == 24)
                    {
                        var pedro = "";
                    }
                    if (debug == 6565)
                    {
                        var pedro = "";
                    }
                    if (coincidencias.Groups.Count > 1)
                    {
                        string lineaDialogo = coincidencias.Groups[2].Value;
                        if (lineaDialogo != string.Empty)
                        {
                            /*if (!dialogos.ContainsKey(lineaDialogo))
                            {
                                dialogos[numLine + i] = lineaDialogo;
                                LineasDialogos[numLine + i] = dialogos;
                                i++;
                            }
                            lines.Add(new LinesSeen() { Line = i, Dialog = lineaDialogo, Length = lineaDialogo.Length });
                            i++;

                        }
                        else
                        {
                            var lineaDialogoEleccion = coincidencias.Groups[4].Value;
                            if (lineaDialogoEleccion != string.Empty)
                            {
                                var lineasElecciones = _Regex.ExtractSelect().Matches(lineaDialogoEleccion);
                                foreach (Match item in lineasElecciones)
                                {
                                    /*if (!dialogos.ContainsKey(item.Groups[1].Value))
                                    {
                                        dialogos[numLine + i] = item.Groups[1].Value;
                                        LineasDialogos[numLine + i] = dialogos;
                                        i++;
                                    }

                                    lines.Add(new LinesSeen() { Line = i, Dialog = item.Groups[1].Value, Length = item.Groups[1].Value.Length });
                                    i++;

                                }
                            }
                            else
                            {
                                var lineaTitulo = coincidencias.Groups[6].Value;
                                if (lineaTitulo != string.Empty)
                                {
                                    lines.Add(new LinesSeen() { Line = i, Dialog = lineaTitulo, Length = lineaTitulo.Length });
                                    i++;
                                }
                                else
                                {
                                    var lineaFraseGuia = coincidencias.Groups[9].Value;
                                    if (lineaFraseGuia != string.Empty)
                                    {
                                        lines.Add(new LinesSeen() { Line = i, Dialog = lineaFraseGuia, Length = lineaFraseGuia.Length });
                                        i++;
                                    }
                                    else
                                    {
                                        var lineaProblematica = coincidencias.Groups[8].Value;
                                        if (lineaProblematica != string.Empty)
                                        {
                                            var lineaProblematica2 = _Regex.ExtractSelect().Matches(lineaProblematica);
                                            foreach (Match item in lineaProblematica2)
                                            {
                                                lines.Add(new LinesSeen() { Line = i, Dialog = item.Groups[1].Value, Length = item.Groups[1].Value.Length });
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }


                        }


                    }

                }
                debug++;
            }
            */


            /*
                var resultado = regexComillas.Matches(line);

                foreach (Match coincidencias in resultado)
                {
                    if (i == 24)
                    {
                        var pedro = "";
                    }
                    if (debug == 6565)
                    {
                        var pedro = "";
                    }
                    if (coincidencias.Groups.Count > 1)
                    {
                        string lineaDialogo = coincidencias.Groups[2].Value;
                        if (lineaDialogo != string.Empty)
                        {
                            /*if (!dialogos.ContainsKey(lineaDialogo))
                            {
                                dialogos[numLine + i] = lineaDialogo;
                                LineasDialogos[numLine + i] = dialogos;
                                i++;
                            }
                            lines.Add(new LinesSeen() { Line = i, Dialog = lineaDialogo, Length = lineaDialogo.Length });
                            i++;

                        }
                        else
                        {
                            var lineaDialogoEleccion = coincidencias.Groups[4].Value;
                            if (lineaDialogoEleccion != string.Empty)
                            {
                                var lineasElecciones = _Regex.ExtractSelect().Matches(lineaDialogoEleccion);
                                foreach (Match item in lineasElecciones)
                                {
                                    /*if (!dialogos.ContainsKey(item.Groups[1].Value))
                                    {
                                        dialogos[numLine + i] = item.Groups[1].Value;
                                        LineasDialogos[numLine + i] = dialogos;
                                        i++;
                                    }

                                    lines.Add(new LinesSeen() { Line = i, Dialog = item.Groups[1].Value, Length = item.Groups[1].Value.Length });
                                    i++;

                                }
                            }
                            else
                            {
                                var lineaTitulo = coincidencias.Groups[6].Value;
                                if (lineaTitulo != string.Empty)
                                {
                                    lines.Add(new LinesSeen() { Line = i, Dialog = lineaTitulo, Length = lineaTitulo.Length });
                                    i++;
                                }
                                else
                                {
                                    var lineaFraseGuia = coincidencias.Groups[9].Value;
                                    if (lineaFraseGuia != string.Empty)
                                    {
                                        lines.Add(new LinesSeen() { Line = i, Dialog = lineaFraseGuia, Length = lineaFraseGuia.Length });
                                        i++;
                                    }
                                    else
                                    {
                                        var lineaProblematica = coincidencias.Groups[8].Value;
                                        if (lineaProblematica != string.Empty)
                                        {
                                            var lineaProblematica2 = _Regex.ExtractSelect().Matches(lineaProblematica);
                                            foreach (Match item in lineaProblematica2)
                                            {
                                                lines.Add(new LinesSeen() { Line = i, Dialog = item.Groups[1].Value, Length = item.Groups[1].Value.Length });
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }


                        }


                    }

                }

                
                */

            /*
             * Grupos
             * 2 - Nombre del personaje
             * 3 - Dialogo de personaje
             * 6 - Monologo, interlocutor,etc.
             * 15 - Patron Elección
             * 17 - Titulo
             * 20 - Logros
             * 23 - Palabra Guia
             * Line 1015 para eleccion debug 926
             * Linea 10113 revizar por que no mezclo
             * linea 10994 titulo
             * Linea 1782 revizar por que no mezclo debug 1682
             */

            string nombrePersonaje, lineaDialogoPersonaje, lineaDialogoMonologoEtc, lineaPatronEleccion, lineaEleccion, lineaDialogoTitulo, lineaDialogoLogro, lineaDialogoGuia;

            while (_FileRead.ReadLineCusCP932(streamReader, out line, out endline))
            {
                var resultado = regexDialogs.Matches(line);

                foreach (Match coincidencias in resultado)
                {
                    if (coincidencias.Groups.Count > 1)
                    {
                        if ((nombrePersonaje = coincidencias.Groups[2].Value) != string.Empty)
                        {
                            if (Get_Characters.ContainsKey(nombrePersonaje))
                            {
                                nombrePersonaje = Get_Characters[nombrePersonaje];
                            }

                        }
                        else
                        {
                            if ((nombrePersonaje = coincidencias.Groups[9].Value) != string.Empty)
                            {
                                if (Get_Characters.ContainsKey(nombrePersonaje))
                                {
                                    nombrePersonaje = Get_Characters[nombrePersonaje];
                                }
                            }
                            else
                            {
                                nombrePersonaje = Get_Characters[nombrePersonaje];
                            }
                        }


                        if ((lineaDialogoPersonaje = coincidencias.Groups[3].Value) != string.Empty)
                        {
                            lines.Add(new LinesSeen() { Line = i, Character = nombrePersonaje, Dialog = lineaDialogoPersonaje, Length = lineaDialogoPersonaje.Length });
                            i++;
                        }


                        if ((lineaDialogoMonologoEtc = coincidencias.Groups[6].Value) != string.Empty)
                        {
                            lines.Add(new LinesSeen() { Line = i, Character = nombrePersonaje, Dialog = lineaDialogoMonologoEtc, Length = lineaDialogoMonologoEtc.Length });
                            i++;
                        }


                        if ((lineaPatronEleccion = coincidencias.Groups[15].Value) != string.Empty)
                        {
                            var elecciones = regexSelect.Matches(lineaPatronEleccion);
                            foreach (Match eleccion in elecciones)
                            {
                                if ((lineaEleccion = eleccion.Groups[1].Value) != string.Empty)
                                {
                                    lines.Add(new LinesSeen() { Line = i, Character = "Elección", Dialog = lineaEleccion, Length = lineaEleccion.Length });
                                    i++;
                                }
                            }

                        }


                        if ((lineaDialogoTitulo = coincidencias.Groups[17].Value) != string.Empty)
                        {
                            lines.Add(new LinesSeen() { Line = i, Character = "Titulo", Dialog = lineaDialogoTitulo, Length = lineaDialogoTitulo.Length });
                            i++;
                        }


                        if ((lineaDialogoLogro = coincidencias.Groups[20].Value) != string.Empty)
                        {
                            lines.Add(new LinesSeen() { Line = i, Character = "Logro", Dialog = lineaDialogoLogro, Length = lineaDialogoLogro.Length });
                            i++;
                        }


                        if ((lineaDialogoGuia = coincidencias.Groups[23].Value) != string.Empty)
                        {
                            lines.Add(new LinesSeen() { Line = i, Character = "Palabra Guia", Dialog = lineaDialogoGuia, Length = lineaDialogoGuia.Length });
                            i++;
                        }


                    }

                }

                debug++;
            }

            return lines;
        }

        public async Task UseRegexToMerch(FileStream streamReader, FileStream streamWriter, Dictionary<string, string> dialogos)
        {
            List<LinesSeen> lines = new();
            string line, endline;
            int i = 1;
            bool data = true;
            int debug = 1;
            Regex regexDialogs = _Regex.ExtractDialogCP932();
            Regex regexSelect = _Regex.ExtractSelect();


            string lineaDialogoPersonaje, lineaDialogoMonologoEtc, lineaPatronEleccion, lineaEleccion, lineaDialogoTitulo, lineaDialogoLogro, lineaDialogoGuia;

            while (_FileRead.ReadLineCusCP932(streamReader, out line, out endline))
            {
                //var resultado = regexDialogs.Matches(line);
                data = true;
                var espacios = string.Empty;
                var resultado = regexDialogs.Replace(line, coincidencias =>
                {


                    if (coincidencias.Groups.Count > 1)
                    {

                        if ((lineaDialogoPersonaje = coincidencias.Groups[3].Value) != string.Empty)
                        {
                            if (dialogos.ContainsKey(lineaDialogoPersonaje))
                            {
                                var dialogoCP932 = GetString(dialogos[lineaDialogoPersonaje], lineaDialogoPersonaje, out espacios);

                                var cambio = regexSelect.Replace(coincidencias.Value, remplazo =>
                                {
                                    if (remplazo.Groups[1].Value == lineaDialogoPersonaje)
                                    {
                                        return $"\"{dialogoCP932}\"{espacios}";
                                    }
                                    else
                                    {
                                        data = false;
                                        return string.Empty;
                                    }

                                });
                                if (data)
                                {
                                    return cambio;
                                }
                                else return coincidencias.Value.Replace(lineaDialogoPersonaje, $"{dialogoCP932}{espacios}");

                            }
                        }


                        if ((lineaDialogoMonologoEtc = coincidencias.Groups[6].Value) != string.Empty)
                        {
                            if (dialogos.ContainsKey(lineaDialogoMonologoEtc))
                            {
                                var dialogoCP932 = GetString(dialogos[lineaDialogoMonologoEtc], lineaDialogoMonologoEtc, out espacios);

                                var cambio = regexSelect.Replace(coincidencias.Value, remplazo =>
                                {
                                    if (remplazo.Groups[1].Value == lineaDialogoMonologoEtc)
                                    {
                                        return $"\"{dialogoCP932}\"{espacios}";
                                    }
                                    else
                                    {
                                        data = false;
                                        return string.Empty;
                                    }

                                });
                                if (data)
                                {
                                    return cambio;
                                }
                                else return coincidencias.Value.Replace(lineaDialogoMonologoEtc, $"{dialogoCP932}{espacios}");


                            }
                        }


                        if ((lineaPatronEleccion = coincidencias.Groups[15].Value) != string.Empty)
                        {
                            var elecciones = regexSelect.Replace(lineaPatronEleccion, eleccion =>
                            {
                                if ((lineaEleccion = eleccion.Groups[1].Value) != string.Empty)
                                {
                                    if (dialogos.ContainsKey(lineaEleccion))
                                    {
                                        var dialogoCP932 = GetString(dialogos[lineaEleccion], lineaEleccion, out espacios);

                                        return $"\"{dialogoCP932}\"{espacios}";

                                    }
                                }
                                return eleccion.Value;
                            });

                            return elecciones;

                        }


                        if ((lineaDialogoTitulo = coincidencias.Groups[17].Value) != string.Empty)
                        {
                            if (dialogos.ContainsKey(lineaDialogoTitulo))
                            {
                                var dialogoCP932 = GetString(dialogos[lineaDialogoTitulo], lineaDialogoTitulo, out espacios);

                                var cambio = regexSelect.Replace(coincidencias.Value, remplazo =>
                                {
                                    if (remplazo.Groups[1].Value == lineaDialogoTitulo)
                                    {
                                        return $"\"{dialogoCP932}\"{espacios}";
                                    }
                                    else
                                    {
                                        data = false;
                                        return string.Empty;
                                    }

                                });
                                if (data)
                                {
                                    return cambio;
                                }
                                else return coincidencias.Value.Replace(lineaDialogoTitulo, $"{dialogoCP932}{espacios}");

                            }
                        }


                        if ((lineaDialogoLogro = coincidencias.Groups[20].Value) != string.Empty)
                        {
                            if (dialogos.ContainsKey(lineaDialogoLogro))
                            {
                                var dialogoCP932 = GetString(dialogos[lineaDialogoLogro], lineaDialogoLogro, out espacios);

                                var cambio = regexSelect.Replace(coincidencias.Value, remplazo =>
                                {
                                    if (remplazo.Groups[1].Value == lineaDialogoLogro)
                                    {
                                        return $"\"{dialogoCP932}\"{espacios}";
                                    }
                                    else
                                    {
                                        data = false;
                                        return string.Empty;
                                    }

                                });
                                if (data)
                                {
                                    return cambio;
                                }
                                else return coincidencias.Value.Replace(lineaDialogoLogro, $"{dialogoCP932}{espacios}");

                            }
                        }


                        if ((lineaDialogoGuia = coincidencias.Groups[23].Value) != string.Empty)
                        {
                            if (dialogos.ContainsKey(lineaDialogoGuia))
                            {
                                var dialogoCP932 = GetString(dialogos[lineaDialogoGuia], lineaDialogoGuia, out espacios);

                                var cambio = regexSelect.Replace(coincidencias.Value, remplazo =>
                                {
                                    if (remplazo.Groups[1].Value == lineaDialogoGuia)
                                    {
                                        return $"\"{dialogoCP932}\"{espacios}";
                                    }
                                    else
                                    {
                                        data = false;
                                        return string.Empty;
                                    }

                                });
                                if (data)
                                {
                                    return cambio;
                                }
                                else return coincidencias.Value.Replace(lineaDialogoGuia, $"{dialogoCP932}{espacios}");

                            }
                        }


                    }

                    return coincidencias.Value;


                });


                byte[] BytesResultado = GetBytes(resultado);

                streamWriter.Write(BytesResultado, 0, BytesResultado.Length);

                byte[] BytesEndLine;
                if (endline != null)
                {
                    BytesEndLine = GetBytes(endline);
                    streamWriter.Write(BytesEndLine, 0, BytesEndLine.Length);
                }


            }



        }


        public List<LinesSeen> UseRegexComillasCP932ToList(Stream streamReader)
        {
            var linesSeen = new List<LinesSeen>();
            string Texto, Tipo;
            bool Datos;
            byte[] bytes;
            int ByteLength;
            int NumLine = 1;

            try
            {

                while (_FileRead.ReadTrueArchiveBytes(streamReader, out bytes, out Texto, out Datos, out Tipo, out ByteLength))
                {
                    if (Datos)
                    {
                        bool eleccion = false;
                        bool DC_P = false;
                        var line = Tipo switch
                        {
                            "Personaje" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Monologo o Concatenacion" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Titulo" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Logro" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Palabra guia" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Eleccion" => _P_Lines.GetEleccion(NumLine, Texto),
                            "DIALOGOS CONJUNTOS (PERSONAJE)" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "DIALOGOS CONJUNTOS (DIALOGO)" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Acción ???" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            "Información" => _P_Lines.GetLines(NumLine, Texto, Tipo, ByteLength),
                            _ => null
                        };
                        if (!string.IsNullOrEmpty(line[0].Character))
                        {
                            linesSeen.AddRange(line);
                            NumLine += line.Count;
                        }


                    }

                }

                if (!string.IsNullOrEmpty(_P_Lines.Conjuntos.Character))
                {
                    linesSeen.Add(new LinesSeen()
                    {
                        Line = NumLine,
                        Character = "TiTULO ???",
                        Dialog = _P_Lines.Conjuntos.Character,
                        Length = _P_Lines.Bytes_DC_P()
                    });
                }
                _P_Lines.Conjuntos.Clean();

            }
            catch (Exception e)
            {
                MessageBox.Show("Error",e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return linesSeen;
        }



        public async Task UseRegexComillasCP932ToMerch(Stream streamReader, Stream streamWriter, Dictionary<string, string> dialogos)
        {
            string Texto, Tipo;
            bool Datos;
            byte[] bytes;
            int ByteLength;
            int NumLine = 1;

            new Sustituir().SustituirCaracteres_Klee(dialogos);

            try
            {
                while (_FileRead.ReadTrueArchiveBytes(streamReader, out bytes, out Texto, out Datos, out Tipo, out ByteLength))
                {
                    if (Datos)
                    {
                        bool eleccion = false;
                        byte[] line = Tipo switch
                        {
                            "Personaje" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Monologo o Concatenacion" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Titulo" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Logro" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Palabra guia" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Eleccion" => _P_Lines.SetEleccion(NumLine, Texto, bytes, dialogos),
                            "DIALOGOS CONJUNTOS (PERSONAJE)" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "DIALOGOS CONJUNTOS (DIALOGO)" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Acción ???" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            "Información" => _P_Lines.SetLines(NumLine, Texto, Tipo, bytes, dialogos),
                            _ => new byte[] { }
                        };
                        streamWriter.Write(line, 0, line.Length);
                    }
                    else
                        streamWriter.Write(bytes, 0, bytes.Length);

                }


            }
            catch (Exception e)
            {
                MessageBox.Show("Error",e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public byte[] GetBytes(string value)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoderCP932 = Encoding.GetEncoding(932);
            using (MemoryStream stream = new MemoryStream())
            {
                int i = 0;
                while (i < value.Length)
                {
                    var c = value[i];
                    if (c >= 0xF000 && c <= 0xF0FF)
                    {
                        stream.WriteByte((byte)(c - 0xF000));
                    }
                    else
                    {
                        var Cbyte = encoderCP932.GetBytes(new char[] { c });
                        stream.Write(Cbyte, 0, Cbyte.Length);
                    }
                    i++;
                }
                return stream.ToArray();
            }
        }


        public string GetString(string dialog, string oriDialog, out string lenght)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoderANSI = Encoding.GetEncoding(1252);
            Encoding encoderCP932 = Encoding.GetEncoding(932);
            var dialogCP932 = encoderCP932.GetString(encoderANSI.GetBytes(dialog));
            var tamañoOriginal = encoderCP932.GetBytes(oriDialog);
            var tamañoDialog = encoderCP932.GetBytes(dialogCP932);
            string espaciosN = string.Empty;


            if (tamañoDialog.Length < tamañoOriginal.Length)
            {
                int numeroEspacios = (tamañoOriginal.Length - 1) - (tamañoDialog.Length - 1);
                for (int i = 0; i < numeroEspacios; i++)
                {
                    espaciosN += " ";
                }
                lenght = espaciosN;
                return dialogCP932;
            }
            else
            {
                lenght = espaciosN;
                return dialogCP932;
            }

        }


    }
}
