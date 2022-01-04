() => {
    /**
     * Represents a Business Contact on WhatsApp
     * @extends {Contact}
     */
    window.Order = class Order extends Base {
        _patch(data) {

            /**
           * List of products
           * @type {Array<Product>}
           */
            if (data.products) {
                this.products = data.products.map(product => new Product(this.client, product));
            }
            /**
             * Order Subtotal
             * @type {string}
             */
            this.subtotal = data.subtotal;
            /**
             * Order Total
             * @type {string}
             */
            this.total = data.total;
            /**
             * Order Currency
             * @type {string}
             */
            this.currency = data.currency;
            /**
             * Order Created At
             * @type {number}
             */
            this.createdAt = data.createdAt;

            return super._patch(data);
        }

    };
}