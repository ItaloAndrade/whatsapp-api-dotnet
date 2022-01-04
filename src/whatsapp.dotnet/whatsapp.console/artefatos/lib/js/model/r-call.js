/**23/08/2021 */
() => {
    /**
     * Represents a Business Contact on WhatsApp
     * @extends {Contact}
     */
    window.Call = class Call extends Base {
        _patch(data) {
            /**
             * Call ID
             * @type {string}
             */
            this.id = data.id;
            /**
             * From
             * @type {string}
             */
            this.from = data.peerJid;
            /**
             * Unix timestamp for when the call was created
             * @type {number}
             */
            this.timestamp = data.offerTime;
            /**
             * Is video
             * @type {boolean}
             */
            this.isVideo = data.isVideo;
            /**
             * Is Group
             * @type {boolean}
             */
            this.isGroup = data.isGroup;
            /**
             * Indicates if the call was sent by the current user
             * @type {boolean}
             */
            this.fromMe = data.outgoing;
            /**
             * Indicates if the call can be handled in waweb
             * @type {boolean}
             */
            this.canHandleLocally = data.canHandleLocally;
            /**
             * Indicates if the call Should be handled in waweb
             * @type {boolean}
             */
            this.webClientShouldHandle = data.webClientShouldHandle;
            /**
             * Object with participants
             * @type {object}
             */
            this.participants = data.participants;

            return super._patch(data);
        }

    };
}