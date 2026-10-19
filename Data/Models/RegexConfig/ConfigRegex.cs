using System.Text.RegularExpressions;

namespace SeenExtractor.Data.Models.RegexConfig
{
    public static partial class ConfigRegex
    {
        [GeneratedRegex(@"(@[\s\S]*?""((\\.|[^""\\])*)""\s*#)|({\n[\s\S]*?})|(#\x01\x04\x00\x00\x01\x00\x00\(""((\\.|[^""\\])*?)""\s*\))|(\(\$ÿ[\s\S]*?""((\\.|[^""\\])*?)[\s\S]*?\))", RegexOptions.Compiled)]
        public static partial Regex ExtractDialogANSI();

        [GeneratedRegex(@"""((\\.|[^""\\])*)""", RegexOptions.Compiled)]
        public static partial Regex ExtractSelect();

        [GeneratedRegex(@"Linea\s([\d]*)?:[\s]*?""((\\.|[^""\\])*)""[\s]*?\([\d]*?\/([\d]*)?\)", RegexOptions.Compiled)]
        public static partial Regex ExtractTranslate();

        [GeneratedRegex(@"Linea\s([\d]*)?[\s]\([\s]([\s\S]*?)[\s]\):[\s]([\s\S]*?)[\s]\([\d]*?\/([\d]*)?\)", RegexOptions.Compiled)]
        public static partial Regex ExtractTranslateTwo();
        
        [GeneratedRegex(@"Linea\s([\d]*)?[\s]\(([\s\S]*?)\):[\s]([\s\S]*?)[\s]\([\d]*?\/([\d]*)?\)", RegexOptions.Compiled)]
        public static partial Regex ExtractTranslateTwoTwo();

        //[GeneratedRegex(@"Linea\s([\d]*)?:[\s][\s]([\s\S]*?)[\s]\([\d]*?\/([\d]*)?\)", RegexOptions.Compiled)]
        //public partial Regex ExtractTranslateTwoClean();

        [GeneratedRegex(@"(@[\s\S]*?【([\s\S]*?)】""((\\.|[^""\\])*)""\s*#)|(@[\s\S]*?""((\\.|[^""\\])*)""\s*#)|(@[\s\S]\n【([\s\S]*?)】""((\\.|[^""\\])*)""\s*#)|(@[\s\S]\n""((\\.|[^""\\])*)""\s*#)|({\n[\s\S]*?})|(#\x01\x04\x00\x00\x01\x00\x00\(""((\\.|[^""\\])*?)""\s*\))|(\(\$[\s\S]\x5c&\x00\x00\$[\s\S]\x01\x00\x00\x00a\x01""((\\.|[^""\\])*?)""[\s\S]*?\))|(\(\$[\s\S]\x5c&\x00\x00\$[\s\S]\x03\x00\x00\x00a\x01""((\\.|[^""\\])*?)""[\s\S]*?\))", RegexOptions.Compiled)]
        public static partial Regex ExtractDialogCP932();

        [GeneratedRegex(@"【([\s\S]*?)】", RegexOptions.Compiled)]
        public static partial Regex ExtractPersonajeBytes();
        
        
        [GeneratedRegex(@"(@""[\s\S]""((\\.|[^""\\])*)"")|(@[\s\S]*?""((\\.|[^""\\])*)"")", RegexOptions.Compiled)]
        public static partial Regex ExtractMonologoOConcatenacionBytes();

    }
}
