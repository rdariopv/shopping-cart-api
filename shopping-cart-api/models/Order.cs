namespace shopping_cart_api.models
{
    public class Order
    {
        public string OrderId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
