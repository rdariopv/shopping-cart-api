using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopping_cart_api.models;

namespace shopping_cart_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private static List<CartItem> Cart = new List<CartItem>();

        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] CartItem item)
        {
            var existingItem = Cart.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Cart.Add(item);
            }
            return Ok(new { message = "Product added to cart", cart = Cart });
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            return Ok(new { items = Cart, total = Cart.Sum(i => i.Price * i.Quantity) });
        }

        [HttpPut("update")]
        public IActionResult UpdateCartItem([FromBody] CartItem item)
        {
            var existingItem = Cart.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem == null)
            {
                return NotFound("Product not found in cart.");
            }

            existingItem.Quantity = item.Quantity;
            return Ok(new { message = "Quantity updated", cart = Cart });
        }

        [HttpDelete("remove")]
        public IActionResult RemoveFromCart([FromBody] string productId)
        {
            var item = Cart.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                return NotFound("Product not found in cart.");
            }

            Cart.Remove(item);
            return Ok(new { message = "Product removed from cart", cart = Cart });
        }

        [HttpDelete("clear")]
        public IActionResult ClearCart()
        {
            Cart.Clear();
            return Ok(new { message = "Cart cleared", cart = Cart });
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CheckoutRequest request)
        {
            // Aquí podrías integrar lógica de procesamiento de pago, validaciones, etc.
            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                Items = Cart,
                Total = Cart.Sum(i => i.Price * i.Quantity),
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                Status = "processing"
            };

            // Limpiar el carrito después del checkout.
            Cart.Clear();

            return Ok(new { message = "Order placed successfully", order = order });
        }

        [Route("version")]
        [HttpGet()]
        public ActionResult<string> versionable()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            return $"{assembly.ToString()};lastBuild:{lastModified.ToString("dd/MM/yyyy HH:mm:ss")}";
        }



    }
}
