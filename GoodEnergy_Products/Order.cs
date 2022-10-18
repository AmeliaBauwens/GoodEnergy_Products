namespace GoodEnergy_Products
{
    public class Order
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public Order(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
