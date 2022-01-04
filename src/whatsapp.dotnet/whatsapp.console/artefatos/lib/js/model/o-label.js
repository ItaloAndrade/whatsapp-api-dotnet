
() => {
    /**
     * Represents a Private Contact on WhatsApp
     * @extends {Contact}
     */
    window.Label = class Label extends Base {
        /**
         * @param {Base} client
         * @param {object} labelData
         */
        constructor(client, labelData){
            super(client);

            if(labelData) this._patch(labelData);
        }

        _patch(labelData) {
            /**
             * Label ID
             * @type {string}
             */
            this.id = labelData.id;

            /**
             * Label name
             * @type {string}
             */
            this.name = labelData.name;

            /**
             * Label hex color
             * @type {string}
             */
            this.hexColor = labelData.hexColor;
        }
        /**
         * Get all chats that have been assigned this Label
         * @returns {Promise<Array<Chat>>}
         */
        async getChats(){
            return this.client.getChatsByLabelId(this.id);
        }
    };
}
