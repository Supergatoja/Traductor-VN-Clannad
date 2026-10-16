using SeenExtractor.Data.Characters;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.RegexConfig;
using System.IO;
using System.Text;

namespace SeenExtractor.Data.Models.P_Lines_Bytes
{
    public class P_Lines
    {
        private ConfigRegex _Regex;
        private readonly Dictionary<string, string> GetCharacteres = Get_Characters.GetCharacteres();
        private Encoding encodingCP932;
        private Encoding encodingANSI;
        internal Dialogos_Conjuntos Conjuntos;

        public P_Lines(ConfigRegex configRegex, Dialogos_Conjuntos dialogos_Conjuntos)
        {
            _Regex = configRegex;
            Conjuntos = dialogos_Conjuntos;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            encodingCP932 = Encoding.GetEncoding(932);
            encodingANSI = Encoding.GetEncoding(1252);
        }
        public List<LinesSeen> GetLines(int NumLine, string Texto, string tipo, int lenght)
        {
            string? Personaje = null;
            bool DC_P;
            var templst = CheckDialogoConjunto(ref NumLine, tipo, out DC_P);
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
            //Conjuntos.Character = string.IsNullOrEmpty(Conjuntos.Character) ? null : Conjuntos.Character;
            //Conjuntos.Dialog = null;
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
                                                                                    Length = encodingCP932.GetBytes(tipo).Length } };
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
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);
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
                //var bytesTextoNuevo = encodingANSI.GetBytes(dialogos[texto]);
                //var bytesTextoNuevoCP932 = encodingCP932.GetString(bytesTextoNuevo);
                var bytesTextoNuevoCP932 = encodingCP932.GetBytes(encodingCP932.GetString(encodingANSI.GetBytes(dialogos[texto])));
                var bytesR = new byte[] { };
                int numEspacios = 0;
                var espacios = string.Empty;

                var nuevoDialogo = dialogos[texto];
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);

                foreach (var Byte in bytes)
                {
                    if (Byte != 0x22)
                        bytesR = bytesR.Append(Byte).ToArray();
                    else
                        break;
                }
                //bytesR = bytesR.Append((byte)0x22).ToArray();
                foreach (var Byte in tempBytes)
                {
                    bytesR = bytesR.Append(Byte).ToArray();
                }
                //bytesR = bytesR.Append((byte)0x22).ToArray();
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
            //Conjuntos.Character = null;
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
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);

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
                var tempBytes = encodingCP932.GetBytes(texto);

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
            var resultado = _Regex.ExtractSelect().Match(texto);
            var Texto = resultado.Groups[1].Value;
            var PersonajeText = _Regex.ExtractPersonajeBytes().Match(texto);
            lengt = (encodingCP932.GetBytes(texto)).Length;
            personaje = PersonajeText.Groups[0].Value;
            personaje = (GetCharacteres.ContainsKey(personaje)) ? GetCharacteres[personaje] : personaje;
            return Texto;
        }

        private byte[] SetPersonaje(string texto, Dictionary<string, string> dialogos)
        {
            var resultado = _Regex.ExtractSelect().Replace(texto, match =>
            {
                string dialogo, espacios = string.Empty;
                int numEspacios = 0;
                if (dialogos.ContainsKey(dialogo = match.Groups[1].Value))
                {
                    var nuevoDialogo = dialogos[dialogo];
                    var lenghtO = encodingCP932.GetBytes(dialogo);
                    var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                    var lenghtNN = encodingCP932.GetBytes(nuevoDialogo);
                    var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                    string dialogos_Espacios;
                    var xd = encodingCP932.GetString(lenghtNN);
                    numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                    for (int i = 0; i < numEspacios; i++)
                        espacios += " ";

                    dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";
                    return dialogos_Espacios;

                }
                return match.Value;
            });

            return encodingCP932.GetBytes(resultado);
        }

        private string GetMoCo(string texto, out int lengt, out string personaje)
        {
            personaje = "Monologo ó Concatenación";
            lengt = (encodingCP932.GetBytes(texto)).Length;
            return texto;
        }

        private string GetAccion(string texto, out int lengt, out string personaje)
        {
            personaje = "Acción ???";
            lengt = (encodingCP932.GetBytes(texto)).Length;
            return texto;
        }

        private string GetInformacion(string texto, out int lengt, out string personaje)
        {
            personaje = "Información";
            lengt = (encodingCP932.GetBytes(texto)).Length;
            return texto;
        }


        private byte[] SetMoCo(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { };

            if (dialogos.ContainsKey(texto))
            {
                var nuevoDialogo = dialogos[texto];
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);

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
            lengt = (encodingCP932.GetBytes(texto)).Length;
            personaje = "Logro";
            return texto;
        }


        private byte[] SetLogro(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            var pedro = encodingCP932.GetBytes($"\"{texto}\"");
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
            lengt = (encodingCP932.GetBytes(texto)).Length;
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
                var nuevoDialogo = dialogos[texto];
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);

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
            lengt = (encodingCP932.GetBytes(texto)).Length;
            personaje = "Titulo";
            return texto;
            //return Texto;
        }


        private byte[] SetTitulo(string texto, Dictionary<string, string> dialogos, byte[] bytes)
        {
            string espacios = string.Empty;
            int numEspacios = 0;
            byte[] resultado = new byte[] { }, bytes_Titulo = new byte[] { 0x00, 0x28 };
            if (dialogos.ContainsKey(texto))
            {
                var nuevoDialogo = dialogos[texto];
                var lenghtO = encodingCP932.GetBytes(texto);
                var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                string dialogos_Espacios;
                numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                for (int i = 0; i < numEspacios; i++)
                    espacios += " ";

                dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encodingCP932 = Encoding.GetEncoding(932);
            var elecciones = texto.Split('∞');
            foreach (var eleccion in elecciones)
            {
                if (!string.IsNullOrEmpty(eleccion))
                {
                    var Bytes = (encodingCP932.GetBytes(eleccion)).Length;

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
                        var nuevoDialogo = dialogos[eleccion];
                        var lenghtO = encodingCP932.GetBytes(eleccion);
                        var lenghtN = encodingANSI.GetBytes(nuevoDialogo);
                        var nuevoDialogo932 = encodingCP932.GetString(lenghtN);
                        string dialogos_Espacios;
                        numEspacios = (lenghtN.Length < lenghtO.Length) ? (lenghtO.Length - 1) - (lenghtN.Length - 1) : 0;

                        for (int i = 0; i < numEspacios; i++)
                            espacios += " ";

                        dialogos_Espacios = $"\"{nuevoDialogo932}{espacios}\"";

                        var tempBytes = encodingCP932.GetBytes(dialogos_Espacios);
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
                            //limite = pos + 4;
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

        internal int Bytes_DC_P() => encodingCP932.GetBytes(Conjuntos.Character).Length;

    }

    public class Dialogos_Conjuntos
    {
        public string? Character { get; set; }

        public string? Dialog { get; set; }
        public byte[]? Bytes { get; set; }

    }

}
