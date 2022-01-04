() => {
    /**
     * Represents a Business Contact on WhatsApp
     * @extends {Contact}
     */
    window.Order = class Order extends Base {
        _patch(data) {

            /** Product ID */
            this.id = data.id;
            /** Retailer ID */
            this.retailer_id = data.retailer_id;
            /** Product Name  */
            this.name = data.name;
            /** Product Description */
            this.description = data.description;

            return super._patch(data);
        } 
    };
}