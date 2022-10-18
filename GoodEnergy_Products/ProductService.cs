namespace GoodEnergy_Products
{
    public class ProductService
    {
        public List<Product> ProductList { get; set; }
        public List<string> CurrentOffers { get; set; }

        private readonly Product _bread = new(1, "Bread", new decimal(1.1));
        private readonly Product _milk = new(2, "Milk", new decimal(0.5));
        private readonly Product _cheese = new(3, "Cheese", new decimal(0.9));
        private readonly Product _soup = new(4, "Soup", new decimal(0.6));
        private readonly Product _butter = new(5, "Butter", new decimal(1.20));

        public ProductService()
        {
            /* Normally I would be retrieving the products/offers from a db and if required setting a private field for the product
            rather than declaring the products at class level and adding them to a list in the constructor
            but I've done it like this here for the sake of simplicity and readability */

            ProductList = new List<Product>()
            {
                { _bread},
                { _milk},
                { _cheese},
                { _soup},
                { _butter}
            };

            CurrentOffers = new List<string>{"When you buy a Cheese, you get a second Cheese free!",
            "When you buy a Soup, you get a half price Bread!",
            "Get a third off Butter!" };

        }

        public void AddToBasket(int productId, ref List<Order> basket)
        {
            Product? product = ProductList.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                Console.WriteLine($"{Environment.NewLine} Product could not be added to your basket.");
                return;
            }

            Order? item = basket.FirstOrDefault(i => i.Product == product);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                basket.Add(new(product, 1));
            }
            Console.WriteLine($"{Environment.NewLine} 1 {product.Name} added to your basket");
        }
      
        public decimal ApplyOffers(List<Order> basket, out decimal cheeseDiscount, out decimal breadDiscount, out decimal butterDiscount)
        {
            cheeseDiscount = SecondCheeseFree(basket);
            breadDiscount = HalfPriceBreadWithSoup(basket);
            butterDiscount = ThirdOffButter(basket);

            return cheeseDiscount + breadDiscount + butterDiscount;
        }

        public decimal SecondCheeseFree(List<Order> basket)
        {
            int cheeseQty = basket.FirstOrDefault(b => b.Product.ProductID == _cheese.ProductID)?.Quantity ?? 0;

            return (cheeseQty / 2) * _cheese.Price;
        }

        public decimal HalfPriceBreadWithSoup(List<Order> basket)
        {
            int soupQty = basket.FirstOrDefault(b => b.Product.ProductID == _soup.ProductID)?.Quantity ?? 0;
            int breadQty = basket.FirstOrDefault(b => b.Product.ProductID == _bread.ProductID)?.Quantity ?? 0;

            //each bread, up to a max of soup quantity is half price
            int minBread = Math.Min(soupQty, breadQty);
            return (minBread * _bread.Price) / 2;
        }

        public decimal ThirdOffButter(List<Order> basket)
        {
            int butteryQty = basket.FirstOrDefault(b => b.Product.ProductID == _butter.ProductID)?.Quantity ?? 0;
            return (butteryQty * _butter.Price) / 3;
        }
    }
}

