using Doshik;

namespace DoshikLanguageServer
{
    class DoshikExternalApiProvider
    {
        public DoshikExternalApi GetExternalApi()
        {
            lock(_lockObj)
            {
                return DoshikExternalApiCache.GetCachedApi();
            }
        }

        private object _lockObj = new object();
    }
}
