() => {

    class InterfaceController {

        constructor() {
        }

        /**
         * Opens the Chat Window
         * @param {string} chatId ID of the chat window that will be opened
         */
        openChatWindow = async (chatId) => {
            let chatWid = window.Store.WidFactory.createWid(chatId);
            let chat = await window.Store.Chat.find(chatWid);
            await window.Store.Cmd.openChatAt(chat);
        }

        /**
         * Opens the Chat Drawer
         * @param {string} chatId ID of the chat drawer that will be opened
         */
        openChatDrawer = async (chatId) => {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.chatInfoDrawer(chat);
        }

        /**
         * Opens the Chat Search
         * @param {string} chatId ID of the chat search that will be opened
         */
        openChatSearch = async (chatId) => {
            let chat = await window.Store.Chat.get(chatId);
            await window.Store.Cmd.chatSearch(chat);
        }

        /**
         * Opens or Scrolls the Chat Window to the position of the message
         * @param {string} msgId ID of the message that will be scrolled to
         */
        openChatWindowAt = async (msgId) => {
            let msg = await window.Store.Msg.get(msgId);
            await window.Store.Cmd.openChatAt(msg.chat, msg.chat.getSearchContext(msg));
        }

        /**
         * Opens the Message Drawer
         * @param {string} msgId ID of the message drawer that will be opened
         */
        openMessageDrawer = async (msgId) => {
            let msg = await window.Store.Msg.get(msgId);
            await window.Store.Cmd.msgInfoDrawer(msg);
        }

        /**
         * Closes the Right Drawer
         */
        closeRightDrawer = async () => {
            await window.Store.Cmd.closeDrawerRight();
        }

        /**
         * Get all Features
            */
        async getFeatures() {
            return window.Store.Features.F;
        }

        /**
         * Check if Feature is enabled
         * @param {string} feature status to check
         */
        async checkFeatureStatus(feature) {
            return window.Store.Features.supportsFeature(feature);
        }

        /**
         * Enable Features
         * @param {string[]} features to be enabled
         */
        async enableFeatures(features) {
            for (const feature in features) {
                window.Store.Features.setFeature(features[feature], true);
            }
        }

        /**
         * Disable Features
         * @param {string[]} features to be disabled
         */
        async disableFeatures(features) {
            for (const feature in features) {
                window.Store.Features.setFeature(features[feature], false);
            }
        }
    }

    window.InterfaceController = new InterfaceController();
}
