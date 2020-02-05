using Doshik;
using System.Text;

namespace DoshikLangCompiler.Compilation
{
    public static class DoshikExternalApiExtensions
    {
        /// <summary>
        /// Полная сигнатура перегрузки метода (используется для вызова метода в assembly)
        /// </summary>
        public static string GetFullExternalName(this DoshikExternalApiTypeMethodOverload overload)
        {
            var sb = new StringBuilder();

            sb.Append(overload.Method.Type.ExternalName);
            sb.Append(".__");
            sb.Append(overload.Method.ExternalName);
            sb.Append("__");

            sb.Append(overload.ExternalName);

            return sb.ToString();
        }
    }
}
