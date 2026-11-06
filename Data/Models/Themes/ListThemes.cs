using static SeenExtractor.Effects;
using static SeenExtractor.EffectW11;

namespace SeenExtractor.Data.Models.Themes
{
    public static class ListThemes
    {
        public readonly struct ThemesOptions<T> (string descripcion, T value) where T  : struct
        {
            public readonly string Descripcion { get; } = descripcion;
            public readonly T Value { get; } = value;
        }

        public static List<ThemesOptions<DWM_SYSTEMBACKDROP_TYPE>> ThemesW11 =>
        [
            new("Sin efectos", DWM_SYSTEMBACKDROP_TYPE.DWMSBT_NONE),
            new("Mica", DWM_SYSTEMBACKDROP_TYPE.DWMSBT_MAINWINDOW),
            new("Acrilico", DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TRANSLUCENTWINDOW),
            new("Mica Opaca", DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TABBEDWINDOW),
            new("Auto", DWM_SYSTEMBACKDROP_TYPE.DWMSBT_AUTO)
        ];
        
        public static List<ThemesOptions<AccentState>> ThemesW10 =>
        [
            new("Sin efectos", AccentState.ACCENT_DISABLED),
            new("Acrilico", AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND),
            new("Blur", AccentState.ACCENT_ENABLE_BLURBEHIND),
            new("Gradiant", AccentState.ACCENT_ENABLE_GRADIENT),
            new("Gradiant transparencia", AccentState.ACCENT_ENABLE_TRANSPARENTGRADIENT)
        ];


    }
}
