using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NETCore.Encrypt;
using whatsapp.dotnet;

namespace whatsapp.console.infra
{
    public static class JsFile
    {
         
        /// <summary>
        /// when it is necessary to make a change to the js lib
        /// </summary>
        public static void GenerateEncripty()
        {
            ManagerInfra.JsLibs = new List<JsLib>();
            var controller =
                Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug\\net6.0", ""),
                    "artefatos\\lib\\js\\interface");
            var infrastructure =
                Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug\\net6.0", ""),
                    "artefatos\\lib\\js\\infrastructure");
            var model = Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug\\net6.0", ""),
                "artefatos\\lib\\js\\model");

            var controllerDirectory = new DirectoryInfo(controller);
            var infrastructureDirectory = new DirectoryInfo(infrastructure);
            var modelDirectory = new DirectoryInfo(model);
            var files = new List<FileInfo>();

            files.AddRange(controllerDirectory.GetFiles("*.js"));
            foreach (var file in files.OrderBy(o => o.Name).ToList())
                System.IO.File.WriteAllText(
                    file.FullName.Replace("interface", "").Replace(file.Name, file.Name.Substring(0, 3) + ".js")
                        .Replace(".js", ".ctr")
                        .Replace("js", "enc"),
                    EncryptProvider.AESEncrypt(System.IO.File.ReadAllText(file.FullName),
                        "iP0tIyqady43QMFDfxMlrVUwISlfTgur"));

            files.Clear();

            files.AddRange(infrastructureDirectory.GetFiles("*.js"));
            foreach (var file in files.OrderBy(o => o.Name).ToList())
                System.IO.File.WriteAllText(
                    file.FullName.Replace("infrastructure", "").Replace(file.Name, file.Name.Substring(0, 4) + ".js")
                        .Replace(".js", ".infr")
                        .Replace("js", "enc"),
                    EncryptProvider.AESEncrypt(System.IO.File.ReadAllText(file.FullName),
                        "iP0tIyqady43QMFDfxMlrVUwISlfTgur"));

            files.Clear();
            files.AddRange(modelDirectory.GetFiles("*.js"));
            foreach (var file in files.OrderBy(o => o.Name).ToList())
                System.IO.File.WriteAllText(
                    file.FullName.Replace("model", "").Replace(file.Name, file.Name.Substring(0, 3) + ".js")
                        .Replace(".js", ".mode")
                        .Replace("js", "enc"),
                    EncryptProvider.AESEncrypt(System.IO.File.ReadAllText(file.FullName),
                        "iP0tIyqady43QMFDfxMlrVUwISlfTgur"));

            files.Clear();
        }
    }

    public class File
    {
        public string Nome { get; set; }

        public string Content { get; set; }
    }
}