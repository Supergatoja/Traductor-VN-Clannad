using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Encodings;

namespace SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings
{
    public static class ConvertTo
    {
        public static string ConvertToCP932(this string text) => GetEncodings.CP932().GetString(text.BytesANSII());

        public static byte[] BytesCP932(this string text) => GetEncodings.CP932().GetBytes(text);

        public static int LengthBytesCP932(this string text) => GetEncodings.CP932().GetByteCount(text);

        public static string ToStringCP932(this byte[] bytes) => GetEncodings.CP932().GetString(bytes);

        public static byte[] BytesCP932(this char[] c) => GetEncodings.CP932().GetBytes(c);






        public static string ConvertToANSII(this string text) => GetEncodings.ANSII().GetString(text.BytesCP932());

        public static byte[] BytesANSII(this string text) => GetEncodings.ANSII().GetBytes(text);

        public static int LengthBytesANSII(this string text) => GetEncodings.ANSII().GetByteCount(text);

        public static string ToStringANSII(this byte[] bytes) => GetEncodings.ANSII().GetString(bytes);

        public static byte[] BytesANSII(this char[] c) => GetEncodings.ANSII().GetBytes(c);
    }
}
