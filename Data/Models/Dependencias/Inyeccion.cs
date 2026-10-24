using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using SeenExtractor.Data.Models.Escritura;
using SeenExtractor.Data.Models.Files;
using SeenExtractor.Data.Models.Lectura;
using SeenExtractor.Data.Models.Lectura.ReadFileStream;
using SeenExtractor.Data.Models.P_Lines_Bytes;

namespace SeenExtractor.Data.Models.Dependencias
{
    public static class Inyeccion
    {
        public static void Servicios(this HostApplicationBuilder builder)
        {
            foreach (var item in Types)
            {
                builder.Services.AddScoped(item);
            }
        }

        private static readonly List<Type> Types =
            [
                typeof(MainWindow),
                typeof(ReadFile),
                typeof(PickFile),
                typeof(ExportDialogs),
                typeof(ActionFiles),
                typeof(StreamFile),
                typeof(ReadTraducción),
                typeof(ReplaceDialogs),
                typeof(RellenarList),
                typeof(Dialogos_Conjuntos),
                typeof(OpenFileDialog),
                typeof(SaveFileDialog),
                typeof(OpenFolderDialog)
            ];
    }
}

