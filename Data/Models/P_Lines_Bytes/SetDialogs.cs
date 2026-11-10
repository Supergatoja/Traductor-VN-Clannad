using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;

namespace SeenExtractor.Data.Models.P_Lines_Bytes
{
    public static class SetDialogs
    {
        public static byte[] SetAccion(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {
                var bytesR = Array.Empty<byte>();

                var espacios = string.Empty;
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();
                var countTemp = 0;
                foreach (var Byte in bytes)
                {
                    if (Byte != 0x22)
                    {
                        bytesR = [.. bytesR, Byte];
                        countTemp++;
                    }
                    else
                        break;
                }
                foreach (var Byte in tempBytes)
                    bytesR = [.. bytesR, Byte];
                countTemp += 2;
                for (; countTemp < bytes.Length; countTemp++)
                    bytesR = [.. bytesR, bytes[countTemp]];

                return bytesR;
            }
            else
                return bytes;
        }

        public static byte[] SetInformacion(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {

                var bytesR = Array.Empty<byte>();

                var espacios = string.Empty;
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;

                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                foreach (var Byte in bytes)
                {
                    if (Byte != 0x22)
                        bytesR = [.. bytesR, Byte];
                    else
                        break;
                }

                foreach (var Byte in tempBytes)
                    bytesR = [.. bytesR, Byte];

                return bytesR;
            }
            else
                return bytes;
        }

        public static byte[] SetMulti(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;

            byte[] resultado = [];

            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                resultado = [.. resultado, bytes[0]];

                foreach (var item in tempBytes)
                    resultado = [.. resultado, item];

                for (int i = bytes.Length - 2; i < bytes.Length; i++)
                    resultado = [.. resultado, bytes[i]];

            }
            else
            {
                var tempBytes = texto.BytesCP932();

                resultado = [.. resultado, bytes[0]];
                resultado = [.. resultado, (byte)0x22];

                foreach (var item in tempBytes)
                    resultado = [.. resultado, item];

                resultado = [.. resultado, (byte)0x22];

                for (int i = bytes.Length - 2; i < bytes.Length; i++)
                    resultado = [.. resultado, bytes[i]];
            }

            return resultado;
        }

        public static byte[] SetPersonaje(string texto, Dictionary<string, string> dialogos)
        {
            var resultado = ConfigRegex.ExtractSelect().Replace(texto, match =>
            {
                string dialogo, espacios = string.Empty;

                if (dialogos.ContainsKey(dialogo = match.Groups[1].Value))
                {
                    var nuevoDialogo = dialogos[dialogo];
                    var lenghtO = dialogo.LengthBytesCP932();
                    var lenghtN = nuevoDialogo.LengthBytesANSII();
                    var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                    string dialogos_Espacios;
                    int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                    for (int i = 0; i < numEspacios; i++)
                        espacios += " ";

                    dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";
                    return dialogos_Espacios;

                }
                return match.Value;
            });

            return resultado.BytesCP932();
        }

        public static byte[] SetMoCo(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;

            byte[] resultado = [];

            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == 0x22 && bytes[i + 1] == 0x22)
                        break;
                    resultado = [.. resultado, bytes[i]];
                }
                foreach (var item in tempBytes)
                    resultado = [.. resultado, item];
            }

            return resultado;
        }

        public static byte[] SetLogro(string texto, byte[] bytes)
        {
            var pedro = $"\"{texto}\"".BytesCP932();
            var Logro = new MemoryStream();
            for (int i = 0; i < 3; i++)
                Logro.WriteByte(bytes[i]);

            foreach (var item in pedro)
                Logro.WriteByte(item);
            Logro.WriteByte(bytes[^1]);

            return Logro.ToArray();
        }

        public static byte[] SetGuia(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;

            byte[] resultado = [];
            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                for (int i = 0; i < 3; i++)
                    resultado = [.. resultado, bytes[i]];
                foreach (var item in tempBytes)
                    resultado = [.. resultado, item];
                resultado = [.. resultado, (byte)0x29];
            }

            return resultado;
        }

        public static byte[] SetTitulo(string texto, Dictionary<string, string> dialogos)
        {
            string espacios = string.Empty;

            byte[] resultado = ""u8.ToArray(), bytes_Titulo = [0x00, 0x28];
            if (dialogos.TryGetValue(texto, out string? nuevoDialogo))
            {
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                foreach (var item in bytes_Titulo)
                    resultado = [.. resultado, item];
                foreach (var item in tempBytes)
                    resultado = [.. resultado, item];
                resultado = [.. resultado, (byte)0x29];
            }

            return resultado;
        }

        public static byte[] SetEleccion(string texto, byte[] Data, Dictionary<string, string> dialogos)
        {
            var elecciones = texto.Split('∞');
            var numElecciones = elecciones.Length - 1;
            var count = 1;
            int pos = 0;
            byte[] resultado = [];
            foreach (var eleccion in elecciones)
            {
                if (!string.IsNullOrEmpty(eleccion))
                {
                    string espacios = string.Empty;

                    if (dialogos.TryGetValue(eleccion, out string? nuevoDialogo))
                    {
                        var bytesR = Array.Empty<byte>();
                        var lenghtO = eleccion.LengthBytesCP932();
                        var lenghtN = nuevoDialogo.LengthBytesANSII();
                        var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                        string dialogos_Espacios;
                        int numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                        for (int i = 0; i < numEspacios; i++)
                            espacios += " ";

                        dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                        var tempBytes = dialogos_Espacios.BytesCP932();
                        if (count == 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                resultado = [.. resultado, Data[i]];
                                pos++;
                            }
                            pos += 2;
                        }
                        else pos += 2;

                        foreach (var item in tempBytes)
                            resultado = [.. resultado, item];
                        if (!(count == numElecciones))
                        {
                            var posn = pos + 0;
                            int limite = pos + 3;
                            for (int i = posn; i < limite; i++)
                            {
                                resultado = [.. resultado, Data[i]];
                                pos++;
                            }
                        }
                        else
                        {
                            var posn = pos + 0;
                            for (int i = posn; i < Data.Length; i++)
                                resultado = [.. resultado, Data[i]];
                        }
                    }

                    count++;

                }
            }

            return resultado;
        }

    }
}
