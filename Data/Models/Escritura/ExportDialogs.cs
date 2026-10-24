using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using System.IO;
namespace SeenExtractor.Data.Models.Escritura
{
    public class ExportDialogs
    {
        public List<LinesSeen>? LinesSeens { get; set; }
        public List<LinesSeen>? OLinesSeens { get; set; }
        private static readonly string numLine = "Linea ";
        public async Task<Dictionary<bool, string>> SaveFileFialog(string Patch, bool type)
        {
            string nameFileByte = Patch.Replace(".txt", " Bytes.txt");
            string msjChat = "Gracias, aquí esta el siguiente bloque:";
            try
            {
                using (FileStream dsFile = new(Patch, FileMode.Create, FileAccess.Write))
                using (FileStream dsFileBytes = new(nameFileByte, FileMode.Create, FileAccess.Write))
                {
                    int i = 1, numBloques = 1;
                    foreach (var dialog in LinesSeens)
                    {
                        if (type)
                        {
                            GetCharacterOriginal(dialog.Line, OLinesSeens, out string Character);
                            if (i < 61)
                            {
                                var dialogoCP392 = GetString(dialog.Dialog, (OLinesSeens[dialog.Line - 1]).Dialog, out int length, out int Olength);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392) + "\n", out int ByteLength);
                                var bytesLineBytes = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392, $"({length}/{Olength})") + "\n", out int ByteLengthBytes);
                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                            }
                            else
                            {
                                var dialogoCP392 = GetString(dialog.Dialog, (OLinesSeens[dialog.Line - 1]).Dialog, out int length, out int Olength);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392) + "\n", out int ByteLength);
                                var bytesLineBytes = GetStreamWrite($"\n{msjChat}\n\n" + string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", dialogoCP392, $"({length}/{Olength})") + "\n", out int ByteLengthBytes);
                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                                i = 1;
                                numBloques++;
                            }
                        }
                        else
                        {
                            GetCharacterOriginal(dialog.Line, LinesSeens, out string Character);
                            if (i < 61)
                            {
                                var Dialog = GetString(dialog.Dialog, out int length);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog) + "\n", out int ByteLength);
                                var bytesLineBytes = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog, $"({length}/{length})") + "\n", out int ByteLengthBytes);
                                await Task.WhenAll(
                                dsFile.WriteAsync(bytesLine, 0, ByteLength),
                                dsFileBytes.WriteAsync(bytesLineBytes, 0, ByteLengthBytes)
                                );
                            }
                            else
                            {
                                var Dialog = GetString(dialog.Dialog, out int length);
                                var bytesLine = GetStreamWrite(string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog) + "\n", out int ByteLength);
                                var bytesLineBytes = GetStreamWrite($"\n{msjChat}\n\n" + string.Join(" ", numLine + $"{dialog.Line} ( {Character} ):", Dialog, $"({length}/{length})") + "\n", out int ByteLengthBytes);
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
                    var NumBloquesBytes = GetStreamWrite($"\nNumero de bloques: {numBloques}\n", out int NumBloquesBytesLength);
                    dsFileBytes.Write(NumBloquesBytes, 0, NumBloquesBytesLength);
                }
                return new() { { true, "Guardado con exito" } };
            }
            catch (Exception e)
            {
                return new() { { true, string.Join(" ", "Ocurrio un error", e.Message) } };
            }
        }
        public static string GetString(string dialog, string Odialog, out int length, out int Olength)
        {
            length = dialog.ConvertToCP932().LengthBytesCP932();
            Olength = Odialog.LengthBytesCP932();
            return dialog;
        }
        public static string GetString(string dialog, out int length)
        {
            length = dialog.LengthBytesCP932();
            return dialog.ConvertToANSII();
        }
        public static byte[] GetBytes(string value)
        {
            using var stream = new MemoryStream();
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
        public static byte[] GetStreamWrite(string line, out int lenght)
        {
            var ByteLine = line.BytesANSII();
            lenght = ByteLine.Length;
            return ByteLine;
        }
        public static void GetCharacterOriginal(int line, List<LinesSeen> lines, out string Character) => Character = (lines.FirstOrDefault(d => d.Line == line)).Character.ConvertToANSII();
    }
}
