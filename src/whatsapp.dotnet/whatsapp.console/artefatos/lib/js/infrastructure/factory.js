/**23/08/2021 */
() => {

    window.ChatFactory = class ChatFactory {
        static create(client, data) {
            if (data.isGroup) {
                return new GroupChat(client, data);
            }

            return new PrivateChat(client, data);
        }
    };

    window.ContactFactory = class ContactFactory {
        static create(client, data) {
            if (data.isBusiness) {
                return new BusinessContact(client, data);
            }

            return new PrivateContact(client, data);
        }
    };
}
