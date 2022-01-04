using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PuppeteerSharp;  
namespace whatsapp.dotnet
{
    public class ws
    {
        private readonly Subject<Notification> _eventRx; /*live events*/
        private readonly bool _changedHideMode; /*indica alteração de modo vizualização*/
        private   EnviromentJs _enviroment; /*js communication client*/
        private   MsgWhatSaap _eventsWhatSaap; /*controller events message */
        private   Provider _provider;  /*provider*/
        private Qrcode _qrCode; /*controller events qrcode */
        private readonly bool _statusHideWhatsAppWeb; /*set hide or show webwhatapp*/


        private static void KillerProcess()
        {
            foreach (var Proc in Process.GetProcesses())

                try
                {
                    if (
                        Proc.ProcessName.ToLower().Contains("chrome")
                        && Proc.MainModule.FileName.Contains(".local-chromium\\Win64-706915\\"))
                        Proc.Kill();
                }
                catch
                {
                }
        }

        public ws(Subject<Notification> eventRx,bool hideWhatsAppWeb,bool saveSession)
        {
            KillerProcess();
            JsUtil.LoadLibsMemory();
            _eventRx = eventRx;
            _statusHideWhatsAppWeb = hideWhatsAppWeb; 
            _changedHideMode = saveSession;
            AsyncHelpers.RunSync(Initialize);
        }

        private async Task Initialize()
        {
            await StartProviders(); 
            var statusProcess = await _enviroment.GetStatusProcessAsync();
           
            switch (statusProcess)
            {
                case ProcessingType.qrcodeProcessing:
                    await _qrCode.QrProcessing(_enviroment);
                    goto case ProcessingType.whatsappMonitory;
                case ProcessingType.whatsappMonitory:
                    await _eventsWhatSaap.ManagerEvents(_enviroment);
                    _provider.eventNotification.OnNext(new Notification{ProcessingType = ProcessingType.whatsappValidationOk});
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// load providers qrcode,enviromentsjs,msgwhassap
        /// </summary>
        /// <returns></returns>
        private async Task StartProviders()
        {
            Killer(); 

            /*se for alteração do modo de vizualização será removida a sessão*/
            if(_changedHideMode)
                if (Directory.Exists($"{Environment.CurrentDirectory}\\session")) Directory.Delete($"{Environment.CurrentDirectory}\\session", true);

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision); /*download driver chrome official site*/

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                UserDataDir = Path.Combine(Environment.CurrentDirectory, "session"),
                Headless = _statusHideWhatsAppWeb,  
                 Args = new[] { "--no-sandbox", "--disable-infobars", "--disable-dev-shm-usage","--disable-browser-side-navigation","--disable-gpu"}
            });
            
            Page whatsAppPage;
            if(_statusHideWhatsAppWeb)
            {
                var incognito = await browser.CreateIncognitoBrowserContextAsync();
                whatsAppPage = await incognito.NewPageAsync();
            }
            else
            {
                whatsAppPage = await browser.NewPageAsync();
            } 
             
            await whatsAppPage.SetUserAgentAsync(
               "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.0 Safari/537.36");
             await whatsAppPage.GoToAsync("https://web.whatsapp.com/"); 
            _provider = new Provider {eventNotification = _eventRx, page = whatsAppPage, browser = browser }; 
            _qrCode = new Qrcode(_provider); 
            _eventsWhatSaap = new MsgWhatSaap(_provider);
            _enviroment = new EnviromentJs(_provider);
 
        }

        /// <summary>
        /// exists other process opened
        /// </summary>
        private void Killer()
        {
            //  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); 
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

        #region Chamadas Api

        /**Return currrent  status */
        public async Task<ProcessingType> GetCurrentStatus()
        {
            return await _enviroment.GetStatusProcessAsync();
        }


        /**Returns an object with information about the invite code's group */
        public async Task<Group> GroupInviteInfo(string inviteCode)
        {
            return await _eventsWhatSaap.GroupInviteInfo(inviteCode);
        }

        /**
            * Accepts an invitation to join a group
            * @param {string} inviteCode Invitation code
        */
        public async Task<string> JoinGroup(string inviteCode)
        {
            return await _eventsWhatSaap.JoinGroup(inviteCode);
        }

        /**
       * Create a new group
       * @param {string} name group title
       * @param {Array<Contact|string>} participants an array of Contacts or contact IDs to add to the group
       * @returns {Object} createRes
       * @returns {string} createRes.gid - ID for the group that was just created
       * @returns {Object.<string,string>} createRes.missingParticipants - participants that were not added to the group. Keys represent the ID for participant that was not added and its value is a status code that represents the reason why participant could not be added. This is usually 403 if the user's privacy settings don't allow you to add them to groups.
       */
        public async Task<GroupCreatedResult> CreateGroup(string name, List<Participant> participants)
        {
            return await _eventsWhatSaap.CreateGroup(name, participants);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Logout()
        {
            return await _eventsWhatSaap.Logout();
        }

        public async Task<string> GetVersion()
        {
            return await _eventsWhatSaap.GetVersion();
        }

        /**
         * Mark as seen for the Chat
         *  @param {string} chatId
         *  @returns {Promise<boolean>} result
         * 
         */
        public async Task<string> MarkSendSeen(string chatId)
        {
            return await _eventsWhatSaap.MarkSendSeen(chatId);
        }

        /** Get chat instance by ID */
        public async Task<Chat> GetChatById(string chatId)
        {
            return await _eventsWhatSaap.GetChatById(chatId);
        }

        /** Get chats */
        public async Task<List<Chat>> GetChats()
        {
            return await _eventsWhatSaap.GetChats();
        }

        /** Get chats */
        public async Task<string> GetImageById(string chatId)
        {
            return await _eventsWhatSaap.GetAvatarById(chatId);
        }

        /** Get chats */
        public async Task<Chat> GetMe()
        {
            return await _eventsWhatSaap.GetMe();
        }

        /** Get chatall contact by id */
        public async Task<List<Contact>> GetContacts()
        {
            return await _eventsWhatSaap.GetContacts();
        }

        public async Task<Contact> GetContactById(string contactId)
        {
            return await _eventsWhatSaap.GetContactById(contactId);
        }

        public async Task<List<Message>> GetMessageById(string contactId,int take)
        {
            return await _eventsWhatSaap.GetMessageById(contactId, take);
        }

        public async Task<string> GetProfilePicUrl(string contactId)
        {
            return await _eventsWhatSaap.GetProfilePicUrl(contactId);
        }

        /**
        * Gets the current connection state for the client
        * @returns {WAState} 
        */
        public async Task<dynamic> GetState()
        {
            return await _eventsWhatSaap.GetState();
        }

        /**
        * Marks the client as online
        */
        public async Task SendPresenceAvailable()
        {
            await _eventsWhatSaap.SendPresenceAvailable();
        }

        /**
            * Send a message to a specific chatId
            * @param {string} chatId
            * @param {string|MessageMedia|Location} content
            * @param {object} options 
            * @returns {Promise<Message>} Message that was just sent
        */
        public  async Task<Message> SendMessageMedia(string chatId, ContentMessage content, MessageOption options)
        {
            if (content.location != null || content.messageMedia != null) throw new Exception("Only with the professional version is it possible to send a file or location. Get in touch with italoandrade.developer@gmail.com.");
            return await _eventsWhatSaap.SendMessageMedia(chatId, content, options);
        }

        #endregion
    }
}