using System.Collections.Generic;
using System.Linq;

namespace DoshikLangIR
{
    public class CompilationContext
    {
        public CompilationContext()
        {
            TypeLibrary = new TypeLibrary(this);
        }

        public DoshikExternalApi ExternalApi { get { return _externalApi; } set { _externalApi = value; TypeLibrary.UpdateExternalApiTypes(); } }

        public TypeLibrary TypeLibrary { get; private set; }

        public CompilationUnit CompilationUnit { get; set; }

        public List<string> CompilationErrors { get; } = new List<string>();

        // ToDo: сделать тут указание строки и столбца в исходном коде, откуда пошла ошибка и возможно чего-то еще
        public CompilationErrorException ThrowCompilationError(string errorText)
        {
            CompilationErrors.Add(errorText);

            throw new CompilationErrorException();
        }

        public DoshikExternalApiEvent FindExternalApiEvent(string eventCodeName)
        {
            return ExternalApi.Events.FirstOrDefault(x => x.CodeName == eventCodeName);
        }

        private DoshikExternalApi _externalApi;
    }
}
