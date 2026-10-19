using SeenExtractor.Data.Characters;
using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;

namespace SeenExtractor.Data.Models.P_Lines_Bytes
{
    public class P_Lines
    {
        private Dictionary<string, string> GetCharacteres = Get_Characters.GetCharacteres();
        internal Dialogos_Conjuntos Conjuntos;

        public P_Lines(Dialogos_Conjuntos dialogos_Conjuntos) => Conjuntos = dialogos_Conjuntos;

        public List<LinesSeen> GetLines(int NumLine, string Texto, string tipo, int lenght)
        {
            string? Personaje = null;
            var templst = CheckDialogoConjunto(ref NumLine, tipo, out bool DC_P);
            string Dialogo = tipo switch
            {
                "Personaje" => GetPersonaje(Texto, out lenght, out Personaje),
                "Monologo o Concatenacion" => GetMoCo(Texto, out lenght, out Personaje),
                "Titulo" => GetTitulo(Texto, out lenght, out Personaje),
                "Logro" => GetLogro(Texto, out lenght, out Personaje),
                "Palabra guia" => GetGuia(Texto, out lenght, out Personaje),
                "DIALOGOS CONJUNTOS (PERSONAJE)" => GetMulti(Texto, 0, out Personaje),
                "DIALOGOS CONJUNTOS (DIALOGO)" => GetMulti(Texto, 1, out Personaje),
                "Acción ???" => GetAccion(Texto, out lenght, out Personaje),
                "Información" => GetInformacion(Texto, out lenght, out Personaje),
                _ => string.Empty
            };

            if (DC_P)
                templst.Add(new LinesSeen()
                {
                    Line = NumLine,
                    Character = Personaje,
                    Dialog = Dialogo,
                    Length = lenght
                });
            return DC_P ? templst : new List<LinesSeen>() { new LinesSeen() { Line = NumLine,
                                                                                                                   Character = Personaje,
                                                                                                                   Dialog = Dialogo,
                                                                                                                   Length = lenght } };
        }
        public byte[] SetLines(int NumLine, string Texto, string tipo, byte[] bytes, Dictionary<string, string> dialogos)
        {
            var Dialogo = tipo switch
            {
                "Personaje" => SetPersonaje(Texto, dialogos),
                "Monologo o Concatenacion" => SetMoCo(Texto, dialogos, bytes),
                "Titulo" => SetTitulo(Texto, dialogos, bytes),
                "Logro" => SetLogro(Texto, dialogos, bytes),
                "Palabra guia" => SetGuia(Texto, dialogos, bytes),
                "DIALOGOS CONJUNTOS (PERSONAJE)" => SetMulti(Texto, dialogos, bytes),
                "DIALOGOS CONJUNTOS (DIALOGO)" => SetMulti(Texto, dialogos, bytes),
                "Acción ???" => SetAccion(Texto, dialogos, bytes),
                "Información" => SetInformacion(Texto, dialogos, bytes),
                _ => new byte[] { }
            };
            return Dialogo;
        }


        private List<LinesSeen> CheckDialogoConjunto(ref int NumLine, string tipo, out bool data)
        {
            if (!string.IsNullOrEmpty(Conjuntos.Character) && tipo != "DIALOGOS CONJUNTOS (DIALOGO)")
            {
                if (Conjuntos.Character != "【】")
                {
                    var lst = new List<LinesSeen>() { new LinesSeen() { Line = NumLine,
                                                                                    Character = "TiTULO ???",
                                                                                    Dialog = Conjuntos.Character,
                                                                                    Length = tipo.LengthBytesCP932() } };
                    NumLine++;
                    data = true;
                    Conjuntos.Clean();
                    return lst;
                }else
                { Conjuntos.Clean(); data = false; return null; }
            }
            else { data = false; return null; }
        }

        private byte[] SetAccion(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            if (dialogos.ContainsKey(texto))
            {
                var bytesR = new byte[] { };
                int numEspacios = 0;
                var espacios = string.Empty;

                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();
                var countTemp = 0;
                foreach (var Byte in bytes)
                {
                    if (Byte != 0x22)
                        {bytesR = bytesR.Append(Byte).ToArray(); countTemp++; }
                    else
                        break;
                }
                foreach (var Byte in tempBytes)
                {
                    bytesR = bytesR.Append(Byte).ToArray();
                }
                countTemp += 2;
                for (int i = countTemp; countTemp < bytes.Length; countTemp++)
                {
                    bytesR = bytesR.Append(bytes[countTemp]).ToArray();
                }

                return bytesR;
            }
            else
                return bytes;
        }

