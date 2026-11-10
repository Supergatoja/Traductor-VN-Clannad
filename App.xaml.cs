using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeenExtractor.Data.Models.Dependencias;
using System.Windows;

namespace SeenExtractor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        private IHost? _host;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Servicios();

            _host = builder.Build();

            await _host.StartAsync();

            MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
        }

    }
}
