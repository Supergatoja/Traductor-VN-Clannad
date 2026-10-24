using System.Windows.Input;

namespace SeenExtractor
{
    public static class MainWindowHelpers
    {
        public static RoutedCommand Next { get; } = new RoutedCommand("Next", typeof(MainWindowHelpers));
        public static RoutedCommand Before { get; } = new RoutedCommand("Before", typeof(MainWindowHelpers));
        public static RoutedCommand Lectura_Masiva { get; } = new RoutedCommand("Lectura_Masiva", typeof(MainWindowHelpers));
        public static RoutedCommand Export { get; } = new RoutedCommand("Export", typeof(MainWindowHelpers));
        public static RoutedCommand Open_Clean_Trans { get; } = new RoutedCommand("Open_Clean_Trans", typeof(MainWindowHelpers));
        public static RoutedCommand CleanTranslate { get; } = new RoutedCommand("CleanTranslate", typeof(MainWindowHelpers));
    }
}