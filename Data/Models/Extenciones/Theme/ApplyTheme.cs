
using System.Windows;
using static SeenExtractor.EffectW11;
using static SeenExtractor.Effects;

namespace SeenExtractor.Data.Models.Extenciones.Theme
{
    static class ApplyTheme
    {
        public static void Theme(Window window, object theme)
        {
            if (theme is AccentState state)
                ApplyTheme(window, state);
            else
                if (theme is DWM_SYSTEMBACKDROP_TYPE state2)
                    ApplyMica(window, state2);
            window.Refresh();
            /*if (OSBuild >= 22000)
                ApplyMica(window, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TABBEDWINDOW);
            else
                if (OSMajor == 10)
                    ApplyTheme(window, theme);
                else
                    if (OSBuild >= 7600 && OSBuild <= 9600)
                        EnableBlur(window);*/
        }
    }
}
