namespace GoodEnergy_Products
{
    public enum EnumProduct
    {
        Bread = 1,
        Milk = 2,
        Cheese = 3,
        Soup = 4,
        Butter = 5
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            ProductID = id;
            Name = name;
            Price = price;
        }
    }
}
