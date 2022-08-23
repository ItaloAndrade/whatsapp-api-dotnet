using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms; 
using whatsapp.dotnet;

namespace WhatsAppDotNet
{
    public partial class Demo : Form
    {

        #region 
        private ws _ws;
        private readonly Subject<Notification> rxSubject = new();
        delegate void SetTextMessage(string message);
        delegate void SetChangeMode(bool value);
        #endregion

        public Demo()
        {
            InitializeComponent();

            
            changeMode(true);

            Thread.Sleep(3000);
            _ = Task.Run(() => line());
        }

        public Task line()
        {
            subscribeNotification(rxSubject);
            _ws = new ws(rxSubject, false, true);
            while (true) Thread.Sleep(int.MaxValue);
        }

        private void subscribeNotification(IObservable<Notification> observable)
        {
            _ = observable.Subscribe(NewNotification);
        }

        public void NewNotification(Notification notification)
        {
            switch (notification.ProcessingType)
            {
                case ProcessingType.qrcodeProcessing:

                    var qrCode = (string)notification.Container;
                    qrCodeImage.Image = help.generateQr(200, 200, qrCode);
                    break;
                case ProcessingType.whatsappValidationOk:
                    if (qrCodeImage.InvokeRequired)
                        Invoke((new SetChangeMode(changeMode)), new object[] { false });
                    else
                        changeMode(false);
                    break;
                case ProcessingType.whatsappMonitory:
                    var content = whatsapp.dotnet.Help.Convert((whatsapp.dotnet.Message)notification.Container);
                    if (txtMessages.InvokeRequired)
                        Invoke((new SetTextMessage(setMessage)), new object[] { content.Body });
                    else
                        setMessage(content.Body);
                    break;
                case ProcessingType.bateryChange:
                    var bate = (Batery)notification.Container;
                    Console.WriteLine(bate.batteryValue);
                    break;
                case ProcessingType.whatsappExit:
                    Close();
                    break;
                case ProcessingType.whatsappErroRestart:
                    break;
                default:
                    break;
            }
        }

        public async Task Logout()
        {
            await _ws.Logout();
            Close();
        }

        public void changeMode(bool value)
        {
            qrCodeImage.Visible = value;
            txtMensagem.Visible = txtMessages.Visible = btnSend.Visible = txtPhone.Visible = !value;
        }

        public void setMessage(string message)
        {
            txtMessages.AppendText(message + "\r\n");
        }

        public async void sendMessage(object sender, EventArgs e)
        {
            await _ws.SendMessageMedia($"{txtPhone.Text.Trim()}@c.us" , new ContentMessage() { msg = txtMensagem.Text }, new MessageOption());
            //txtMessages.AppendText(txtMensagem.Text.Trim() + "\r\n");
        }
    }
}
