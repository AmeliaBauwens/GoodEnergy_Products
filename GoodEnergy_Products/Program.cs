

namespace GoodEnergy_Products
{
    public static class Program
    {
        
        private static readonly ProductService productService = new(); //Typically this would be DI'd in 

        public static void Main()
        {
            List<Order> basket = new();

            Console.WriteLine("Hello! We currently have the following current offers:");
            productService.CurrentOffers.ForEach(o => Console.WriteLine(o));

            GoShopping(basket);

            Console.ReadLine();
        }

        private static void GoShopping(List<Order> basket)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Please select from the following products. Press Enter to finish shopping.");
            productService.ProductList.ForEach(p => Console.WriteLine($"{p.ProductID} - {p.Name}  {p.Price.ToGBString()}"));

            //In a real world app this would have a nice UI with clickable buttons and show you a running total as you change your basket etc
            //but keeping it simple in the context of a console app!
            bool shopping = true;
            while (shopping)
            {
                switch (Console.ReadKey().Key)
                {
                    //Bread
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        productService.AddToBasket(1, ref basket);
                        break;
                    //Milk
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        productService.AddToBasket(2, ref basket);
                        break;
                    //Cheese = 3
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        productService.AddToBasket(3, ref basket);
                        break;
                    //Soup = 4,
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        productService.AddToBasket(4, ref basket);
                        break;
                    //Butter = 5
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        productService.AddToBasket(5, ref basket);
                        break;
                    case ConsoleKey.Enter:
                        shopping = false;
                        break;
                    default:
                        Console.WriteLine($"{Environment.NewLine} Unrecognised item. Please try again or press Enter to finish shopping.");
                        break;
                }
            }
            CalculateTotals(basket);
        }

        private static void CalculateTotals(List<Order> basket)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Your basket contains:");

            decimal cost = 0;
            basket.ForEach(b =>
            {
                cost += b.Product.Price * b.Quantity;
                Console.WriteLine($"{b.Quantity} {b.Product.Name}");
            });

            Console.WriteLine($"With a total cost of {cost.ToGBString()}");

            decimal savings = productService.ApplyOffers(basket, out decimal cheeseDiscount, out decimal breadDiscount, out decimal butterDiscount);

            if (cheeseDiscount > 0)
                Console.WriteLine($"{cheeseDiscount.ToGBString()} saved on Cheese");
            if (breadDiscount > 0)
                Console.WriteLine($"{breadDiscount.ToGBString()} saved on Bread");
            if (butterDiscount > 0)
                Console.WriteLine($"{butterDiscount.ToGBString()} saved on Butter");

            Console.WriteLine($"Total savings: {savings.ToGBString()}");
            Console.WriteLine($"{Environment.NewLine}Total cost: {(cost - savings).ToGBString()}");

            Console.WriteLine("Press Enter to checkout, C to carry on shopping or X to clear your basket");
            bool checking = true;
            while (checking)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        checking = false;
                        Console.WriteLine("Thank you for shopping!");
                        break;
                    case ConsoleKey.C:
                        checking = false; 
                        GoShopping(basket);
                        break;
                    case ConsoleKey.X:
                        checking = false; 
                        basket.Clear();
                        GoShopping(basket);
                        break;
                    default:
                        Console.WriteLine($"{Environment.NewLine} Unrecognised item. Please try again or press Enter to finish shopping.");
                        break;
                }
            }
        }
    }
}