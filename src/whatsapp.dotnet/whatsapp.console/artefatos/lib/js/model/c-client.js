() => {

    window.Client = class Client {

        constructor(options = {}) {
        }

        /**
         * Logs out the client, closing the current session
         */
        async logout() {
            return window.Store.AppState.logout();
        }

        /**
         * Returns the version of WhatsApp Web currently being run
         * @returns {Promise<string>}
         */
        async getWWebVersion() {
            return window.Debug.VERSION;
        }

        /**
         * Mark as seen for the Chat
         *  @param {string} chatId
         *  @returns {Promise<boolean>} result
         *
         */
        async sendSeen(chatId) {
            return window.WWebJS.sendSeen(chatId);
        }

        /**
         * Message options.
         * @typedef {Object} MessageSendOptions
         * @property {boolean} [linkPreview=true] - Show links preview
         * @property {boolean} [sendAudioAsVoice=false] - Send audio as voice message
         * @property {boolean} [sendMediaAsSticker=false] - Send media as a sticker
         * @property {boolean} [sendMediaAsDocument=false] - Send media as a document
         * @property {boolean} [parseVCards=true] - Automatically parse vCards and send them as contacts
         * @property {string} [caption] - Image or video caption
         * @property {string} [quotedMessageId] - Id of the message that is being quoted (or replied to)
         * @property {Contact[]} [mentions] - Contacts that are being mentioned in the message
         * @property {boolean} [sendSeen=true] - Mark the conversation as seen after sending the message
         * @property {MessageMedia} [media] - Media to be sent
         */

        /**
           * Send a message to a specific chatId
           * @param {string} chatId
           * @param {string|MessageMedia|Location} content
           * @param {object} options
           * @returns {Promise<Message>} Message that was just sent
           */
        async sendMessage(chatId, content, options = {}) {
            let internalOptions = {
                linkPreview: options.linkPreview === false ? undefined : true,
                sendAudioAsVoice: options.sendAudioAsVoice,
                sendVideoAsGif: options.sendVideoAsGif,
                sendMediaAsSticker: options.sendMediaAsSticker,
                sendMediaAsDocument: options.sendMediaAsDocument,
                caption: options.caption,
                quotedMessageId: options.quotedMessageId,
                parseVCards: options.parseVCards === false ? false : true,
                mentionedJidList: Array.isArray(options.mentions) ? options.mentions.map(contact => contact.id._serialized) : [],
                ...options.extra
            };

            const sendSeen = typeof options.sendSeen === 'undefined' ? true : options.sendSeen;

            if (content instanceof MessageMedia) {
                internalOptions.attachment = content;
                content = '';
            } else if (options.media instanceof MessageMedia) {
                internalOptions.attachment = options.media;
                internalOptions.caption = content;
                content = '';
            } else if (content instanceof Location) {
                internalOptions.location = content;
                content = '';
            } else if (content instanceof Contact) {
                internalOptions.contactCard = content.id._serialized;
                content = '';
            } else if (Array.isArray(content) && content.length > 0 && content[0] instanceof Contact) {
                internalOptions.contactCardList = content.map(contact => contact.id._serialized);
                content = '';
            }

            if (internalOptions.sendMediaAsSticker && internalOptions.attachment) {
                //internalOptions.attachment = await Util.formatToWebpSticker(internalOptions.attachment);
            }



            const chatWid = window.Store.WidFactory.createWid(chatId);
            const chat = await window.Store.Chat.find(chatWid);

            if (sendSeen) {
                window.WWebJS.sendSeen(chatId);
            }
            const msg = await window.WWebJS.sendMessage(chat, content, internalOptions, sendSeen);
            const newMessage = msg.serialize();
            return new Message(this, newMessage);
        }

        /**
     * Searches for messages
     * @param {string} query
     * @param {Object} [options]
     * @param {number} [options.page]
     * @param {number} [options.limit]
     * @param {string} [options.chatId]
     * @returns {Promise<Message[]>}
     */
        async searchMessages(query, options = {}) {

            const { messages } = await window.Store.Msg.search(query, options.page, options.limit, options.chatId);
            const messagesResult = messages.map(msg => window.WWebJS.getMessageModel(msg)); 
            return messagesResult.map(msg => new Message(this, msg));
        }



        /**
         * Get all current chat instances
         * @returns {Promise<Array<Chat>>}
         */
        getChats() {
            let chats = window.WWebJS.getChats();
            return chats.map(chat => window.ChatFactory.create(this, chat));
        }

        /**
         * Get chat instance by ID
         * @param {string} chatId
         * @returns {Promise<Chat>}
         */
        getChatById(chatId) {
            let chat = window.WWebJS.getChat(chatId);
            return ChatFactory.create(this, chat);
        }

        /**
         * Get all current contact instances
         * @returns {Promise<Array<Contact>>}
         */
        async getContacts() {
            let contacts = window.WWebJS.getContacts();
            return contacts.map(contact => ContactFactory.create(this, contact));
        }

        /**
         * Get contact instance by ID
         * @param {string} contactId
         * @returns {Promise<Contact>}
         */
        async getContactById(contactId) {
            let contact = window.WWebJS.getContact(contactId);
            return ContactFactory.create(this, contact);
        }

        /**
         * Returns an object with information about the invite code's group
         * @param {string} inviteCode
         * @returns {Promise<object>} Invite information
         */
        async getInviteInfo(inviteCode) {
            return window.Store.Wap.groupInviteInfo(inviteCode);
        }

        /**
         * Accepts an invitation to join a group
         * @param {string} inviteCode Invitation code
         * @returns {Promise<string>} Id of the joined Chat
         */
        async acceptInvite(inviteCode) {
            const chatId = await window.Store.Invite.sendJoinGroupViaInvite(inviteCode);
            return chatId._serialized;
        }

        /**
         * Sets the current user's status message
         * @param {string} status New status message
         */
        async setStatus(status) {
            return await window.Store.Wap.sendSetStatus(status);
        }

        /**
         * Sets the current user's display name.
         * This is the name shown to WhatsApp users that have not added you as a contact beside your number in groups and in your profile.
         * @param {string} displayName New display name
         */
        async setDisplayName(displayName) {
            return await window.Store.Wap.setPushname(displayName);
        }

        /**
         * Gets the current connection state for the client
         * @returns {WAState}
         */
        async getState() {
            return window.Store.AppState.state;
        }

        /**
         * Marks the client as online
         */
        async sendPresenceAvailable() {
            return window.Store.Wap.sendPresenceAvailable();
        }

        /**
         * Enables and returns the archive state of the Chat
         * @returns {boolean}
         */
        async archiveChat(chatId) {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.archiveChat(chat, true);
            return chat.archive;
        }

        /**
         * Changes and returns the archive state of the Chat
         * @returns {boolean}
         */
        async unarchiveChat(chatId) {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.archiveChat(chat, false);
            return chat.archive;
        }

        /**
         * Pins the Chat
         * @returns {Promise<boolean>} New pin state. Could be false if the max number of pinned chats was reached.
         */
        async pinChat(chatId) {

            let chat = window.Store.Chat.get(chatId);
            if (chat.pin) {
                return true;
            }
            const MAX_PIN_COUNT = 3;
            if (window.Store.Chat.models.length > MAX_PIN_COUNT) {
                let maxPinned = window.Store.Chat.models[MAX_PIN_COUNT - 1].pin;
                if (maxPinned) {
                    return false;
                }
            }
            await window.Store.Cmd.pinChat(chat, true);
            return true;
        }

        /**
         * Unpins the Chat
         * @returns {Promise<boolean>} New pin state
         */
        async unpinChat(chatId) {

            let chat = window.Store.Chat.get(chatId);
            if (!chat.pin) {
                return false;
            }
            await window.Store.Cmd.pinChat(chat, false);
            return false;
        }

        /**
         * Mutes the Chat until a specified date
         * @param {string} chatId ID of the chat that will be muted
         * @param {Date} unmuteDate Date when the chat will be unmuted
         */
        async muteChat(chatId, unmuteDate) {
            let chat = await window.Store.Chat.get(chatId);
            await chat.mute.mute(unmuteDate.getTime() / 1000, !0);
        }

        /**
         * Unmutes the Chat
         * @param {string} chatId ID of the chat that will be unmuted
         */
        async unmuteChat(chatId) {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.muteChat(chat, false);
        }

        /**
         * Mark the Chat as unread
         * @param {string} chatId ID of the chat that will be marked as unread
         */
        async markChatUnread(chatId) {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.markChatUnread(chat, true);
        }

        /**
         * Returns the contact ID's profile picture URL, if privacy settings allow it
         * @param {string} contactId the whatsapp user's ID
         * @returns {Promise<string>}
         */
        async getProfilePicUrl(contactId) {
            const profilePic = await window.Store.Wap.profilePicFind(contactId);
            return profilePic ? profilePic.eurl : undefined;
        }

        /**
         * Force reset of connection state for the client
         */
        async resetState() {
            window.Store.AppState.phoneWatchdog.shiftTimer.forceRunNow();
        }

        /**
         * Check if a given ID is registered in whatsapp
         * @param {string} id the whatsapp user's ID
         * @returns {Promise<Boolean>}
         */
        async isRegisteredUser(id) {
            let result = await window.Store.Wap.queryExist(id);
            return result.jid !== undefined;
        }

        /**
         * Get the registered WhatsApp ID for a number.
         * Will return null if the number is not registered on WhatsApp.
         * @param {string} number Number or ID ("@c.us" will be automatically appended if not specified)
         * @returns {Promise<Object|null>}
         */
        async getNumberId(number) {
            if (!number.endsWith('@c.us')) {
                number += '@c.us';
            }

            try {
                return window.WWebJS.getNumberId(number);
            } catch (_) {
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
        async createGroup(name, participants) {
            if (!Array.isArray(participants) || participants.length == 0) {
                throw 'You need to add at least one other participant to the group';
            }

            if (participants.every(c => c instanceof Contact)) {
                participants = participants.map(c => c.id._serialized);
            }

            const createRes = await window.Store.Wap.createGroup(name, participants);

            if (!createRes.status === 200) {
                throw 'An error occurred while creating the group!';
            }


            const missingParticipants = createRes.participants.reduce(((missing, c) => {
                const id = Object.keys(c)[0];
                const statusCode = c[id].code;
                if (statusCode != 200) return Object.assign(missing, { [id]: statusCode });
                return missing;
            }), {});

            return { gid: createRes.gid, missingParticipants };
        }

        /**
         * Get all current Labels
         * @returns {Promise<Array<Label>>}
         */
        async getLabels() {
            const labels = window.WWebJS.getLabels();
            return labels.map(data => new Label(this, data));
        }

        /**
         * Get Label instance by ID
         * @param {string} labelId
         * @returns {Promise<Label>}
         */
        async getLabelById(labelId) {
            const label = window.WWebJS.getLabel(labelId);
            return new Label(this, label);
        }

        /**
         * Get all Labels assigned to a chat
         * @param {string} chatId
         * @returns {Promise<Array<Label>>}
         */
        async getChatLabels(chatId) {
            const labels = window.WWebJS.getChatLabels(chatId);
            return labels.map(data => new Label(this, data));
        }

        /**
         * Get all Chats for a specific Label
         * @param {string} labelId
         * @returns {Promise<Array<Chat>>}
         */
        async getChatsByLabelId(labelId) {

            const label = window.Store.Label.get(labelId);
            const labelItems = label.labelItemCollection.models;
            const chatIds = labelItems.reduce((result, item) => {
                if (item.parentType === 'Chat') {
                    result.push(item.parentId);
                }
                return result;
            }, []);

            return Promise.all(chatIds.map(id => this.getChatById(id)));
        }
    };
}
