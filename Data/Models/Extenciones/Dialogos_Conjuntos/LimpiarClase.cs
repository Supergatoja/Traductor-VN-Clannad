namespace SeenExtractor.Data.Models.P_Lines_Bytes
{
    internal static class LimpiarClase
    {
        public static void Clean(this Dialogos_Conjuntos dialogos)
        {
            dialogos.Character = null;
            dialogos.Bytes = null;
            dialogos.Dialog = null;
        }
    }
}
