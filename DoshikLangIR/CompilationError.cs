namespace DoshikLangIR
{
    public class CompilationError
    {
        public string Message { get; set; }

        public int LineIdx { get; set; }
        public int CharInLineIdx { get; set; }

        public int? LineIdxTo { get; set; }
        public int? CharInLineIdxTo { get; set; }
    }
}
