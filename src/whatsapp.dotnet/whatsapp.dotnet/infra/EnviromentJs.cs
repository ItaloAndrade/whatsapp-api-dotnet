using System;
using System.Diagnostics;
using System.IO;
using System.Linq; 
using System.Threading;
using System.Threading.Tasks; 

namespace whatsapp.dotnet
{
    public class EnviromentJs
    {
        private readonly Provider _provider;


        /// <summary>
        ///     no momento que instanciar já carrega no a ambiente os js para ser startada posteriormente
        /// </summary>
        /// <param name="provider"></param>
        public EnviromentJs(Provider provider)
        {
            _provider = provider;
            AsyncHelpers.RunSync(InjectJsAsync);
        }


        /// <summary>
        ///     carrega as bibliotecas javascript
        /// </summary>
        /// <returns></returns>
        private async Task InjectJsAsync()
        {
            try
            {
                await _provider.page.EvaluateFunctionAsync(ManagerInfra.JsLibs
                    .FirstOrDefault(obj => obj.Name == "util.infr")?.Content);
            }
            catch (Exception e)
            { 
            }
        }


        /// <summary>
        ///     se tiver valor no storage significa que o mesmo está autenticado
        /// </summary>
        /// <returns></returns>
        public async Task<ProcessingType> GetStatusProcessAsync()
        {
            try
            {
                await _provider.page.WaitForFunctionAsync(@"() =>
                    (document.getElementsByClassName('landing-title version-title').length > 0 
                    || document.getElementsByTagName('canvas').length  > 0  ||
                    undefined !== (Array.from(document.getElementsByTagName('div')).
                    find(o=>o.hasAttribute('data-asset-intro-image-light') || o.hasAttribute('data-asset-intro-image-dark'))))");

                if (await _provider.page.EvaluateFunctionAsync<int>(
                    "() =>  document.getElementsByClassName('landing-title version-title').length") == 1)
                    await RecoverySession();

                return await _provider.page.EvaluateFunctionAsync<dynamic>(
                           "() =>  window.localStorage.getItem('WAToken1')") != null &&
                       await _provider.page.EvaluateFunctionAsync<dynamic>(
                           "() =>  window.localStorage.getItem('WAToken2')") != null
                    ? ProcessingType.whatsappMonitory
                    : ProcessingType.qrcodeProcessing;
            }
            catch
            {
                await RecoverySession();
                /*aguarda para reiniciar processo*/
                Thread.Sleep(new TimeSpan(1, 0, 0));
                return ProcessingType.whatsappErroRestart;
            }
        }

        /// <summary>
        ///     erro grave , tentativa remover e recriar sessão
        /// </summary>
        /// <returns></returns>
        private async Task RecoverySession()
        {
            await _provider.page.ScreenshotAsync("erro.png");
            Killer();
            if (Directory.Exists($"{Environment.CurrentDirectory}\\session"))
                Directory.Delete($"{Environment.CurrentDirectory}\\session", true);
             
        }


        /// <summary>
        ///     exists other process opened
        /// </summary>
        private void Killer()
        {
            if (_provider.page != null && !_provider.page.IsClosed)
            {
                AsyncHelpers.RunSync(() => _provider.page.CloseAsync());
                AsyncHelpers.RunSync(() => _provider.browser.CloseAsync());
            }

            foreach (var Proc in Process.GetProcesses())

                try
                {
                    if (
                        Proc.ProcessName.ToLower().Contains("chrome")
                        && Proc.MainModule.FileName.Contains(".local-chromium\\Win64-706915\\") ||
                        Proc.ProcessName.ToLower().Contains("agentbot.start") &&
                        Proc.Id != Process.GetCurrentProcess().Id)
                        Proc.Kill();
                }
                catch
                {
                }
        }

        public async Task RemoveTokenAsync()
        {
            try
            {
                await _provider.page.EvaluateFunctionAsync<dynamic>(
                    "() => { window.localStorage.removeItem('WAToken1');}");

                await _provider.page.EvaluateFunctionAsync<dynamic>(
                    "() => { window.localStorage.removeItem('WAToken2'); }");
            }
            catch (Exception e)
            { 
            }
        }
    }
}