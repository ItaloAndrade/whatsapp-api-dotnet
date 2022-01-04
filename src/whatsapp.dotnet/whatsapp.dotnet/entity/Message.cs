using Newtonsoft.Json;

namespace whatsapp.dotnet
{
    public class Message
    {
        /**
        * ID that represents the message
        * @type {object}
        */
        [JsonProperty(PropertyName = "objectId")]
        public string objectId { get; set; } /*code whazaap Id*/

        /**
          * ACK status for the message
          * @type {MessageAck}
          */

        public string ack { get; set; }

        /**
        * MediaKey that represents the sticker 'ID'
        * @type {string}
        */


        public string mediaKey { get; set; }

        /**
        * Indicates if the message has media available for download
        * @type {boolean}
        */

        public bool hasMedia { get; set; }

        /**
      * Message content
      * @type {string}
      */

        public string body { get; set; }


        /** 
         * Message type
         * @type {MessageTypes}
         */

        public string type { get; set; }

        /**
         * Unix timestamp for when the message was created
         * @type {number}
         */

        public string timestamp { get; set; }

        /**
       * ID for the Chat that this message was sent to, except if the message was sent by the current user.
       * @type {string}
       */


        public string from { get; set; }


        /**
        * ID for who this message is for.
        * 
        * If the message is sent by the current user, it will be the Chat to which the message is being sent.
        * If the message is sent by another user, it will be the ID for the current user. 
        * @type {string}
        */

        public string to { get; set; }

        /**
        * If the message was sent to a group, this field will contain the user that sent the message.
        * @type {string}
        */

        public string author { get; set; }


        /**
        * Indicates if the message was forwarded
        * @type {boolean}
        */

        public bool isForwarded { get; set; }

        /**
         * Indicates if the message was a broadcast
         * @type {boolean}
         */

        public bool broadcast { get; set; }

        /** 
       * Indicates if the message was sent by the current user
       * @type {boolean}
       */

        public bool fromMe { get; set; }

        /**
        * Indicates if the message was sent as a reply to another message.
        * @type {boolean}
        */

        public bool hasQuotedMsg { get; set; }

        /**
          * Location information contained in the message, if the message is type "location"
          * @type {Location}
          */

        public Location location { get; set; }

        ///**
        // * Indicates the mentions in the message body.
        // * @type {Array
        // * <string>}
        // */

         public MessageMedia MessageMedia { get; set; }
    }
}