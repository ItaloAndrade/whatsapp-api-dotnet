using Newtonsoft.Json;

namespace whatsapp.dotnet
{
    public class GroupNotification
    {
        /**
        * ID that represents the groupNotification
        * @type {object}
        */
        [JsonProperty(PropertyName = "objectId")]
        public string objectId { get; set; } /*code whazaap Id*/

        /**
         * Extra content
         * @type {string}
         */

        public string body { get; set; }

        /** 
         * GroupNotification type
         * @type {GroupNotificationTypes}
         */

        public string type { get; set; }

        /**
         * Unix timestamp for when the groupNotification was created
         * @type {number}
         */

        public string timestamp { get; set; }

        /**
         * ID for the Chat that this groupNotification was sent for.
         * 
         * @type {string}
         */
        public string chatId { get; set; }

        /**
         * ContactId for the user that produced the GroupNotification.
         * @type {string}
         */

        public string author { get; set; }

        /**
         * Contact IDs for the users that were affected by this GroupNotification.
         * @type {Array
         * <string>}
         */

        public string[] recipientIds { get; set; }
    }
}