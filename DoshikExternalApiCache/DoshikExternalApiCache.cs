﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace Doshik
{
    public class DoshikExternalApiCache
    {
        public static DoshikExternalApi GetCachedApi()
        {
            if (_memoryCachedApi != null)
                return _memoryCachedApi;

            // Если в памяти кэша нет - пытаемся прочитать с диска

            var fileName = GetCacheFileName(GetCacheFolder());

            // Если на диске такого файла нет - возвращаем null
            if (!File.Exists(fileName))
                return null;

            // Если же файл есть - читаем, десериализуем и пишем в кэш, затем возвращаем

            var apiJson = File.ReadAllText(fileName);

            _memoryCachedApi = JsonConvert.DeserializeObject<DoshikExternalApi>(apiJson);

            return _memoryCachedApi;
        }

        public static void SetApiToCache(DoshikExternalApi api)
        {
            var cacheFolder = GetCacheFolder();
            Directory.CreateDirectory(cacheFolder);

            var fileName = GetCacheFileName(cacheFolder);

            var formatting = Formatting.None;

            var settings = new JsonSerializerSettings();

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            var apiJson = JsonConvert.SerializeObject(api, formatting, settings);

            File.WriteAllText(fileName, apiJson);
        }

        private static string GetCacheFolder()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataFolder, appDataCacheFolder);
        }

        private static string GetCacheFileName(string cacheFolder)
        {
            return Path.Combine(cacheFolder, cacheFileNameShort);
        }

        private const string appDataCacheFolder = "DoshikExternalApiCache";
        private const string cacheFileNameShort = "cache.json";

        private static DoshikExternalApi _memoryCachedApi;
    }
}
