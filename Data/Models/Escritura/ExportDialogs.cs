using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using System.IO;
using System.Text;

namespace SeenExtractor.Data.Models.Escritura
{
    public class ExportDialogs
    {
        public List<LinesSeen> LinesSeens { get; set; }
        public List<LinesSeen> OLinesSeens { get; set; }
        private PickFile _File;
        private static string numLine = "Linea ";
        private Encoding encoderANSI;
        private Encoding encoderCP932;
        public ExportDialogs(PickFile file)
        {
            encoderANSI = Encoding.GetEncoding(1252);
            encoderCP932 = Encoding.GetEncoding(932);
            _File = file;
        }
        public async Task<Dictionary<bool, string>> SaveFileFialog(string Patch, bool type)
        {
            string nameFileByte = Patch.Replace(".txt", " Bytes.txt");
            string msjChat = "Gracias, aquí esta el siguiente bloque:";

            try
            {
                using (FileStream dsFile = new(Patch, FileMode.Create, FileAccess.Write))
                using (FileStream dsFileBytes = new(nameFileByte, FileMode.Create, FileAccess.Write))
                {
                    int length, Olength, ByteLength, ByteLengthBytes, NumBloquesBytesLength;
                    int i = 1;
                    int numBloques = 1;
                    string Character;
                    foreach (var dialog in LinesSeens)
                    {
                        if (type)
                            GetCharacterOriginal(dialog.Line, OLinesSeens, out Character);
                        else
                            GetCharacterOriginal(dialog.Line, LinesSeens, out Character);

                        if (i < 61)
                        {
                            if (type)
                            {
                                var dialogoCP392 = GetString(dialog.Dialog, (OLinesSeens[dialog.Line - 1]).Dialog, out length, out Olength);

                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392) + "\n", out ByteLength);
                                var bytesLineBytes = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392, $"({length}/{Olength})") + "\n", out ByteLengthBytes);

                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                            }
                            else
                            {
                                var Dialog = GetString(dialog.Dialog, out length);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog) + "\n", out ByteLength);
                                var bytesLineBytes = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog, $"({length}/{length})") + "\n", out ByteLengthBytes);

                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                            }
                        }
                        else
                        {
                            if (type)
                            {
                                var dialogoCP392 = GetString(dialog.Dialog, (OLinesSeens[dialog.Line - 1]).Dialog, out length, out Olength);

                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392) + "\n", out ByteLength);
                                var bytesLineBytes = GetStreamWrite($"\n{msjChat}\n\n" + string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392, $"({length}/{Olength})") + "\n", out ByteLengthBytes);

                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                                i = 1;
                                numBloques++;
                            }
                            else
                            {
                                var Dialog = GetString(dialog.Dialog, out length);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog) + "\n", out ByteLength);
                                var bytesLineBytes = GetStreamWrite($"\n{msjChat}\n\n" + string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog, $"({length}/{length})") + "\n", out ByteLengthBytes);

                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                                i = 1;
                                numBloques++;
                            }
                        }
                        i++;
                    }

                    var NumBloquesBytes = GetStreamWrite($"\nNumero de bloques: {numBloques}\n", out NumBloquesBytesLength);
                    dsFileBytes.Write(NumBloquesBytes, 0, NumBloquesBytesLength);

                }

                return new() { { true, "Guardado con exito" } };
            }
            catch (Exception e)
            {
                return new() { { true, "Ocurrio un error" } };
            }


        }

        public string GetString(string dialog, string Odialog, out int length, out int Olength)
        {
            var dialogCP932 = encoderCP932.GetString(encoderANSI.GetBytes(dialog));
            var tamañoOriginal = encoderCP932.GetBytes(Odialog);
            var tamañoDialog = encoderCP932.GetBytes(dialogCP932);
            length = tamañoDialog.Length;
            Olength = tamañoOriginal.Length;
            return encoderANSI.GetString(encoderANSI.GetBytes(dialog));

        }

        public string GetString(string dialog, out int length)
        {
            var tamañoDialog = encoderCP932.GetBytes(dialog);
            length = tamañoDialog.Length;
            return encoderANSI.GetString(tamañoDialog);
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


        public byte[] GetStreamWrite(string line, out int lenght)
        {
            //ConvertANSI(line, out line);
            //var ByteLine = GetBytes(line);
            var ByteLine = ConvertANSI(line);
            lenght = ByteLine.Length;
            return ByteLine;
        }

        public byte[] ConvertANSI(string line)
        {
            return encoderANSI.GetBytes(line);
        }

        public void GetCharacterOriginal(int line, List<LinesSeen> lines, out string Character) => Character = encoderANSI.GetString(encoderCP932.GetBytes((lines.FirstOrDefault(d => d.Line == line)).Character));


    }
}
