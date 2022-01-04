using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using whatsapp.dotnet;
using whatsapp.dotnet;

namespace whatsapp.dotnet
{
    public class Qrcode
    {
        public Qrcode(Provider provider)
        {
            _provider = provider;
        }

        public Provider _provider { get; set; }

        private async Task WaitQr()
        {
            try
            {
                await _provider.page.WaitForSelectorAsync("div[data-ref]");
            }
            catch
            {
                //Console.WriteLine("Erro metodo WaitQr.");
            }
        }

        public async Task QrProcessing(EnviromentJs _enviroment)
        {
            var isQrCode = true;
            var qrCodeOld = string.Empty;
            var countErro = 0;
            while (isQrCode)
                try
                {
                    countErro++;
                    var isInternet =
                        await _provider.page.EvaluateFunctionAsync<dynamic>(
                            "() =>  Array.from(document.querySelectorAll('div')).find(el => el.textContent === 'Tentando conectar ao celular') == undefined");

                    if (!isInternet)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(new {type = 0, data = 1}));
                        Console.Out.Flush();
                        Thread.Sleep(5000);
                        continue;
                    }

                    var qrCodeResult = await GetQrCode();


                    if (string.Empty == qrCodeResult && isInternet)
                    {
                        var statusProcess = await _enviroment.GetStatusProcessAsync();

                        if (statusProcess == ProcessingType.whatsappMonitory) isQrCode = false;
                    }
                    else if (qrCodeResult != qrCodeOld)
                    { 
                        _provider.eventNotification.OnNext(new Notification
                        {
                            ProcessingType = ProcessingType.qrcodeProcessing,
                            Container = qrCodeResult,
                            Instance = _provider.instance
                        });
                    }

                    qrCodeOld = qrCodeResult ?? qrCodeOld;
                    Thread.Sleep(500);
                }
                catch
                {
                    Thread.Sleep(1000);
                    if (countErro < 5) continue; 
                }
        }

        private async Task<string> GetQrCode()
        {
            try
            {
                var qrCode =
                    await _provider.page.EvaluateFunctionAsync<string>("async ()=>await Util.getQrCode()");
                return qrCode;
            }
            catch
            {
                return null;
            }
        }
    }
}