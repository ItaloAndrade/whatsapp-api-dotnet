using System.Collections.Generic;
using System.Linq;
using NETCore.Encrypt;

namespace whatsapp.dotnet
{
    public static class JsUtil
    {
        /*usado toda vez para carregar o js na memoria*/
        public static void LoadLibsMemory()
        {
            ManagerInfra.JsLibs = new List<JsLib>();

            foreach (var file in JsFile.GetFiles().Where(obj => obj.Nome.Contains(".ctr")).OrderBy(o => o.Nome).ToList())
            {
                ManagerInfra.JsLibs.Add(new JsLib
                {
                    Content = EncryptProvider.AESDecrypt(file.Content,
                          "iP0tIyqady43QMFDfxMlrVUwISlfTgur"),
                    Name = file.Nome,
                    Sequencial = ManagerInfra.JsLibs.Count
                }); // jsAll.AppendLine(File.ReadAllText(file.FullName));
            }

            foreach (var file in JsFile.GetFiles().Where(obj => obj.Nome.Contains(".infr")).OrderBy(o => o.Nome).ToList())
            {
                ManagerInfra.JsLibs.Add(new JsLib
                {
                    Content = EncryptProvider.AESDecrypt(file.Content,
                          "iP0tIyqady43QMFDfxMlrVUwISlfTgur"),
                    Name = file.Nome,
                    Sequencial = ManagerInfra.JsLibs.Count
                }); // jsAll.AppendLine(File.ReadAllText(file.FullName));
            }
            foreach (var file in JsFile.GetFiles().Where(obj => obj.Nome.Contains(".mode")).OrderBy(o => o.Nome).ToList())
            {
                ManagerInfra.JsLibs.Add(new JsLib
                {
                    Content = EncryptProvider.AESDecrypt(file.Content,
                          "iP0tIyqady43QMFDfxMlrVUwISlfTgur"),
                    Name = file.Nome,
                    Sequencial = ManagerInfra.JsLibs.Count
                }); // jsAll.AppendLine(File.ReadAllText(file.FullName));
            }
        }
    }
}