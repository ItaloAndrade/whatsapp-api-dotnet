/**23/08/2021 */
() => {
/**
 * Represents a Business Contact on WhatsApp
 * @extends {Contact}
 */
    window.BusinessContact = class BusinessContact extends Contact {
        _patch(data) {
            /**
             * The contact's business profile
             */
            this.businessProfile = data.businessProfile;

            return super._patch(data);
        }

    };
}