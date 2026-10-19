using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;
using SeenExtractor.Data.Models.Sustitución;
using System.IO;
using System.Windows;

namespace SeenExtractor.Data.Models.Lectura
{
    public class RellenarList
    {
        private StreamFile _FileRead;
        private P_Lines _P_Lines;

        public RellenarList(StreamFile streamFile, P_Lines p_Lines)
        {
            _FileRead = streamFile;
            _P_Lines = p_Lines;
        }

        public List<LinesSeen> UseRegexComillasCP932ToList(Stream streamReader)
        {
            var linesSeen = new List<LinesSeen>();
            int NumLine = 1;

            try
            {

                while (_FileRead.ReadTrueArchiveBytes(streamReader, out byte[] bytes, out string Texto, out bool Datos, out string Tipo, out int ByteLength))
                {
                    if (Datos)
                    {
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
                            _ => []
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
            int NumLine = 1;

            dialogos.SustituirCaracteres_Klee();

            try
            {
                while (_FileRead.ReadTrueArchiveBytes(streamReader, out byte[] bytes, out string Texto, out bool Datos, out string Tipo, out int ByteLength))
                {
                    if (Datos)
                    {
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
                            _ => []
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
                        var Cbyte = new char[] { c }.BytesCP932();
                        stream.Write(Cbyte, 0, Cbyte.Length);
                    }
                    i++;
                }
                return stream.ToArray();
            }
        }


        public string GetString(string dialog, string oriDialog, out string lenght)
        {
            var dialogCP932 = dialog.ConvertToCP932();
            var tamañoOriginal = oriDialog.LengthBytesCP932();
            var tamañoDialog = dialogCP932.LengthBytesCP932();
            var espaciosN = string.Empty;


            if (tamañoDialog < tamañoOriginal)
            {
                int numeroEspacios = (tamañoOriginal - 1) - (tamañoDialog - 1);
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
