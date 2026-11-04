using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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

    public static class Effects
    {
        // Win32 API para efecto Acrylic
        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,       // Efecto Blur clásico de Windows 10
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4 // Efecto Acrílico moderno de Windows 10/11
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        public static void EnableBlur(Window mainWindow)
        {
            var windowHelper = new WindowInteropHelper(mainWindow);
            var hwnd = windowHelper.Handle;

            var accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND,
                GradientColor = unchecked((int)0x001F1F1F),
                AnimationId = 4,
                AccentFlags = 8
                //GradientColor = unchecked((int)0xFFA39EBC)
            };

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(hwnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }



    }

    public static class EffectW11
    {
        // Atributo para controlar el fondo del sistema (Introducido en Win 11 22H2)
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        public enum DWM_SYSTEMBACKDROP_TYPE
        {
            DWMSBT_AUTO = 0,
            DWMSBT_NONE = 1,
            DWMSBT_MAINWINDOW = 2,      // Efecto MICA estándar
            DWMSBT_TRANSLUCENTWINDOW = 3,// Efecto ACRÍLICO moderno de DWM
            DWMSBT_TABBEDWINDOW = 4     // Efecto MICA ALT
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            ref int pvAttribute,
            int cbAttribute
        );

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        private struct Position
        {
            public const uint SWP_NOSIZE = 0x0001;
            public const uint SWP_NOMOVE = 0x0002;
            public const uint SWP_NOZORDER = 0x0004;
            public const uint SWP_FRAMECHANGED = 0x0020;
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public static void ApplyMica(Window window, DWM_SYSTEMBACKDROP_TYPE backdropType)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;
            var attributeValue = (int)backdropType;

            MARGINS margins = new MARGINS { cxLeftWidth = -1, cxRightWidth = -1, cyTopHeight = -1, cyBottomHeight = -1 };
            DwmExtendFrameIntoClientArea(windowHandle, ref margins);
            // Invocación nativa a DWM
            DwmSetWindowAttribute(
                windowHandle,
                DWMWA_SYSTEMBACKDROP_TYPE,
                ref attributeValue,
                sizeof(int)
            );
            SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0, Position.SWP_NOMOVE | Position.SWP_NOSIZE | Position.SWP_NOZORDER | Position.SWP_FRAMECHANGED);
        }
    }

}