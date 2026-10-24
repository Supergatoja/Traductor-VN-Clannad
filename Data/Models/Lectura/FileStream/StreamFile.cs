using SeenExtractor.Data.Models.Extenciones.StreamWriter;
using System.IO;
namespace SeenExtractor.Data.Models.Lectura.ReadFileStream
{
    public class StreamFile
    {
        public bool ReadTrueArchiveBytes(Stream file, out byte[] bytes, out string Text, out bool Content, out TipoDialogo Type, out int lenght)
        {
            if (file.Position >= file.Length)
            {
                bytes = [];
                Content = false;
                Text = string.Empty;
                Type = TipoDialogo.None;
                lenght = 0;
                return false;
            }
            byte ReadByte, nextByte, currentByte, nextByteC;
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
                        using var stream = new MemoryStream();
                        stream.Add(ReadByte, nextByte);
                        while ((ReadInt = file.ReadByte()) != -1)
                        {
                            currentByte = (byte)ReadInt;
                            if (currentByte == 0x81)
                            {
                                if (RIGHT_BLACK_LENTICULAR_BRACKET = ((nextByte = (byte)file.ReadByte()) == 0x7A))
                                {
                                    if (!(ComillaInicio = ((nextByteC = (byte)file.ReadByte()) == 0x22)))
                                    {
                                        stream.Add(currentByte, nextByte);
                                        file.Position--;
                                        stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                        return true;
                                    }
                                    else
                                        stream.Add(currentByte, nextByte, nextByteC);
                                }
                                else
                                    stream.Add(currentByte, nextByte);
                            }
                            else
                            {
                                if (currentByte == 0x5C)
                                {
                                    if (ComillaEscapada = ((nextByte = (byte)file.ReadByte()) == 0x22))
                                        stream.Add(currentByte, nextByte);
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
                                        stream.GetDialogCharacter932(out Text, out bytes, out Type, out lenght, out Content);
                                        return true;
                                    }
                                    else
                                        stream.WriteByte(currentByte);
                                }
                            }
                        }
                    }
                    else
                    {
                        bytes = [ReadByte];
                        Content = false;
                        Text = string.Empty;
                        file.Position--;
                        Type = TipoDialogo.None;
                        lenght = 0;
                        return true;
                    }
                }
                if (ReadByte == 0x40)
                    Arroba = true;
                //DIALOGO CONCATENADO O MONOLOGO 
                if (Arroba)
                {
                    using var stream = new MemoryStream();
                    using var streamDialogo = new MemoryStream();
                    int nextInt;
                    bool TextComillas = false, Arroba2 = false, ArrobaTemp = false;
                    stream.WriteByte(ReadByte);
                    var OtraArroba = 0;
                    while ((nextInt = file.ReadByte()) != -1)
                    {
                        nextByte = (byte)nextInt;
                        if (nextByte == 0x22 && CountAfterArroba == 2)
                        {
                            TextComillas = true;
                            stream.WriteByte(nextByte);
                            CountAfterArroba = 0;
                        }
                        else
                        {
                            if (CountAfterArroba < 2 && TextComillas == false && ArrobaTemp == false)
                            {
                                if (CountAfterArroba == 1 && nextByte == 0x40)
                                {
                                    file.Position--;
                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
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
                                            stream.Add(nextByte, currentByte);
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
                                            this.GetDialog932(stream, streamDialogo, TipoDialogo.MonologoConcat, out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                        else
                                        {
                                            if (nextByte == 0x40)
                                            {
                                                stream.Add(streamDialogo);
                                                stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
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
                                        stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                        return true;
                                    }
                                    else
                                    {
                                        if (!Arroba2)
                                        {
                                            stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                            file.Position--;
                                            return true;
                                        }
                                        else
                                        {
                                            CountAfterArroba = 0;
                                            ArrobaTemp = true;
                                            if (nextByte == 0x22 && OtraArroba == 2)
                                            {
                                                TextComillas = true;
                                                stream.WriteByte(nextByte);
                                                OtraArroba = 0;
                                            }
                                            else
                                            {
                                                byte LEFT_BLACK_LENTICULAR_BRACKET_TEMP;
                                                if (nextByte == 0x81 && (LEFT_BLACK_LENTICULAR_BRACKET_TEMP = (byte)file.ReadByte()) == 0x79)
                                                {
                                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
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
                                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
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
                //PATRON 0x00
                if (ReadByte == 0x00)
                {
                    using var streamDialogo = new MemoryStream();
                    using var stream = new MemoryStream();
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
                                if (currentByte == 0x5C)
                                {
                                    if (ComillaEscapada = ((nextByte = (byte)file.ReadByte()) == 0x22))
                                        streamDialogo.Add(currentByte, nextByte);
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
                                        if (actual = (nextByte = (byte)file.ReadByte()) == 0x29)
                                        {
                                            stream.Add(currentByte, nextByte);
                                            this.GetDialog932(stream, streamDialogo, TipoDialogo.TItulo, out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                        else
                                        {
                                            stream.Add(streamDialogo, currentByte);
                                            stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                    }
                                    else
                                        if (Count < 101)
                                        {
                                            streamDialogo.WriteByte(currentByte);
                                            Count++;
                                        }
                                        else
                                        {
                                            stream.Add(streamDialogo, currentByte);
                                            stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                }
                            }
                        }
                        else
                        {
                            if (nextByte == 0x24)
                            {
                                stream.WriteByte(nextByte);
                                for (int i = 0; i < 5; i++)
                                    stream.WriteByte((byte)file.ReadByte());
                                if ((nextByte = (byte)file.ReadByte()) == 0x22)
                                {
                                    stream.WriteByte(nextByte);
                                    while ((ReadInt = file.ReadByte()) != -1)
                                    {
                                        currentByte = (byte)ReadInt;
                                        if (currentByte == 0x5C)
                                        {
                                            if (ComillaEscapada = ((nextByte = (byte)file.ReadByte()) == 0x22))
                                                stream.Add(currentByte, nextByte);
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
                                                this.GetDialog932(stream, streamDialogo, TipoDialogo.Informacion, out Text, out bytes, out lenght, out Type, out Content);
                                                return true;
                                            }
                                            else
                                                streamDialogo.WriteByte(currentByte);
                                        }
                                    }
                                }
                                else
                                {
                                    stream.WriteByte(nextByte);
                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                    return true;
                                }
                            }
                            else
                            {
                                stream.WriteByte(nextByte);
                                stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
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
                                                this.GetDialog932(stream, streamDialogo, TipoDialogo.Logro, out Text, out bytes, out lenght, out Type, out Content);
                                                return true;
                                            }
                                            else
                                            {   //PALABRA GUIA
                                                if (actual = nextByte == 0x29)
                                                {
                                                    stream.WriteByte(nextByte);
                                                    this.GetDialog932(stream, streamDialogo, TipoDialogo.PlabraGuia, out Text, out bytes, out lenght, out Type, out Content);
                                                    return true;
                                                }
                                                else
                                                    file.Position--;
                                            }
                                        }
                                        else
                                            streamDialogo.WriteByte(currentByte);
                                    }
                                }
                                else
                                {
                                    stream.WriteByte(nextByte);
                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                    return true;
                                }
                            }
                            else
                            {
                                stream.WriteByte(nextByte);
                                stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                return true;
                            }
                        }
                        else
                        {
                            if (Fin == -1)
                            {
                                stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                return true;
                            }
                            else
                            {
                                file.Position--;
                                stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                return true;
                            }
                        }
                    }
                }
                //ELECCION
                if (ReadByte == 0x7B)
                {
                    if ((nextByte = ((byte)file.ReadByte())) == 0x0A)
                    {
                        using var streamDialogo = new MemoryStream();
                        using var stream = new MemoryStream();
                        stream.Add(ReadByte, nextByte);
                        int nextByteInt, CountAfterSalto = 0;
                        bool ComillaInicio2 = false, FinComilla2 = false;
                        while ((nextByteInt = (file.ReadByte())) != -1)
                        {
                            currentByte = (byte)nextByteInt;
                            if (ComillaInicio = (currentByte == 0x22 && CountAfterSalto == 2))
                            {
                                stream.WriteByte(currentByte);
                                CountAfterSalto = 0;
                                ComillaInicio2 = true;
                            }
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
                                            this.GetDialog932(stream, streamDialogo, TipoDialogo.Eleccion, out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                        else
                                        {
                                            if (currentByte == 0x22 && !FinComilla2)
                                            {
                                                stream.WriteByte(currentByte);
                                                FinComilla2 = true;
                                                streamDialogo.Add(0x81, 0x87);
                                            }
                                            else
                                            {
                                                if (currentByte != 0x22 && FinComilla2)
                                                    stream.WriteByte(currentByte);
                                                else
                                                {
                                                    if (currentByte == 0x22 && FinComilla2)
                                                    {
                                                        byte tmpCheck, tmpCheck2;
                                                        tmpCheck = (byte)file.ReadByte();
                                                        tmpCheck2 = (byte)file.ReadByte();
                                                        if (tmpCheck < (byte)0x20)
                                                        {
                                                            if (tmpCheck2 == (byte)0x22)
                                                            {
                                                                stream.Add(currentByte, tmpCheck, tmpCheck2);
                                                                FinComilla2 = false;
                                                            }
                                                            else
                                                            {
                                                                file.Position--;
                                                                stream.Add(currentByte, tmpCheck);
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
                                        stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        file.Position--;
                        bytes = [ReadByte];
                        Content = false;
                        Type = TipoDialogo.None;
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
                        using var stream = new MemoryStream();
                        using var streamDialogo = new MemoryStream();
                        stream.Add(ReadByte, nextByte);
                        var Count = 0;
                        while ((ReadInt = (byte)file.ReadByte()) != -1)
                        {
                            ReadByte = (byte)ReadInt;
                            if (ReadByte == 0x5C)
                            {
                                if ((nextByte = (byte)file.ReadByte()) == 0x22)
                                    streamDialogo.Add(ReadByte, nextByte);
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
                                            stream.Add(tempComilla, tempParentesis, nextByte);
                                            this.GetDialog932(stream, streamDialogo, TipoDialogo.DialogoConjuntoPersonaje, out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                        else
                                        {
                                            if (nextByte == 0x0A)
                                            {
                                                stream.Add(tempComilla, tempParentesis, nextByte);
                                                this.GetDialog932(stream, streamDialogo, TipoDialogo.DialogoConjunto, out Text, out bytes, out lenght, out Type, out Content);
                                                return true;
                                            }
                                            else
                                            {
                                                if (nextByte == 0x24)
                                                {
                                                    stream.Add(tempComilla, tempParentesis, nextByte);
                                                    this.GetDialog932(stream, streamDialogo, TipoDialogo.Accion, out Text, out bytes, out lenght, out Type, out Content);
                                                    return true;
                                                }
                                                else
                                                {
                                                    stream.Add(streamDialogo, tempComilla, tempParentesis, nextByte);
                                                    stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Count > 100)
                                    {
                                        stream.Add(streamDialogo, ReadByte);
                                        stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                        return true;
                                    }
                                    else
                                    {
                                        if (ReadByte == 0x40)
                                        {
                                            stream.Add(streamDialogo);
                                            file.Position--;
                                            stream.Discard(out Text, out bytes, out lenght, out Type, out Content);
                                            return true;
                                        }
                                        streamDialogo.WriteByte(ReadByte);
                                        Count++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bytes = [ReadByte];
                        Content = false;
                        Text = string.Empty;
                        Type = TipoDialogo.None;
                        lenght = 0;
                        file.Position--;
                        return true;
                    }
                }
                bytes = [ReadByte];
                Content = false;
                Text = string.Empty;
                Type = TipoDialogo.None;
                lenght = 0;
                return true;
            }
            bytes = [];
            Content = false;
            Text = string.Empty;
            Type = TipoDialogo.None;
            lenght = 0;
            return false;
        }
    }
}