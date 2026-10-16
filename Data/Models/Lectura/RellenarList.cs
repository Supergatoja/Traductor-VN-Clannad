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

        public RellenarList(ConfigRegex configRegex, StreamFile streamFile, P_Lines p_Lines)
        {
            _Regex = configRegex;
            _FileRead = streamFile;
            _P_Lines = p_Lines;
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
