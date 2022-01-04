namespace whatsapp.dotnet
{
    public class Chat
    {
        /**
        * ID that represents the chat
        * @type {object}
        */
        public ChatId id { get; set; }

        /**
         * Title of the chat
         * @type {string}
         */
        public string name { get; set; }

        /**
         * Indicates if the Chat is a Group Chat
         * @type {boolean}
         */
        public bool isGroup { get; set; }

        /**
         * Indicates if the Chat is readonly
         * @type {boolean}
         */
        public bool isReadOnly { get; set; }

        /**
         * Amount of messages unread
         * @type {number}
         */
        public int unreadCount { get; set; }

        /**
         * Unix timestamp for when the chat was created
         * @type {number}
         */

        public string timestamp { get; set; }

        /**
         * Indicates if the Chat is archived
         * @type {boolean}
         */
        public bool archived { get; set; }

        public dynamic groupMetadata { get; set; }

        public string eurl { get; set; }

        public string img { get; set; }

        public string imgFull { get; set; }

        public string formattedTitle { get; set; }

        public bool isMe { get; set; } = false;
    }

    public class ChatId
    {
        public string user { get; set; }
        public string _serialized { get; set; }
        public string server { get; set; }
    }
}