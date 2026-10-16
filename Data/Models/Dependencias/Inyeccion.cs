using Microsoft.Win32;
using SeenExtractor;
using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.Lectura.Masiva;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;
using SeenExtractor.Data.Models.RegexConfig;
using SeenExtractor.Data.Models.Sustitución;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeenExtractor.Data.Models.Dependencias
{
    public class Inyeccion
    {
        private List<Type> List;
        public List<Type> GetList => List;
        public Inyeccion()
        {
            List = new List<Type>()
            {
                typeof(MainWindow),
                typeof(ReadFile),
                typeof(PickFile),
                typeof(ExportDialogs),
                typeof(ConvertFile),
                typeof(ActionFiles),
                typeof(ConfigRegex),
                typeof(StreamFile),
                typeof(ReadTraducción),
                typeof(ReadMasivSeen),
                typeof(ReplaceDialogs),
                typeof(RellenarList),
                typeof(Sustituir),
                typeof(Dialogos_Conjuntos),
                typeof(P_Lines),
                typeof(OpenFileDialog),
                typeof(SaveFileDialog),
                typeof(OpenFolderDialog)
            };
        }


    }
}

