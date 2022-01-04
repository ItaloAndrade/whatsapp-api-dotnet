() => {
    /**
     * Represents a Private Contact on WhatsApp
     * @extends {Contact}
     */
    window.EventWS = class EventWS {
        constructor(options = {}) {
        }

        static createEvents() {

            window.Store.Msg.on("add",
                async (msg) => {
                                         
                    if (!msg.isNewMsg || msg.isGroupMsg) return;

                    const hasMedia = msg.hasMedia;
                    msg = window.WWebJS.getMessageModel(msg);

                    if (msg.type === 'gp2') {
                        const notification = new GroupNotification(this, msg);
                        if (msg.subtype === 'add' || msg.subtype === 'invite') {
                            /**
                             * Emitted when a user joins the chat via invite link or is added by an admin.
                             * @event Client#group_join
                             * @param {GroupNotification} notification GroupNotification with more information about the action
                            */
                            emitMessage(window.Events.GROUP_JOIN, notification);
                        } else if (msg.subtype === 'remove' || msg.subtype === 'leave') {
                            /**
                             * Emitted when a user leaves the chat or is removed by an admin.
                             * @event Client#group_leave
                             * @param {GroupNotification} notification GroupNotification with more information about the action
                             */
                            emitMessage(window.Events.GROUP_LEAVE, notification);
                        } else {
                            /**
                             * Emitted when group settings are updated, such as subject, description or picture.
                             * @event Client#group_update
                             * @param {GroupNotification} notification GroupNotification with more information about the action
                             */
                            emitMessage(window.Events.GROUP_UPDATE, notification);
                        }
                        return;
                    }

                    const message = new Message(this, msg);

                    if (msg.id.fromMe) return;

                    if (message.hasMedia) {
                        message.MessageMedia = await message.downloadMedia();
                    }

                    /**
                    * Emitted when a new message is received.
                    * @event Client#message
                    * @param {Message} message The message that was received
                    */
                    emitMessage(window.Events.MESSAGE_RECEIVED, message);

                }); 
            window.Store.AppState.on("change:state",
                (_AppState, state) => {
                    console.log(state);
                      
                    const ACCEPTED_STATES = [WAState.CONNECTED, WAState.OPENING, WAState.PAIRING, WAState.TIMEOUT];

                    if (!ACCEPTED_STATES.includes(state)) {
                        emitChangedExit(state);
                    }
                });
            window.Store.Conn.on("change:battery",
                (state) => {

                    const { battery, plugged } = state;

                    if (battery === undefined) return;

                    /**
                     * Emitted when the battery percentage for the attached device changes
                     * @event Client#change_battery
                     * @param {object} batteryInfo
                     * @param {number} batteryInfo.battery - The current battery percentage
                     * @param {boolean} batteryInfo.plugged - Indicates if the phone is plugged in (true) or not (false)
                     */
                    emitBatteryState({ 'batteryValue': battery, 'isPlugged': plugged });

                });

        }
    };
}