        private byte[] SetInformacion(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            if (dialogos.ContainsKey(texto))
            {

                var bytesR = new byte[] { };
                int numEspacios = 0;
                var espacios = string.Empty;

                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;

                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                foreach (var Byte in bytes)
                {
                    if (Byte != 0x22)
                        bytesR = bytesR.Append(Byte).ToArray();
                    else
                        break;
                }

                foreach (var Byte in tempBytes)
                {
                    bytesR = bytesR.Append(Byte).ToArray();
                }

                return bytesR;
            }
            else
                return bytes;
        }

        private string GetMulti(string Texto, int Type, out string Personaje)
        {
            var data = Type switch
            {
                0 => CheckCharacter(Texto),
                1 => Conjuntos.Dialog = Texto,
                _ => string.Empty
            };

            if (Conjuntos.Dialog == null )
            {
                Personaje = string.Empty;
                return data;
            }
            else
            {
                if (Conjuntos.Character == null)
                    Personaje = "Elección en pantalla";
                else
                    Personaje = Conjuntos.Character;
                Conjuntos.Dialog = null;
                Conjuntos.Character = null;
                return data;
            }
        }

        private byte[] SetMulti(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { };

            if (dialogos.ContainsKey(texto))
            {
                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                resultado = resultado.Append(bytes[0]).ToArray();

                foreach (var item in tempBytes)
                {
                    resultado = resultado.Append(item).ToArray();
                }

                for (int i = bytes.Length - 2; i < bytes.Length; i++)
                {
                    resultado = resultado.Append(bytes[i]).ToArray();
                }

            }
            else
            {
                var tempBytes = texto.BytesCP932();

                resultado = resultado.Append(bytes[0]).ToArray();
                resultado = resultado.Append((byte)0x22).ToArray();

                foreach (var item in tempBytes)
                {
                    resultado = resultado.Append(item).ToArray();
                }

                resultado = resultado.Append((byte)0x22).ToArray();

                for (int i = bytes.Length - 2; i < bytes.Length; i++)
                {
                    resultado = resultado.Append(bytes[i]).ToArray();
                }
            }

            return resultado;
        }

        private string GetPersonaje(string texto, out int lengt, out string personaje)
        {
            var resultado = ConfigRegex.ExtractSelect().Match(texto);
            var Texto = resultado.Groups[1].Value;
            var PersonajeText = ConfigRegex.ExtractPersonajeBytes().Match(texto);
            lengt = texto.LengthBytesCP932();
            personaje = PersonajeText.Groups[0].Value;
            personaje = (GetCharacteres.ContainsKey(personaje)) ? GetCharacteres[personaje] : personaje;
            return Texto;
        }

        private byte[] SetPersonaje(string texto, Dictionary<string, string> dialogos)
        {
            var resultado = ConfigRegex.ExtractSelect().Replace(texto, match =>
            {
                string dialogo, espacios = string.Empty;
                int numEspacios = 0;
                if (dialogos.ContainsKey(dialogo = match.Groups[1].Value))
                {
                    var nuevoDialogo = dialogos[dialogo];
                    var lenghtO = dialogo.LengthBytesCP932();
                    var lenghtN = nuevoDialogo.LengthBytesANSII();
                    var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                    string dialogos_Espacios;
                    numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                    for (int i = 0; i < numEspacios; i++)
                        espacios += " ";

                    dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";
                    return dialogos_Espacios;

                }
                return match.Value;
            });

            return resultado.BytesCP932();
        }

        private string GetMoCo(string texto, out int lengt, out string personaje)
        {
            personaje = "Monologo ó Concatenación";
            lengt = texto.LengthBytesCP932();
            return texto;
        }

        private string GetAccion(string texto, out int lengt, out string personaje)
        {
            personaje = "Acción ???";
            lengt = texto.LengthBytesCP932();
            return texto;
        }

        private string GetInformacion(string texto, out int lengt, out string personaje)
        {
            personaje = "Información";
            lengt = texto.LengthBytesCP932();
            return texto;
        }


        private byte[] SetMoCo(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { };

            if (dialogos.ContainsKey(texto))
            {
                var bytesR = new byte[] { };
                
                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == 0x22 && bytes[i + 1] == 0x22)
                        break;
                    resultado = resultado.Append(bytes[i]).ToArray();
                }
                foreach (var item in tempBytes)
                {
                    resultado = resultado.Append(item).ToArray();
                }
            }

