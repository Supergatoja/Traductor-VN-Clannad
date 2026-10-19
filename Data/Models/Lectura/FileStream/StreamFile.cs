using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using System.IO;

namespace SeenExtractor.Data.Models.Lectura.ReadFileStream
{
    public class StreamFile
    {
        public bool ReadTrueArchiveBytes(Stream file, out byte[] bytes, out string Text, out bool Content, out string Type, out int lenght)
        {
            if (file.Position >= file.Length)
            {
                bytes = null;
                Content = false;
                Text = string.Empty;
                Type = string.Empty;
                lenght = 0;
                return false;
            }
            
            byte ReadByte, nextByte, currentByte, nextByteC;
            byte[] Bytes = new byte[1];
            int ReadInt, CountAfterArroba = 0;
            bool LEFT_BLACK_LENTICULAR_BRACKET, RIGHT_BLACK_LENTICULAR_BRACKET, ComillaInicio, ComillaEscapada, Arroba = false, Parentesis_Apertura;

            while ((ReadInt = file.ReadByte()) != -1)
            {
                ReadByte = (byte)ReadInt;
                //DIALOGO CON PERSONAJE
                if (ReadByte == 0x81)
                {
                    if (LEFT_BLACK_LENTICULAR_BRACKET = ((nextByte = (byte)file.ReadByte()) == 0x79))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stream.WriteByte(ReadByte);
                            stream.WriteByte(nextByte);
                            while ((ReadInt = file.ReadByte()) != -1)
                            {
                                currentByte = (byte)ReadInt;

                                if (currentByte == 0x81)
                                {
                                    if (RIGHT_BLACK_LENTICULAR_BRACKET = ((nextByte = (byte)file.ReadByte()) == 0x7A))
                                    {
                                        if (!(ComillaInicio = ((nextByteC = (byte)file.ReadByte()) == 0x22)))
                                        {
                                            var bytesStream = new byte[] { currentByte, nextByte };
                                            file.Position--;
                                            stream.Write(bytesStream, 0, bytesStream.Length);
                                            Text = string.Empty;
                                            bytes = stream.ToArray();
                                            Content = false;
                                            Type = string.Empty;
                                            lenght = 0;
                                            return true;
                                        }
                                        else
                                        {
                                            var bytesStream = new byte[] { currentByte, nextByte, nextByteC };
                                            stream.Write(bytesStream, 0, bytesStream.Length);
                                        }
                                    }
                                    else
                                    {
                                        stream.WriteByte(currentByte);
                                        stream.WriteByte(nextByte);
                                    }

                                }
                                else
                                {

                                    if (currentByte == 0x5C)
                                    {
                                        if (ComillaEscapada = ((nextByte = (byte)file.ReadByte()) == 0x22))
                                        {
                                            var bytesEscapado = new byte[] { currentByte, nextByte };
                                            stream.Write(bytesEscapado, 0, bytesEscapado.Length);
                                        }
                                        else
                                        {
                                            stream.WriteByte(currentByte);
                                            file.Position--;
                                        }

                                    }
                                    else
                                    {

                                        if (currentByte == 0x22)
                                        {
                                            stream.WriteByte(currentByte);
                                            var ByteArreglo = stream.ToArray();
                                            var textoCP932 = ByteArreglo.ToStringCP932();
                                            Text = textoCP932;
                                            bytes = ByteArreglo;
                                            Content = true;
                                            Type = "Personaje";
                                            lenght = ByteArreglo.Length;
                                            return true;

                                        }
                                        else stream.WriteByte(currentByte);
                                    }

                                }

                            }
                        }

                    }
                    else
                    {
                        bytes = new byte[] { ReadByte };
                        Content = false;
                        Text = string.Empty;
                        file.Position--;
                        Type = string.Empty;
                        lenght = 0;
                        return true;
                    }
                }

                if (ReadByte == 0x40)
                    Arroba = true;

