using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using System.IO;

namespace SeenExtractor.Data.Models.Extenciones.StreamWriter
{
    public static class WriteByte
    {
        public static void Add(this MemoryStream stream, params byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                stream.WriteByte(b);
            }
        }
        public static void Add(this MemoryStream stream, MemoryStream memory)
        {
            foreach (byte b in memory.ToArray())
            {
                stream.WriteByte(b);
            }
        }
        public static void Add(this MemoryStream stream, byte[] array, params byte[] bytes)
        {
            foreach (byte b in array)
            {
                stream.WriteByte(b);
            }
            foreach (byte b in bytes)
            {
                stream.WriteByte(b);
            }
        }
        public static void Add(this MemoryStream stream, MemoryStream memory, params byte[] bytes)
        {
            foreach (byte b in memory.ToArray())
            {
                stream.WriteByte(b);
            }
            foreach (byte b in bytes)
            {
                stream.WriteByte(b);
            }
        }
    }

    public static class StreamOut
    {
        public static void GetDialogCharacter932(this MemoryStream stream, out string Text, out byte[] bytes, out TipoDialogo Type, out int length, out bool content)
        {
            bytes = stream.ToArray();
            Text = bytes.ToStringCP932();
            length = bytes.Length;
            content = true;
            Type = TipoDialogo.Characters;

        }

        public static void Discard(this MemoryStream stream, out string Text, out byte[] bytes, out int length, out TipoDialogo Type, out bool content)
        {
            bytes = stream.ToArray();
            Text = string.Empty;
            length = bytes.Length;
            Type = TipoDialogo.None;
            content = false;
        }

        public static string ToStringCP932(this MemoryStream stream) => stream.ToArray().ToStringCP932();

        public static void GetDialog932(this StreamFile file, MemoryStream stream, MemoryStream streamDialog, TipoDialogo tipoDialogo, out string Text, out byte[] bytes, out int length, out TipoDialogo Type, out bool content)
        {
            Text = streamDialog.ToStringCP932();
            bytes = stream.ToArray();
            length = bytes.Length;
            Type = tipoDialogo;
            content = true;
        }
    }

    public readonly struct TipoDialogo(string v) : IEquatable<TipoDialogo>
    {
        private readonly string tipoDialogo = v;

        public static TipoDialogo Characters { get; } = new TipoDialogo(nameof(Characters));
        public static TipoDialogo MonologoConcat { get; } = new TipoDialogo(nameof(MonologoConcat));
        public static TipoDialogo TItulo { get; } = new TipoDialogo(nameof(TItulo));
        public static TipoDialogo Logro { get; } = new TipoDialogo(nameof(Logro));
        public static TipoDialogo Accion { get; } = new TipoDialogo(nameof(Accion));
        public static TipoDialogo DialogoConjunto { get; } = new TipoDialogo(nameof(DialogoConjunto));
        public static TipoDialogo DialogoConjuntoPersonaje { get; } = new TipoDialogo(nameof(DialogoConjuntoPersonaje));
        public static TipoDialogo Eleccion { get; } = new TipoDialogo(nameof(Eleccion));
        public static TipoDialogo PantallaEleccion { get; } = new TipoDialogo(nameof(PantallaEleccion));
        public static TipoDialogo Informacion { get; } = new TipoDialogo(nameof(Informacion));
        public static TipoDialogo PlabraGuia { get; } = new TipoDialogo(nameof(PlabraGuia));
        public static TipoDialogo None { get; } = new TipoDialogo(nameof(None));
        public override string ToString() => tipoDialogo ?? string.Empty;
        public bool Equals(TipoDialogo other) => Equals(other.tipoDialogo);
        internal bool Equals(string other) =>
            string.Equals(tipoDialogo, other, StringComparison.Ordinal);

        public static bool operator ==(TipoDialogo left, TipoDialogo right) =>
            left.Equals(right);

        public static bool operator !=(TipoDialogo left, TipoDialogo right) =>
            !left.Equals(right);
        /*
        public override bool Equals(object obj) =>
            obj is TipoDialogo dialogo && Equals(dialogo);
        */
        public override int GetHashCode() => tipoDialogo?.GetHashCode() ?? 0;
    }

}
