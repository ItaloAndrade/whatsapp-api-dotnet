using System.Collections.Generic;
using System.Threading;

namespace whatsapp.dotnet
{
    public class ProcessApi
    {
        public string Instance { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    }

    public class JsLib
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int Sequencial { get; set; }
    }

    public static class ManagerInfra
    {
        public static List<JsLib> JsLibs { get; set; } = new List<JsLib>();
        public static List<ProcessApi> Processes { get; set; } = new List<ProcessApi>();
    }

    public static class Extentions
    {
        public static List<T> GetClone<T>(this List<T> source)
        {
            return source.GetRange(0, source.Count);
        }
    }
}