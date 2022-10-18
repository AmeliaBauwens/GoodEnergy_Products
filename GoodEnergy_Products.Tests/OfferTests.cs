namespace GoodEnergy_Products.Tests
{
    [TestClass]
    public class OfferTests
    {
        private List<Product> Products { get; set; }
        private readonly ProductService _productService;
        private readonly Product _soup;
        private readonly Product _bread;
        private readonly Product _milk;
        private readonly Product _cheese;
        private readonly Product _butter;


        public OfferTests()
        {
            _productService = new ProductService();
            Products = _productService.ProductList;

            //these might be null, but they really shouldn't be. And if they are, it will fail quickly
            _soup = Products.FirstOrDefault(p => p.ProductID == (int)EnumProduct.Soup);
            _bread = Products.FirstOrDefault(p => p.ProductID == (int)EnumProduct.Bread);
            _milk   = Products.FirstOrDefault(p => p.ProductID == (int)EnumProduct.Milk);
            _cheese  = Products.FirstOrDefault(p => p.ProductID == (int)EnumProduct.Cheese);
            _butter = Products.FirstOrDefault(p => p.ProductID == (int)EnumProduct.Butter);
        }

        [TestMethod]
        public void ProductsArePopulated()
        {
            Assert.IsNotNull(_soup);
            Assert.IsNotNull(_bread);
            Assert.IsNotNull(_milk);
            Assert.IsNotNull(_cheese);
            Assert.IsNotNull(_butter);
        }

        [TestMethod]
        public void AddCheeseToBasket()
        {
            ProductsArePopulated();

            List<Order> orders = new();
            _productService.AddToBasket(_cheese.ProductID, ref orders);
            _productService.AddToBasket(_cheese.ProductID, ref orders);

            Assert.IsTrue(orders.Count == 1);
            Assert.IsTrue(orders[0].Quantity == 2);
            Assert.IsTrue(orders[0].Product.ProductID == _cheese.ProductID);
        }

        [TestMethod]
        public void OneSoupTwoBread()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_soup,1),
                new Order(_bread,2),
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual(2.8M, total);

            decimal breadDiscount = _productService.HalfPriceBreadWithSoup(orders);
            Assert.AreEqual(0.55M, breadDiscount);

            decimal savings = total - breadDiscount;
            Assert.AreEqual(2.25M, savings);
        }

        [TestMethod]
        public void ThreeCheese()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_cheese,3)
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual((decimal)2.7, total);

            decimal cheeseDiscount = _productService.SecondCheeseFree(orders);
            Assert.AreEqual((decimal)0.9, cheeseDiscount);

            decimal savings = total - cheeseDiscount;
            Assert.AreEqual((decimal)1.8, savings);
        }

        [TestMethod]
        public void FourCheese()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_cheese,4)
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual((decimal)3.6, total);

            decimal cheeseDiscount = _productService.SecondCheeseFree(orders);
            Assert.AreEqual((decimal)1.8, cheeseDiscount);

            decimal savings = total - cheeseDiscount;
            Assert.AreEqual((decimal)1.8, savings);
        }

        [TestMethod]
        public void OnlyButter()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_butter,1)
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual((decimal)1.2, total);

            decimal butterDiscount = _productService.ThirdOffButter(orders);
            Assert.AreEqual((decimal)0.4, butterDiscount);

            decimal savings = total - butterDiscount;
            Assert.AreEqual((decimal)0.8, savings);
        }

        [TestMethod]
        public void ButterMilkCheese()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_butter,1),
                new Order(_milk,1),
                new Order(_cheese,1)
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual((decimal)2.6, total);

            _productService.ApplyOffers(orders, out decimal cheeseDiscount, out decimal breadDiscount, out decimal butterDiscount);
            Assert.AreEqual(0, cheeseDiscount);
            Assert.AreEqual(0, breadDiscount);
            Assert.AreEqual((decimal)0.4, butterDiscount);

            decimal savings = total - butterDiscount;
            Assert.AreEqual((decimal)2.2, savings);
        }

        [TestMethod]
        public void MultiOffers()
        {
            ProductsArePopulated();

            List<Order> orders = new()
            {
                new Order(_butter,2), 
                new Order(_cheese,3), 
                new Order(_soup,1),
                new Order(_bread,2)
            };

            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual((decimal)7.9, total);

            _productService.ApplyOffers(orders, out decimal cheeseDiscount, out decimal breadDiscount, out decimal butterDiscount);
            Assert.AreEqual((decimal)0.9, cheeseDiscount);
            Assert.AreEqual((decimal)0.55, breadDiscount);
            Assert.AreEqual((decimal)0.8, butterDiscount);

            decimal totalSavings = cheeseDiscount + breadDiscount + butterDiscount;
            Assert.AreEqual((decimal)2.25,totalSavings);

            decimal savings = total - totalSavings;
            Assert.AreEqual((decimal)5.65, savings);
        }

        [TestMethod]
        public void EmptyBasket()
        {
            List<Order> orders = new();
            
            decimal total = orders.Sum(a => a.Product.Price * a.Quantity);
            Assert.AreEqual(0, total);

            _productService.ApplyOffers(orders, out decimal cheeseDiscount, out decimal breadDiscount, out decimal butterDiscount);
            Assert.AreEqual(0, cheeseDiscount);
            Assert.AreEqual(0, breadDiscount);
            Assert.AreEqual(0, butterDiscount);

            decimal savings = total - butterDiscount - cheeseDiscount - breadDiscount;
            Assert.AreEqual(0, savings);
        }

        [TestMethod]
        public void DecimalToGBString()
        {
            decimal value = 0.5M;
            string strValue = value.ToGBString();
            Assert.AreEqual("£0.50", strValue);
        }
    }
}