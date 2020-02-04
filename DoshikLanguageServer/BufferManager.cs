using System.Collections.Concurrent;

namespace DoshikLanguageServer
{
    class BufferManager
    {
        private ConcurrentDictionary<string, SomeData> _buffers = new ConcurrentDictionary<string, SomeData>();

        public void UpdateBuffer(string documentPath, SomeData buffer)
        {
            _buffers.AddOrUpdate(documentPath, buffer, (k, v) => buffer);
        }

        public SomeData GetBuffer(string documentPath)
        {
            return _buffers.TryGetValue(documentPath, out var buffer) ? buffer : null;
        }
    }

    class SomeData
    {
        public string Data { get; set; }
    }
}
