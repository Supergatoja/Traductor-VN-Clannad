
using System.Windows;
using static SeenExtractor.EffectW11;

namespace SeenExtractor.Data.Models.Extenciones.Theme
{
    static class ApplyTheme
    {
        public static void Theme(this Window window)
        {

            if (Environment.OSVersion.Version.Build >= 22000)
                EffectW11.ApplyMica(window, DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TABBEDWINDOW);
            else
                if (Environment.OSVersion.Version.Major == 10)
                    Effects.EnableBlur(window);
                else
                    if (Environment.OSVersion.Version.Build >= 7600 && Environment.OSVersion.Version.Build <= 9600)
                        Effects.EnableBlur(window);
        }
    }
}
