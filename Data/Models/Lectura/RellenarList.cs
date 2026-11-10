using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Extenciones.StreamWriter;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;
using SeenExtractor.Data.Models.Sustitución;
using System.IO;
using System.Windows;
namespace SeenExtractor.Data.Models.Lectura
{
    public class RellenarList(StreamFile streamFile)
    {
        public List<LinesSeen> UseRegexComillasCP932ToList(Stream streamReader)
        {
            var linesSeen = new List<LinesSeen>();
            int NumLine = 1;
            try
            {
                while (streamFile.ReadTrueArchiveBytes(streamReader, out byte[] bytes, out string Texto, out bool Datos, out TipoDialogo Tipo, out int ByteLength))
                {
                    if (Datos)
                    {
                        var line = Tipo.ToString() switch
                        {
                            nameof(TipoDialogo.Characters) => GetDialogs.GetPersonaje(ref NumLine, Texto),
                            nameof(TipoDialogo.MonologoConcat) => GetDialogs.GetMoCo(ref NumLine, Texto),
                            nameof(TipoDialogo.TItulo) => GetDialogs.GetTitulo(ref NumLine, Texto),
                            nameof(TipoDialogo.Logro) => GetDialogs.GetLogro(ref NumLine, Texto),
                            nameof(TipoDialogo.PlabraGuia) => GetDialogs.GetGuia(ref NumLine, Texto),
                            nameof(TipoDialogo.Eleccion) => GetDialogs.GetEleccion(ref NumLine, Texto),
                            nameof(TipoDialogo.DialogoConjunto) => GetDialogs.GetMulti(ref NumLine, Texto, 1),
                            nameof(TipoDialogo.DialogoConjuntoPersonaje) => GetDialogs.GetMulti(ref NumLine, Texto, 0),
                            nameof(TipoDialogo.Accion) => GetDialogs.GetAccion(ref NumLine, Texto),
                            nameof(TipoDialogo.Informacion) => GetDialogs.GetInformacion(ref NumLine, Texto),
                            _ => []
                        };
                        if (!string.IsNullOrEmpty(line[0].Character))
                        {
                            linesSeen.AddRange(line);
                            NumLine++;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(GetDialogs.Conjuntos.Character))
                {
                    linesSeen.Add(new LinesSeen()
                    {
                        Line = NumLine,
                        Character = "TiTULO ???",
                        Dialog = GetDialogs.Conjuntos.Character,
                        Length = GetDialogs.Conjuntos.Character.LengthBytesCP932()
                    });
                }
                GetDialogs.Conjuntos.Clean();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error",e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return linesSeen;
        }
        public async Task UseRegexComillasCP932ToMerch(Stream streamReader, Stream streamWriter, Dictionary<string, string> dialogos)
        {
            dialogos.SustituirCaracteres_Klee();
            try
            {
                while (streamFile.ReadTrueArchiveBytes(streamReader, out byte[] bytes, out string Texto, out bool Datos, out TipoDialogo Tipo, out int ByteLength))
                {
                    if (Datos)
                    {
                        var tmp = Texto;
                        byte[] line = Tipo.ToString() switch
                        {
                            nameof(TipoDialogo.Characters) => SetDialogs.SetPersonaje(Texto, dialogos),
                            nameof(TipoDialogo.MonologoConcat) => SetDialogs.SetMoCo(Texto, dialogos, bytes),
                            nameof(TipoDialogo.TItulo) => SetDialogs.SetTitulo(Texto, dialogos),
                            nameof(TipoDialogo.Logro) => SetDialogs.SetLogro(Texto, bytes),
                            nameof(TipoDialogo.PlabraGuia) => SetDialogs.SetGuia(Texto, dialogos, bytes),
                            nameof(TipoDialogo.Eleccion) => SetDialogs.SetEleccion(Texto, bytes, dialogos),
                            nameof(TipoDialogo.DialogoConjuntoPersonaje) => SetDialogs.SetMulti(Texto, dialogos, bytes),
                            nameof(TipoDialogo.DialogoConjunto) => SetDialogs.SetMulti(Texto, dialogos, bytes),
                            nameof(TipoDialogo.Accion) => SetDialogs.SetAccion(Texto, dialogos, bytes),
                            nameof(TipoDialogo.Informacion) => SetDialogs.SetInformacion(Texto, dialogos, bytes),
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
    }
}
