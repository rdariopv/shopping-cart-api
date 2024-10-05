namespace shopping_cart_api.models
{
    public class CheckoutRequest
    {
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
    }
}