            return resultado;
        }

        private string GetLogro(string texto, out int lengt, out string personaje)
        {
            lengt = texto.LengthBytesCP932();
            personaje = "Logro";
            return texto;
        }


        private byte[] SetLogro(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            var pedro = $"\"{texto}\"".BytesCP932();
            var Logro = new MemoryStream();
            for (int i = 0; i < 3; i++)
            {
                Logro.WriteByte(bytes[i]);
            }

            foreach (var item in pedro)
            {
                Logro.WriteByte(item);
            }
            Logro.WriteByte(bytes[bytes.Length - 1]);

            return Logro.ToArray();
        }

        private string GetGuia(string texto, out int lengt, out string personaje)
        {
            lengt = texto.LengthBytesCP932();
            personaje = "Palabra Guia";
            return texto;
        }


        private byte[] SetGuia(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { };
            if (dialogos.ContainsKey(texto))
            {
                var bytesR = new byte[] { };
                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                for (int i = 0; i < 3; i++)
                {
                    resultado = resultado.Append(bytes[i]).ToArray();
                }
                foreach (var item in tempBytes)
                {
                    resultado = resultado.Append(item).ToArray();
                }
                resultado = resultado.Append((byte)0x29).ToArray();
            }

            return resultado;
        }

        private string GetTitulo(string texto, out int lengt, out string personaje)
        {
            lengt = texto.LengthBytesCP932();
            personaje = "Titulo";
            return texto;
        }


        private byte[] SetTitulo(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { }, bytes_Titulo = new byte[] { 0x00, 0x28 };
            if (dialogos.ContainsKey(texto))
            {
                var bytesR = new byte[] { };
                var nuevoDialogo = dialogos[texto];
                var lenghtO = texto.LengthBytesCP932();
                var lenghtN = nuevoDialogo.LengthBytesANSII();
                var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                string dialogos_Espacios;
                numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = dialogos_Espacios.BytesCP932();

                foreach (var item in bytes_Titulo)
                {
                    resultado = resultado.Append(item).ToArray();
                }
                foreach (var item in tempBytes)
                {
                    resultado = resultado.Append(item).ToArray();
                }
                resultado = resultado.Append((byte)0x29).ToArray();
            }

            return resultado;
        }

        public List<LinesSeen> GetEleccion(int NumLine, string texto)
        {
            var lines = new List<LinesSeen>();
            var elecciones = texto.Split('∞');
            foreach (var eleccion in elecciones)
            {
                if (!string.IsNullOrEmpty(eleccion))
                {
                    var Bytes = eleccion.LengthBytesCP932();

                    lines.Add(new LinesSeen() { Line = NumLine, Character = "Elección", Dialog = eleccion, Length = Bytes });
                    NumLine++;
                }
            }

            return lines;
        }

        public byte[] SetEleccion(int NumLine, string texto, byte[] Data, Dictionary<string, string> dialogos)
        {
            var elecciones = texto.Split('∞');
            var numElecciones = elecciones.Length - 1;
            var count = 1;
            int pos = 0, limite = 0;
            byte[] resultado = new byte[] { };
            foreach (var eleccion in elecciones)
            {
                if (!string.IsNullOrEmpty(eleccion))
                {
                    string espacios = string.Empty;
                    int numEspacios = 0;
                    if (dialogos.ContainsKey(eleccion))
                    {
                        var bytesR = new byte[] { };
                        var nuevoDialogo = dialogos[eleccion];
                        var lenghtO = eleccion.LengthBytesCP932();
                        var lenghtN = nuevoDialogo.LengthBytesANSII();
                        var nuevoDialogo932 = nuevoDialogo.ConvertToCP932();
                        string dialogos_Espacios;
                        numEspacios = (lenghtN < lenghtO) ? (lenghtO - 1) - (lenghtN - 1) : 0;

                        for (int i = 0; i < numEspacios; i++)
                            espacios += " ";

                        dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                        var tempBytes = dialogos_Espacios.BytesCP932();
                        if (count == 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                resultado = resultado.Append(Data[i]).ToArray();
                                pos++;
                            }
                            pos += 2;
                        }
                        else pos += 2;

                        foreach (var item in tempBytes)
                        {
                            resultado = resultado.Append(item).ToArray();
                        }
                        if (!(count == numElecciones))
                        {
                            var posn = pos + 0;
                            limite = pos + 3;
                            for (int i = posn; i < limite; i++)
                            {
                                resultado = resultado.Append(Data[i]).ToArray();
                                pos++;
                            }
                        }
                        else
                        {
                            var posn = pos + 0;
                            for (int i = posn; i < Data.Length; i++)
                            {
                                resultado = resultado.Append(Data[i]).ToArray();
                            }
                        }
                    }

                    count++;

                }
            }

            return resultado;
        }

        private string CheckCharacter(string value)
        {
            var Texto = string.IsNullOrEmpty(value) ? "【】" : value;
            Conjuntos.Character = Texto;
            return Texto;
        }

        internal int Bytes_DC_P() => Conjuntos.Character.LengthBytesCP932();

    }

    public class Dialogos_Conjuntos
    {
        public string? Character { get; set; }

        public string? Dialog { get; set; }
        public byte[]? Bytes { get; set; }

    }

}
