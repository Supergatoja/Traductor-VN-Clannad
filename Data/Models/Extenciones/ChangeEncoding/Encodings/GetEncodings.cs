using System.Text;

namespace SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Encodings
{
    public static class GetEncodings
    {
        static GetEncodings()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static  Encoding ANSII() => Encoding.GetEncoding(1252);
        
        public static  Encoding CP932() => Encoding.GetEncoding(932);
        
    }
}
