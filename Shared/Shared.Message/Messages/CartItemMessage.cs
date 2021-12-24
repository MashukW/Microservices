namespace Shared.Message.Messages
{
    public class CartItemMessage
    {
        public CartItemMessage()
        {
            Product = new CartProductMessage();
        }

        public Guid PublicId { get; set; }

        public int Count { get; set; }

        public CartProductMessage Product { get; set; }
    }
}
