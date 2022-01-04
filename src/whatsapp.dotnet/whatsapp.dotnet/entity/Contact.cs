namespace whatsapp.dotnet
{
    public class Contact
    {
        /**
         * Contact's phone number
         */
        public string number { get; set; }

        /**
         * Indicates if the contact is a business contact
         */
        public bool isBusiness { get; set; }

        /**
         * ID that represents the contact
         */
        public ContactId id { get; set; }

        /**
         * Indicates if the contact is an enterprise contact
         */
        public bool isEnterprise { get; set; }

        /**
         * Indicates if the contact is a group contact
         */
        public bool isGroup { get; set; }

        /**
         * Indicates if the contact is the current user's contact
         */
        public bool isMe { get; set; }

        /**
         * Indicates if the number is saved in the current phone's contacts
         */
        public bool isMyContact { get; set; }

        /**
         * Indicates if the contact is a user contact
         */
        public bool isUser { get; set; }

        /**
         * Indicates if the number is registered on WhatsApp
         */
        public bool isWAContact { get; set; }

        /**
         * @todo verify labels type. didn't have any documentation
         */
        public string[] labels { get; set; }

        /**
         * The contact's name, as saved by the current user
         */
        public string name { get; set; }

        /**
         * The name that the contact has configured to be shown publically
         */
        public string pushname { get; set; }

        /**
         * @todo missing documentation
         */
        public string sectionHeader { get; set; }

        /**
         * A shortened version of name
         */
        public string shortName { get; set; }

        /**
         * Indicates if the status from the contact is muted
         */
        public bool statusMute { get; set; }

        /**
         * @todo missing documentation
         */
        public string type { get; set; }

        /**
         * @todo missing documentation
         */
        public string verifiedLevel { get; set; }

        /**
         * @todo missing documentation
         */
        public string verifiedName { get; set; }
    }

    public class ContactId
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }
}