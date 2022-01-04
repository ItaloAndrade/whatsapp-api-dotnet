/**23/08/2021 */

() => {
    /**
     * Represents a Chat on WhatsApp
     * @extends {Base}
     */
    window.Chat = class Chat extends Base {
        constructor(client, data) {
            super(client);

            if (data) this._patch(data);
        }

        _patch(data) {
            /**
             * ID that represents the chat
             * @type {object}
             */
            this.id = data.id;

            /**
             * Title of the chat
             * @type {string}
             */
            this.name = data.formattedTitle;

            /**
             * Indicates if the Chat is a Group Chat
             * @type {boolean}
             */
            this.isGroup = data.isGroup;

            /**
             * Indicates if the Chat is readonly
             * @type {boolean}
             */
            this.isReadOnly = data.isReadOnly;

            /**
             * Amount of messages unread
             * @type {number}
             */
            this.unreadCount = data.unreadCount;

            /**
             * Unix timestamp for when the last activity occurred
             * @type {number}
             */
            this.timestamp = data.t;

            /**
             * Indicates if the Chat is archived
             * @type {boolean}
             */
            this.archived = data.archive;

            /**
             * Indicates if the Chat is pinned
             * @type {boolean}
             */
            this.pinned = !!data.pin;

            /**
             * Indicates if the chat is muted or not
             * @type {number}
             */
            this.isMuted = data.isMuted;

            /**
             * Unix timestamp for when the mute expires
             * @type {number}
             */
            this.muteExpiration = data.muteExpiration;

            return super._patch(data);
        }

        /**
      * Send a message to this chat
      * @param {string|MessageMedia|Location} content
      * @param {MessageSendOptions} [options]
      * @returns {Promise<Message>} Message that was just sent
      */
        async sendMessage(content, options) {
            return this.client.sendMessage(this.id._serialized, content, options);
        }

        /**
      * Set the message as seen
      * @returns {Promise<Boolean>} result
      */
        async sendSeen() {
            return this.client.sendSeen(this.id._serialized);
        }

        /**
         * Clears all messages from the chat
         * @returns {Promise<Boolean>} result
         */
        async clearMessages() {
            return window.WWebJS.sendClearChat(this.id._serialized);
        }

        /**
         * Deletes the chat
         * @returns {Promise<Boolean>} result
         */
        async delete() {
            return window.WWebJS.sendDeleteChat(this.id._serialized);
        }

        /**
         * Archives this chat
         */
        async archive() {
            return this.client.archiveChat(this.id._serialized);
        }

        /**
         * un-archives this chat
         */
        async unarchive() {
            return this.client.unarchiveChat(this.id._serialized);
        }

        /**
         * Pins this chat
         * @returns {Promise<boolean>} New pin state. Could be false if the max number of pinned chats was reached.
         */
        async pin() {
            return this.client.pinChat(this.id._serialized);
        }

        /**
         * Unpins this chat
         * @returns {Promise<boolean>} New pin state
         */
        async unpin() {
            return this.client.unpinChat(this.id._serialized);
        }

        /**
         * Mutes this chat until a specified date
         * @param {Date} unmuteDate Date at which the Chat will be unmuted
         */
        async mute(unmuteDate) {
            return this.client.muteChat(this.id._serialized, unmuteDate);
        }

        /**
         * Unmutes this chat
         */
        async unmute() {
            return this.client.unmuteChat(this.id._serialized);
        }

        /**
         * Mark this chat as unread
         */
        async markUnread() {
            return this.client.markChatUnread(this.id._serialized);
        }

        /**
         * Loads chat messages, sorted from earliest to latest.
         * @param {Object} searchOptions Options for searching messages. Right now only limit is supported.
         * @param {Number} [searchOptions.limit=50] The amount of messages to return. Note that the actual number of returned messages may be smaller if there aren't enough messages in the conversation. Set this to Infinity to load all messages.
         * @returns {Promise<Array<Message>>}
         */
        async fetchMessages(searchOptions) {
            if (!searchOptions || !searchOptions.limit) {
                searchOptions = { limit: 50 };
            }

            const msgFilter = m => !m.isNotification; // dont include notification messages

            const chat = window.Store.Chat.get(this.id._serialized);
            let msgs = chat.msgs.models.filter(msgFilter);

            while (msgs.length < searchOptions.limit) {
                const loadedMessages = await chat.loadEarlierMsgs();
                if (!loadedMessages) break;
                msgs = [...loadedMessages.filter(msgFilter), ...msgs];
            }

            msgs.sort((a, b) => (a.t > b.t) ? 1 : -1);
            if (msgs.length > searchOptions.limit) msgs = msgs.splice(msgs.length - searchOptions.limit);
            let messages = msgs.map(m => window.WWebJS.getMessageModel(m));
            return messages.map(m => new Message(this.client, m));
        }

        /**
         * Simulate typing in chat. This will last for 25 seconds.
         */
        async sendStateTyping() {
            window.WWebJS.sendChatstate("typing", this.id._serialized);
            return true;
        }

        /**
         * Simulate recording audio in chat. This will last for 25 seconds.
         */
        async sendStateRecording() {
            window.WWebJS.sendChatstate('recording', this.id._serialized);
            return true;
        }

        /**
         * Stops typing or recording in chat immediately.
         */
        async clearState() {
            window.WWebJS.sendChatstate('stop', this.id._serialized);
            return true;
        }

        /**
         * Returns the Contact that corresponds to this Chat.
         * @returns {Promise<Contact>}
         */
        async getContact() {
            return await this.client.getContactById(this.id._serialized);
        }

        /**
        * Returns the Contact that corresponds to this Chat.
        * @returns {Promise<Contact>}
         */
        async getContactById(id) {
            return await this.client.getContactById(id);
        }

        /**
         * Returns array of all Labels assigned to this Chat
         * @returns {Promise<Array<Label>>}
         */
        async getLabels() {
            return this.client.getChatLabels(this.id._serialized);
        }
    }
}