                //DIALOGO CONCATENADO O MONOLOGO 
                if (Arroba)
                {
                    using (var stream = new MemoryStream())
                    using (var streamDialogo = new MemoryStream())
                    {
                        int nextInt;
                        bool TextComillas = false, Arroba2 = false, ArrobaTemp = false;
                        stream.WriteByte(ReadByte);
                        var OtraArroba = 0;
                        while ((nextInt = file.ReadByte()) != -1)
                        {
                            nextByte = (byte)nextInt;

                            if (nextByte == 0x22 && CountAfterArroba == 2)
                            { TextComillas = true; stream.WriteByte(nextByte); CountAfterArroba = 0; }
                            else
                            {
                                if (CountAfterArroba < 2 && TextComillas == false && ArrobaTemp == false)
                                {
                                    if (CountAfterArroba == 1 && nextByte == 0x40)
                                    {
                                        file.Position--;
                                        Text = string.Empty;
                                        bytes = stream.ToArray();
                                        Content = false;
                                        Type = string.Empty;
                                        lenght = 0;
                                        return true;
                                    }
                                    else
                                    {
                                        if (CountAfterArroba == 0 && nextByte == 0x40)
                                        {
                                            CountAfterArroba++;
                                            stream.WriteByte(nextByte);
                                            Arroba2 = true;
                                        }
                                        else
                                        {
                                            CountAfterArroba++;
                                            OtraArroba += Arroba2 ? 1 : 0;
                                            stream.WriteByte(nextByte);
                                        }
                                    }
                                }
                                else
                                {
                                    if (TextComillas)
                                    {
                                        if (nextByte == 0x5C)
                                        {
                                            if (ComillaEscapada = ((currentByte = (byte)file.ReadByte()) == 0x22))
                                            {
                                                var ByteComillaEscapada = new byte[] { nextByte, currentByte };
                                                streamDialogo.Write(ByteComillaEscapada, 0, ByteComillaEscapada.Length);
                                            }
                                            else
                                            {
                                                file.Position--;
                                                streamDialogo.WriteByte(nextByte);
                                            }
                                        }
                                        else
                                        {

                                            if (nextByte == 0x22)
                                            {
                                                stream.WriteByte(nextByte);
                                                var ByteArreglo = stream.ToArray();
                                                bytes = ByteArreglo;
                                                var BytesDialogo = streamDialogo.ToArray();
                                                Text = BytesDialogo.ToStringCP932();
                                                Content = true;
                                                Type = "Monologo o Concatenacion";
                                                lenght = ByteArreglo.Length;
                                                return true;
                                            }
                                            else
                                            {
                                                if (nextByte == 0x40)
                                                {
                                                    Text = string.Empty;
                                                    var ByteTemporal = streamDialogo.ToArray();
                                                    stream.Write(ByteTemporal, 0, ByteTemporal.Length);
                                                    bytes = stream.ToArray();
                                                    Content = false;
                                                    Type = string.Empty;
                                                    lenght = 0;
                                                    file.Position--;
                                                    return true;
                                                }
                                                streamDialogo.WriteByte(nextByte);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (CountAfterArroba == 2 && nextByte == 0x40)
                                        {
                                            file.Position--;
                                            Text = string.Empty;
                                            bytes = stream.ToArray();
                                            Content = false;
                                            Type = string.Empty;
                                            lenght = 0;
                                            return true;
                                        }
                                        else
                                        {
                                            if (!Arroba2)
                                            {
                                                Text = string.Empty;
                                                bytes = stream.ToArray();
                                                Content = false;
                                                Type = string.Empty;
                                                lenght = 0;
                                                file.Position--;
                                                return true;
                                            }
                                            else
                                            {
                                                CountAfterArroba = 0;
                                                ArrobaTemp = true;
                                                if (nextByte == 0x22 && OtraArroba == 2)
                                                { TextComillas = true; stream.WriteByte(nextByte); OtraArroba = 0; }
                                                else
                                                {
                                                    byte LEFT_BLACK_LENTICULAR_BRACKET_TEMP;
                                                    if (nextByte == 0x81 && (LEFT_BLACK_LENTICULAR_BRACKET_TEMP = (byte)file.ReadByte()) == 0x79)
                                                    {
                                                        Text = string.Empty;
                                                        bytes = stream.ToArray();
                                                        Content = false;
                                                        Type = string.Empty;
                                                        lenght = 0;
                                                        file.Position -= 2;
                                                        return true;
                                                    }
                                                    if (OtraArroba < 2)
                                                    {
                                                        OtraArroba++;
                                                        stream.WriteByte(nextByte);
                                                    }
                                                    else
                                                    {
                                                        Text = string.Empty;
                                                        bytes = stream.ToArray();
                                                        Content = false;
                                                        Type = string.Empty;
                                                        lenght = 0;
                                                        file.Position--;
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //PATRON 0x00

                if (ReadByte == 0x00)
                {

                    using (var streamDialogo = new MemoryStream())
                    using (var stream = new MemoryStream())
                    {
                        stream.WriteByte(ReadByte); //TITULO
                        int Fin;
                        int Count = 0;
                        if (Parentesis_Apertura = (nextByte = (byte)(Fin = file.ReadByte())) == 0x28)
                        {
                            stream.WriteByte(nextByte);
                            if (ComillaInicio = (nextByte = (byte)file.ReadByte()) == 0x22)
                            {
                                stream.WriteByte(nextByte);
                                int nextByteInt;
                                while ((nextByteInt = file.ReadByte()) != -1)
                                {
                                    currentByte = (byte)nextByteInt;
                                    bool actual;

                                    if (currentByte == 0x22)
                                    {
                                        //var temporal = currentByte;
                                        //stream.WriteByte(currentByte);
                                        if (actual = (nextByte = (byte)file.ReadByte()) == 0x29)
                                        {
                                            stream.WriteByte(currentByte);
                                            stream.WriteByte(nextByte);
                                            var ByteArreglo = stream.ToArray();
                                            var ByteDialogo = streamDialogo.ToArray();
                                            bytes = ByteArreglo;
                                            Content = true;
                                            Text = ByteDialogo.ToStringCP932();
                                            Type = "Titulo";
                                            lenght = ByteArreglo.Length;
                                            return true;
                                        }
                                        else
                                        {
                                            var tempBytes = streamDialogo.ToArray();
                                            stream.Write(tempBytes, 0, tempBytes.Length);
                                            stream.WriteByte(currentByte);
                                            file.Position--;
                                            bytes = stream.ToArray();
                                            Text = string.Empty;
                                            Content = false;
                                            Type = string.Empty;
                                            lenght = 0;
                                            return true;
                                        }

                                    }
                                    else
                                        if (Count < 101)
                                        {
                                            streamDialogo.WriteByte(currentByte); Count++;
                                        }
                                        else
                                        {
                                            var tempBytes = streamDialogo.ToArray();
                                            stream.Write(tempBytes, 0, tempBytes.Length);
                                            stream.WriteByte(currentByte);
                                            file.Position--;
                                            bytes = stream.ToArray();
                                            Text = string.Empty;
                                            Content = false;
                                            Type = string.Empty;
                                            lenght = 0;
                                            return true;
                                        }

                                }
                            }
                            else
                            {
                                if (nextByte == 0x24)
                                {
                                    stream.WriteByte(nextByte);
                                    for (int i = 0; i < 5; i++)
                                    {
                                        stream.WriteByte((byte)file.ReadByte());
                                    }
                                    if ((nextByte = (byte)file.ReadByte()) == 0x22)
                                    {
                                        stream.WriteByte(nextByte);
                                        while ((ReadInt = file.ReadByte()) != -1)
                                        {
                                            currentByte = (byte)ReadInt;

                                            if (currentByte == 0x5C)
                                            {
                                                if (ComillaEscapada = ((nextByte = (byte)file.ReadByte()) == 0x22))
                                                {
                                                    var bytesEscapado = new byte[] { currentByte, nextByte };
                                                    streamDialogo.Write(bytesEscapado, 0, bytesEscapado.Length);
                                                }
                                                else
                                                {
                                                    streamDialogo.WriteByte(currentByte);
                                                    file.Position--;
                                                }

                                            }
                                            else
                                            {

                                                if (currentByte == 0x22)
                                                {
                                                    stream.WriteByte(currentByte);
                                                    var ByteArreglo = streamDialogo.ToArray();
                                                    var textoCP932 = ByteArreglo.ToStringCP932();
                                                    Text = textoCP932;
                                                    bytes = stream.ToArray();
                                                    Content = true;
                                                    Type = "Información";
                                                    lenght = ByteArreglo.Length;
                                                    return true;

                                                }
                                                else streamDialogo.WriteByte(currentByte);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        stream.WriteByte(nextByte);
                                        bytes = stream.ToArray();
                                        Text = string.Empty;
                                        Content = false;
                                        Type = string.Empty;
                                        lenght = 0;
                                        return true;
                                    }
                                }
                                else
                                {
                                    stream.WriteByte(nextByte);
                                    bytes = stream.ToArray();
                                    Text = string.Empty;
                                    Content = false;
                                    Type = string.Empty;
                                    lenght = 0;
                                    return true;
                                }

                            }
                        }
                        else
                        {
                            bool a;//SEGUNDO PATRON
                            if (a = nextByte == 0x61)
                            {
                                stream.WriteByte(nextByte);
                                bool uno;
                                if (uno = (nextByte = (byte)file.ReadByte()) == 0x01)
                                {
                                    stream.WriteByte(nextByte);

                                    if (ComillaInicio = (nextByte = (byte)file.ReadByte()) == 0x22)
                                    {
                                        stream.WriteByte(nextByte);
                                        int nextByteInt;
                                        while ((nextByteInt = file.ReadByte()) != -1)
                                        {
                                            currentByte = (byte)nextByteInt;
                                            bool actual;

                                            if (currentByte == 0x22)
                                            {
                                                stream.WriteByte(currentByte);// LOGRO
                                                if (actual = (nextByte = (byte)file.ReadByte()) == 0x61)
                                                {
                                                    stream.WriteByte(nextByte);
                                                    var ByteArreglo = stream.ToArray();
                                                    var ByteArregloDialogo = streamDialogo.ToArray();
                                                    bytes = ByteArreglo;
                                                    Content = true;
                                                    Text = ByteArregloDialogo.ToStringCP932();
                                                    Type = "Logro";
                                                    lenght = ByteArreglo.Length;
                                                    return true;
                                                }
                                                else
                                                {   //PALABRA GUIA
                                                    if (actual = nextByte == 0x29)
                                                    {
                                                        stream.WriteByte(nextByte);
                                                        var ByteArreglo = stream.ToArray();
                                                        var ByteArregloDialogo = streamDialogo.ToArray();
                                                        bytes = ByteArreglo;
                                                        Content = true;
                                                        Text = ByteArregloDialogo.ToStringCP932();
                                                        Type = "Palabra guia";
                                                        lenght = ByteArreglo.Length;
                                                        return true;
                                                    }
                                                    else
                                                    {
                                                        file.Position--;
                                                    }
                                                }
                                            }
                                            else
                                                streamDialogo.WriteByte(currentByte);

                                        }
                                    }
                                    else
                                    {

                                        stream.WriteByte(nextByte);
                                        bytes = stream.ToArray();
                                        Text = string.Empty;
                                        Content = false;
                                        Type = string.Empty;
                                        lenght = 0;
                                        return true;
                                    }

                                }
                                else
                                {
                                    stream.WriteByte(nextByte);
                                    bytes = stream.ToArray();
                                    Text = string.Empty;
                                    Content = false;
                                    Type = string.Empty;
                                    lenght = 0;
                                    return true;
                                }
                            }
                            else
                            {
                                if (Fin == -1)
                                {
                                    bytes = stream.ToArray();
                                    Text = string.Empty;
                                    Content = false;
                                    Type = string.Empty;
                                    lenght = 0;
                                    return true;
                                }
                                else
                                {

                                    file.Position--;
                                    bytes = stream.ToArray();
                                    Text = string.Empty;
                                    Content = false;
                                    Type = string.Empty;
                                    lenght = 0;
                                    return true;
                                }
                            }
                        }
                    }
                }

                //ELECCION
                if (ReadByte == 0x7B)
                {
                    bool Salto = false;
                    if (Salto = (nextByte = ((byte)file.ReadByte())) == 0x0A)
                    {

                        using (var streamDialogo = new MemoryStream())
                        using (MemoryStream stream = new MemoryStream())
                        {
                            stream.WriteByte(ReadByte);
                            stream.WriteByte(nextByte);
                            int nextByteInt, CountAfterSalto = 0;
                            bool ComillaInicio2 = false, FinComilla2 = false;

                            while ((nextByteInt = (file.ReadByte())) != -1)
                            {
                                currentByte = (byte)nextByteInt;
                                if (ComillaInicio = (currentByte == 0x22 && CountAfterSalto == 2))
                                { stream.WriteByte(currentByte); CountAfterSalto = 0; ComillaInicio2 = true; }
                                else
                                {

                                    if (CountAfterSalto < 2 && ComillaInicio2 == false)
                                    {
                                        CountAfterSalto++;
                                        stream.WriteByte(currentByte);
                                    }
                                    else
                                    {
                                        if (ComillaInicio2)
                                        {
                                            if (currentByte == 0x7D)
                                            {
                                                stream.WriteByte(currentByte);
                                                var BytesStream = stream.ToArray();
                                                var BytesStreamDialogo = streamDialogo.ToArray();
                                                Text = BytesStreamDialogo.ToStringCP932();
                                                var pedro = BytesStream.ToStringCP932();
                                                Content = true;
                                                lenght = BytesStreamDialogo.Length;
                                                Type = "Eleccion";
                                                bytes = BytesStream;
                                                return true;
                                            }
                                            else
                                            {
                                                if (currentByte == 0x22 && !FinComilla2)
                                                {
                                                    stream.WriteByte(currentByte);
                                                    FinComilla2 = true;
                                                    var diagonal = new byte[] { 0x81, 0x87 };
                                                    streamDialogo.Write(diagonal, 0, diagonal.Length);

                                                }
                                                else
                                                {

                                                    if (currentByte != 0x22 && FinComilla2)
                                                    {
                                                        stream.WriteByte(currentByte);
                                                    }
                                                    else
                                                    {
                                                        if (currentByte == 0x22 && FinComilla2)
                                                        {

                                                            byte tmpCheck, tmpCheck2;
                                                            byte[] tempByteLeader;
                                                            tmpCheck = (byte)file.ReadByte();
                                                            tmpCheck2 = (byte)file.ReadByte();
                                                            if (tmpCheck < (byte)0x20)
                                                            {
                                                                if (tmpCheck2 == (byte)0x22)
                                                                {
                                                                    tempByteLeader = new[] { currentByte, tmpCheck, tmpCheck2 };
                                                                    FinComilla2 = false;
                                                                    stream.Write(tempByteLeader, 0, tempByteLeader.Length);
                                                                }
                                                                else
                                                                {
                                                                    file.Position--;
                                                                    tempByteLeader = new[] { currentByte, tmpCheck };
                                                                    stream.Write(tempByteLeader, 0, tempByteLeader.Length);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                FinComilla2 = false;
                                                                stream.WriteByte(currentByte);
                                                                file.Position -= 2;
                                                            }
                                                        }
                                                        else
                                                            streamDialogo.WriteByte(currentByte);
                                                    }

                                                }
                                            }

                                        }
                                        else
                                        {
                                            file.Position--;
                                            bytes = stream.ToArray();
                                            Content = false;
                                            Type = string.Empty;
                                            Text = string.Empty;
                                            lenght = 0;
                                            return true;
                                        }
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        file.Position--;
                        bytes = new byte[] { ReadByte };
                        Content = false;
                        Type = string.Empty;
                        Text = string.Empty;
                        lenght = 0;
                        return true;
                    }
                }

                //DIALOGOS CONJUNTOS
                if (ReadByte == 0x5D)
                {
                    if ((nextByte = (byte)file.ReadByte()) == 0x22)
                    {
                        using (var stream = new MemoryStream())
                        using (var streamDialogo = new MemoryStream())
                        {
                            stream.WriteByte(ReadByte);
                            stream.WriteByte(nextByte);
                            var Count = 0;

                            while ((ReadInt = (byte)file.ReadByte()) != -1)
                            {
                                ReadByte = (byte)ReadInt;

                                if (ReadByte == 0x5C)
                                {
                                    if ((nextByte = (byte)file.ReadByte()) == 0x22)
                                    {
                                        streamDialogo.WriteByte(ReadByte);
                                        streamDialogo.WriteByte(nextByte);
                                    }
                                    else
                                    {
                                        streamDialogo.WriteByte(ReadByte);
                                        file.Position--;
                                    }
                                }
                                else
                                {
                                    if (ReadByte == 0x22)
                                    {
                                        if ((nextByte = (byte)file.ReadByte()) == 0x29)
                                        {
                                            var tempComilla = ReadByte;
                                            var tempParentesis = nextByte;

                                            if ((nextByte = (byte)file.ReadByte()) == 0x23)
                                            {
                                                stream.WriteByte(tempComilla);
                                                stream.WriteByte(tempParentesis);
                                                stream.WriteByte(nextByte);
                                                bytes = stream.ToArray();
                                                var length = streamDialogo.ToArray();
                                                Text = length.ToStringCP932();
                                                lenght = length.Length;
                                                Type = "DIALOGOS CONJUNTOS (PERSONAJE)";
                                                Content = true;
                                                return true;

                                            }
                                            else
                                            {
                                                if (nextByte == 0x0A)
                                                {
                                                    stream.WriteByte(tempComilla);
                                                    stream.WriteByte(tempParentesis);
                                                    stream.WriteByte(nextByte);
                                                    bytes = stream.ToArray();
                                                    var length = streamDialogo.ToArray();
                                                    Text = length.ToStringCP932();
                                                    lenght = length.Length;
                                                    Type = "DIALOGOS CONJUNTOS (DIALOGO)";
                                                    Content = true;
                                                    return true;

                                                }
                                                else
                                                {

                                                    if (nextByte == 0x24)
                                                    {
                                                        stream.WriteByte(tempComilla);
                                                        stream.WriteByte(tempParentesis);
                                                        stream.WriteByte(nextByte);
                                                        bytes = stream.ToArray();
                                                        var length = streamDialogo.ToArray();
                                                        Text = length.ToStringCP932();
                                                        lenght = length.Length;
                                                        Type = "Acción ???";
                                                        Content = true;
                                                        return true;
                                                    }
                                                    else
                                                    {
                                                        var tempBytes = streamDialogo.ToArray();
                                                        stream.Write(tempBytes, 0, tempBytes.Length);
                                                        stream.WriteByte(tempComilla);
                                                        stream.WriteByte(tempParentesis);
                                                        stream.WriteByte(nextByte);
                                                        bytes = stream.ToArray();
                                                        Content = false;
                                                        Type = string.Empty;
                                                        lenght = 0;
                                                        Text = string.Empty;
                                                        return true;
                                                    }
                                                }
                                            }

                                            streamDialogo.WriteByte(ReadByte);
                                            streamDialogo.WriteByte(nextByte);
                                        }
                                    }
                                    else
                                    {
                                        if (Count > 100)
                                        {
                                            var tempBytes = streamDialogo.ToArray();
                                            stream.Write(tempBytes, 0, tempBytes.Length);
                                            stream.WriteByte(ReadByte);
                                            bytes = stream.ToArray();
                                            Content = false;
                                            Type = string.Empty;
                                            lenght = 0;
                                            Text = string.Empty;
                                            return true;
                                        }
                                        else
                                        {
                                            if (ReadByte == 0x40)
                                            {
                                                var tempBytes = streamDialogo.ToArray();
                                                stream.Write(tempBytes, 0, tempBytes.Length);
                                                file.Position--;
                                                bytes = stream.ToArray();
                                                Content = false;
                                                Type = string.Empty;
                                                lenght = 0;
                                                Text = string.Empty;
                                                return true;
                                            }
                                            streamDialogo.WriteByte(ReadByte);
                                            Count++;
                                        }

                                    }
                                }

                            }

                        }
                    }
                    else
                    {
                        bytes = new byte[] { ReadByte };
                        Content = false;
                        Text = string.Empty;
                        Type = string.Empty;
                        lenght = 0;
                        file.Position--;
                        return true;
                    }


                }


                bytes = new byte[] { ReadByte };
                Content = false;
                Text = string.Empty;
                Type = string.Empty;
                lenght = 0;
                return true;

            }


            bytes = null;
            Content = false;
            Text = string.Empty;
            Type = string.Empty;
            lenght = 0;
            return false;

        }


        public bool IsByteEncabezado(byte b)
        {
            return (b >= 0x81 && b <= 0x9F) || (b >= 0xE0 && b <= 0xFC);
        }


        public bool IsByteSegundo(byte b)
        {
            return (b >= 0x40 && b <= 0x7E) || (b >= 0x80 && b <= 0xFC);
        }




    }
}
