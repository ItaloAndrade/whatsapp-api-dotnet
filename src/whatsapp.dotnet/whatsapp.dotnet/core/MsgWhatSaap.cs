using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace whatsapp.dotnet
{
    public class MsgWhatSaap
    {
        #region IOC

        public Provider _provider { get; set; }

        public MsgWhatSaap(Provider provider)
        {
            _provider = provider;

        }

        #endregion

        #region Notification Structure

        /// <summary>
        /// carrega as bibliotecas javascript
        /// </summary>
        /// <returns></returns>
        public void InjectJsAsync()
        {
            try
            {
                foreach (var item in ManagerInfra.JsLibs.Where(obj => obj.Name != "util.infr").OrderBy(obj => obj.Sequencial).ToList())
                    AsyncHelpers.RunSync(() => _provider.page.EvaluateFunctionAsync(item.Content));
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// manager events
        /// </summary>
        /// <returns></returns>
        public async Task ManagerEvents(EnviromentJs _enviroment)
        {
            try
            {
                await WaitLoadingChat();
                Thread.Sleep(1000);
                InjectJsAsync();
                await IsExistStore();
                /*processo monitoramento mensagem*/
                await ReceivedMessageEvent();
                /*logout monitoramento*/
                await ReceivedLogout(_enviroment);
                /*monitramento bateria*/
                await ChangeBatteryState();

            }
            catch
            {
                //Console.WriteLine("Erro verificar se já existe .");
            }
        }

        /// <summary>
        /// verifica se  o contexto esta configurado
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsExistStore()
        {
            try
            {
                return await _provider.page.EvaluateFunctionAsync<bool>("()=> window.Store != undefined");
            }
            catch (Exception e)
            {
                
                return false;
            }
        }

        /// <summary>
        /// da delay até aparecer o container do chat
        /// </summary>
        /// <returns></returns>
        public async Task WaitLoadingChat()
        {
            try
            {
                await _provider.page.WaitForSelectorAsync("#pane-side");
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Erro ao aguardar painel ! " + e.Message);
            }
        }

        /// <summary>
        /// recebe mensagem 
        /// </summary>
        /// <returns></returns>
        private async Task ReceivedMessageEvent()
        {
            try
            {
                await _provider.page.ExposeFunctionAsync("emitMessage", async (EventsMessage typeMes, dynamic msg) =>
                {
                    Message msgSerialize = JsonConvert.DeserializeObject<Message>(msg.ToString());


                    if (typeMes != EventsMessage.MESSAGE_RECEIVED) return;

                    await Task.Run(() =>
                    {
                        _provider.eventNotification.OnNext(new Notification
                        {
                            EventsMessage = typeMes,
                            SubNotificationType = SubNotificationType.msgPrivate,
                            ProcessingType = ProcessingType.whatsappMonitory,
                            Container = msgSerialize
                        });
                    });
                });

                await _provider.page.EvaluateFunctionAsync(@"()=>{ window.EventWS.createEvents(); }");

            }
            catch (Exception e)
            {
              
            }
        }

        /// <summary>
        /// realiza o logout
        /// </summary>
        /// <returns></returns>
        private async Task ReceivedLogout(EnviromentJs _enviroment)
        {
            try
            {
                var statusClose = false;
                _provider.page.FrameNavigated += (sender, e) =>
                {
                    if (statusClose) return;
                    statusClose = true;
                    _provider.eventNotification.OnNext(new Notification
                    {
                        Instance = _provider.instance,
                        ProcessingType = ProcessingType.whatsappExit,
                        Container = null
                    });
                };
            }
            catch
            {
                //Console.WriteLine("Problema monitorar saída logout.");
            }
        }

        /// <summary>
        /// realiza o logout
        /// </summary>
        /// <returns></returns>
        private async Task ChangeBatteryState()
        {
            try
            {
                await _provider.page.ExposeFunctionAsync("emitBatteryState", async (dynamic state) =>
                {
                    await Task.Delay(10);

                    if (state == null) return;

                    var batery = JsonConvert.DeserializeObject<Batery>(state.ToString());

                    _provider.eventNotification.OnNext(new Notification
                    {
                        Instance = _provider.instance,
                        ProcessingType = ProcessingType.bateryChange,
                        Container = batery
                    });
                });
            }
            catch (Exception ex)
            { 
            }

        }

        #endregion

        #region Chamadas Api

        /**Returns an object with information about the invite code's group */
        public async Task<Group> GroupInviteInfo(string inviteCode)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (inviteCode)=>{
                                           try{  
                                                let objCliente = new window.Client();
                                                let result = await  objCliente.getInviteInfo(inviteCode)  ;
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro get data group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", inviteCode);

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<Group>(json ?? string.Empty);
            }
            catch
            {
                return null;
            }
        }

        /**
            * Accepts an invitation to join a group
            * @param {string} inviteCode Invitation code
        */
        public async Task<string> JoinGroup(string inviteCode)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (inviteCode)=>{
                                           try{             
                                                let objCliente = new window.Client();
                                                let result =  await objCliente.acceptInvite(inviteCode) ;
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", inviteCode);

                return result.StatusTransaction == StatusTransaction.Ok ? result.Result.ToString() : null;
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu um erro ao tentar adicionar usuário ao grupo , convite inválido." + e.Message);
                return null;
            }
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
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (name,participants)=>{
                                           try{             
                                                let objCliente = new window.Client();
                                                let result =  await objCliente.createGroup(name,participants) ;
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                    console.log(err);
                                                       
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", name, participants);
                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<GroupCreatedResult>(json ?? string.Empty);

            }
            catch (Exception e)
            { 
                return null;
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Logout()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @" ()=>{
                                           try{   
                                                let objCliente = new window.Client();
                                                objCliente.logout() ;
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : true ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : false,'StatusTransaction' : 1 }) ;                      
                                            }
                                }");

                return result.StatusTransaction == StatusTransaction.Ok;
            }
            catch (Exception e)
            { 
                return false;
            }
        }

        public async Task<string> GetVersion()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"()=>{
                                           try{             
                                                let result =   window.Client.getWWebVersion();  
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }");

                return result.StatusTransaction == StatusTransaction.Ok ? result.Result.ToString() : null;
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu ao tentar obter versão." + e.Message);
                return null;
            }
        }

        /**
         * Mark as seen for the Chat
         *  @param {string} chatId
         *  @returns {Promise<boolean>} result
         * 
         */
        public async Task<string> MarkSendSeen(string chatId)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"(chatId)=>{
                                           try{             
                                                let result =   window.Client.sendSeen(chatId);  
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", chatId);

                return result.StatusTransaction == StatusTransaction.Ok ? result.Result.ToString() : null;
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu MarkSendSeen." + e.Message);
                return null;
            }
        }

        /** Get chat instance by ID */
        public async Task<Chat> GetChatById(string chatId)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"(chatId)=>{
                                           try{   
                                                let objCliente = new window.Client(); 
                                                let result =  objCliente.getChatById(chatId);  
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", chatId);

                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<Chat>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("GetChatById com erro." + e.Message);
                return null;
            }
        }

        /** Get chats */
        public async Task<List<Chat>> GetChats()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async ()=>{
                                           try{   
                                             
                                                let objCliente = new window.Client();  
                                                let result =  objCliente.getChats();    
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){return   Object.assign({'ContentTypeWS' : 0,'Result' : ' erro get  chats !','StatusTransaction' : 1 }) ;   }
                                }");

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<List<Chat>>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("GetChats com problemas." + e.Message);
                return null;
            }
        }

        public async Task<string> GetAvatarById(string chatId)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (chatId)=>{
                                           try{   
                                                let base64 = '';     
                                                let objCliente = new window.Client(); 
                                                let chat =   objCliente.getChatById(chatId); 
                                                if(chat && chat.id){
                                                    debugger
                                                    let resultIma =  await window.Store.ProfilePicThumb.find(chat.id);  
                                                    if(resultIma && resultIma.__x_img)
                                                        base64 = await window.Util.downloadFileWithCredentials(resultIma.__x_img); 
                                                 }

                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : base64 ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){return   Object.assign({'ContentTypeWS' : 0,'Result' : ' erro get  chats !','StatusTransaction' : 1 }) ;   }
                                }", chatId);

                return result.StatusTransaction != StatusTransaction.Ok ?
                    string.Empty : result.Result.ToString();
            }
            catch (Exception e)
            { 
                //Console.WriteLine("GetChats com problemas." + e.Message);
                return string.Empty;
            }
        }

        public async Task<Chat> GetMe()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async ()=>{
                                           try{   
                                                let chat = {};
                                                let objCliente = new window.Client();  
                                                let result = await objCliente.getContacts();     
                                                let contact = result.find(obj=>obj.isMe);    
                                                chat.isMe = true;
                                                chat.name = contact.name;
                                                chat.id = contact.id;
                                                debugger
                                                if(chat && chat.id){
                                                      try{
                                                            let resultIma = await window.Store.ProfilePicThumb.find(chat.id);  
                                                            if(resultIma && resultIma.__x_img)
                                                            chat.img = await window.Util.downloadFileWithCredentials(resultIma.__x_img);                                                         
                                                        }catch(errr){
                                                            chat.img = '';
                                                        }
                                                 }
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : chat ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro GetMe  !','StatusTransaction' : 1 }) ;   }
                                }");

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<Chat>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("GetChats com problemas." + e.Message);
                return null;
            }
        }


        /** Get chatall contact by id */
        public async Task<List<Contact>> GetContacts()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async ()=>{
                                           try{   
 
                                                let objCliente = new window.Client();  
                                                let result = await objCliente.getContacts();   
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){ 
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }");

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<List<Contact>>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu ao tentar obter contatos." + e.Message);
                return null;
            }
        }


        public async Task<Contact> GetContactById(string contactId)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (contactId)=>{
                                           try{   
 
                                                let objCliente = new window.Client();  
                                                let result =  objCliente.getContactById(contactId);   
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){ 
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", contactId);

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<Contact>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu ao tentar obter contatos." + e.Message);
                return null;
            }
        }

        /// <summary>
        /// get all message by id
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetMessageById(string contactId, int take)
        {
            try
            {
                if (take < 1) take = 50;
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (contactId,take)=>{
                                           try{   
                                                debugger 
                                                let objCliente = new window.Client();  
                                                let result = await objCliente.getMessagesByOptions(take,contactId);   
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){ 
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", contactId, take);

                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<List<Message>>(json ?? string.Empty);
            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu ao tentar obter contatos." + e.Message);
                return null;
            }
        }


        public async Task<string> GetProfilePicUrl(string contactId)
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"async (contactId)=>{
                                           try{   
 
                                                let objCliente = new window.Client();   
                                                let result = await objCliente.getProfilePicUrl(contactId);   
                                                     
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){   
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", contactId);


                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                return result.Result?.ToString();
            }
            catch (Exception e)
            { 
                return null;
            }
        }

        /**
        * Gets the current connection state for the client
        * @returns {WAState} 
        */
        public async Task<dynamic> GetState()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"()=>{
                                           try{    
                                                let objCliente = new window.Client(); 
                                                let result =   objCliente.getState();      
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){      
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }");


                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                return result.Result;
            }
            catch (Exception e)
            { 
                return null;
            }
        }

        /**
        * Marks the client as online
        */
        public async Task SendPresenceAvailable()
        {
            try
            {
                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                    @"()=>{
                                           try{    
                                                let objCliente = new window.Client();       
                                                let result = objCliente.sendPresenceAvailable();      
                                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result ,'StatusTransaction ' : 0 }) ;                      
                                            }catch (err){      
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }");

            }
            catch (Exception e)
            { 
            }
        }

        /**
            * Send a message to a specific chatId
            * @param {string} chatId
            * @param {string|MessageMedia|Location} content
            * @param {object} options 
            * @returns {Promise<Message>} Message that was just sent
        */
        public async Task<Message> SendMessageMedia(string chatId, ContentMessage content, MessageOption options)
        {
            try
            {

                var result = await _provider.page.EvaluateFunctionAsync<WsResult>(
                     @"async (chatId,content,options)=>{
                          try{    
                                let objCliente = new window.Client();
                                options = {};
                                 debugger
                                if (content.messageMedia) {
                                    content = new window.MessageMedia(content.messageMedia.mimetype,content.messageMedia.data,content.messageMedia.filename);
                                } else if (content.location) {
                                    content = new window.Location(content.location.latitude,content.location.longitude,content.location.description);
                                } else {
                                    content = content.msg;
                                }
                                
                                let result = objCliente.sendMessage(chatId,content,options); 
                                return Object.assign({},  {'ContentTypeWS' : 0,'Result' : result,'StatusTransaction ' : 0 }) ;          
                                            }catch (err){      
                                                  return   Object.assign({'ContentTypeWS' : 0,'Result' : 'Erro add user in group !','StatusTransaction' : 1 }) ;                      
                                            }
                                }", chatId, content, options);


                if (result.StatusTransaction != StatusTransaction.Ok) return null;
                var json = result.Result.ToString();
                return JsonConvert.DeserializeObject<Message>(json ?? string.Empty);

            }
            catch (Exception e)
            { 
                //Console.WriteLine("Ocorreu ao tentar obter GetInviteInfo." + e.Message);
                return null;
            }
        }

        #endregion
    }
}