/**23/08/2021 */
() => {
/**
 * Location information
 */
    window.Location = class Location {
        /**
         * @param {number} latitude
         * @param {number} longitude
         * @param {?string} description
         */
        constructor(latitude, longitude, description) {
            /**
             * Location latitude
             * @type {number}
             */
            this.latitude = latitude;

            /**
             * Location longitude
             * @type {number}
             */
            this.longitude = longitude;

            /**
             * Name for the location
             * @type {?string}
             */
            this.description = description;
        }
    };
}