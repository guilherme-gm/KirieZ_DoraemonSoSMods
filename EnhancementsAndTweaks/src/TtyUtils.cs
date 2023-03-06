namespace EnhancementsAndTweaks
{
    internal class TtyUtils
    {
        public static readonly string NormalStyle = "\033[0m";
        public static readonly string BoldStyle = "\033[1m";

        public static string BoldText(string text)
        {
            return $"{TtyUtils.BoldStyle}{text}{TtyUtils.NormalStyle}";
        }
    }
}
