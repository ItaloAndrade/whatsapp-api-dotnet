/**23/08/2021 */

() => {

    window.DefaultOptions = {
        puppeteer: {
            headless: true,
            defaultViewport: null
        },
        session: false,
        qrTimeoutMs: 45000,
        qrRefreshIntervalMs: 20000,
        authTimeoutMs: 45000,
        takeoverOnConflict: false,
        takeoverTimeoutMs: 0,
        userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36',
        ffmpegPath: 'ffmpeg',
        bypassCSP: false
    };

    window.Status = {
        INITIALIZING: 0,
        AUTHENTICATING: 1,
        READY: 3
    };

    /**
     * Client status
     * @readonly
     * @enum {number}
     */
    window.Status = {
        INITIALIZING: 0,
        AUTHENTICATING: 1,
        READY: 3
    };

    /**
     * Events that can be emitted by the client
     * @readonly
     * @enum {string}
     */
    window.Events = {
        AUTHENTICATED: 0,
        AUTHENTICATION_FAILURE: 1,
        READY: 2,
        MESSAGE_RECEIVED: 3,
        MESSAGE_CREATE: 4,
        MESSAGE_REVOKED_EVERYONE: 5,
        MESSAGE_REVOKED_ME: 6,
        MESSAGE_ACK: 7,
        MEDIA_UPLOADED: 8,
        GROUP_JOIN: 9,
        GROUP_LEAVE: 10,
        GROUP_UPDATE: 11,
        QR_RECEIVED: 12,
        DISCONNECTED: 13,
        STATE_CHANGED: 14,
        BATTERY_CHANGED: 15,
        INCOMING_CALL: 16
    };

    /**
     * Message types
     * @readonly
     * @enum {string}
     */
    window.MessageTypes = {
        TEXT: 'chat',
        AUDIO: 'audio',
        VOICE: 'ptt',
        IMAGE: 'image',
        VIDEO: 'video',
        DOCUMENT: 'document',
        STICKER: 'sticker',
        LOCATION: 'location',
        CONTACT_CARD: 'vcard',
        CONTACT_CARD_MULTI: 'multi_vcard',
        ORDER: 'order',
        REVOKED: 'revoked',
        PRODUCT: 'product',
        UNKNOWN: 'unknown',
        GROUP_INVITE: 'groups_v4_invite'
    };

    /**
     * Group notification types
     * @readonly
     * @enum {string}
     */
    window.GroupNotificationTypes = {
        ADD: 'add',
        INVITE: 'invite',
        REMOVE: 'remove',
        LEAVE: 'leave',
        SUBJECT: 'subject',
        DESCRIPTION: 'description',
        PICTURE: 'picture',
        ANNOUNCE: 'announce',
        RESTRICT: 'restrict',
    };

    /**
     * Chat types
     * @readonly
     * @enum {string}
     */
    window.ChatTypes = {
        SOLO: 'solo',
        GROUP: 'group',
        UNKNOWN: 'unknown'
    };

    /**
     * WhatsApp state
     * @readonly
     * @enum {string}
     */
    window.WAState = {
        CONFLICT: 'CONFLICT',
        CONNECTED: 'CONNECTED',
        DEPRECATED_VERSION: 'DEPRECATED_VERSION',
        OPENING: 'OPENING',
        PAIRING: 'PAIRING',
        PROXYBLOCK: 'PROXYBLOCK',
        SMB_TOS_BLOCK: 'SMB_TOS_BLOCK',
        TIMEOUT: 'TIMEOUT',
        TOS_BLOCK: 'TOS_BLOCK',
        UNLAUNCHED: 'UNLAUNCHED',
        UNPAIRED: 'UNPAIRED',
        UNPAIRED_IDLE: 'UNPAIRED_IDLE'
    };

    /**
     * Message ACK
     * @readonly
     * @enum {number}
     */
    window.MessageAck = {
        ACK_ERROR: -1,
        ACK_PENDING: 0,
        ACK_SERVER: 1,
        ACK_DEVICE: 2,
        ACK_READ: 3,
        ACK_PLAYED: 4,
    };
}





