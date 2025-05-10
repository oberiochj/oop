using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection;

namespace ChocolateShopApp
{
    interface IChocolate
    {
        void DisplayInfo();
        double CalculatePrice(int quantity);
    }
    public abstract class Chocolate : IChocolate
    {
        private string _name;
        private double _pricePerUnit;
        private int _calories;

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public double PricePerUnit
        {
            get { return _pricePerUnit; }
            protected set { _pricePerUnit = value; }
        }

        public int Calories
        {
            get { return _calories; }
            protected set { _calories = value; }
        }

        protected Chocolate(string name, double pricePerUnit, int calories)
        {
            _name = name;
            _pricePerUnit = pricePerUnit;
            _calories = calories;
        }
        public abstract void DisplayInfo();

        public virtual double CalculatePrice(int quantity)
        {
            return _pricePerUnit * quantity;
        }
        public double CalculatePrice(int quantity, double discountPercent)
        {
            double total = CalculatePrice(quantity);
            double discountAmount = total * discountPercent / 100.0;
            return total - discountAmount;
        }

        public double CalculatePrice(int quantity, double discountPercent, double taxPercent)
        {
            double subtotal = CalculatePrice(quantity, discountPercent);
            double taxAmount = subtotal * taxPercent / 100.0;
            return subtotal + taxAmount;
        }
    }

    public abstract class FlavoredChocolate : Chocolate
    {
        protected string Flavor;

        protected FlavoredChocolate(string name, double pricePerUnit, int calories, string flavor)
            : base(name, pricePerUnit, calories)
        {
            Flavor = flavor;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Flavor: {Flavor} | Price: ${PricePerUnit:F2} per piece | Calories: {Calories}");
        }

        public virtual string GetFlavorDescription()
        {
            return Flavor;
        }
    }

    // Concrete chocolate classes 
    public class DarkChocolate : Chocolate
    {
        public DarkChocolate() : base("Dark Chocolate", 2.5, 150)
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Price: ${PricePerUnit:F2} per piece | Calories:{Calories}");
            Console.WriteLine("Rich and intense dark chocolate flavor with minimum 70% cocoa content.");
        }

        public override double CalculatePrice(int quantity)
        {
            double basePrice = base.CalculatePrice(quantity);
            if (quantity > 10)
            {
                return basePrice * 0.9;
            }
            return basePrice;
        }
    }

    public class MilkChocolate : Chocolate
    {
        public MilkChocolate() : base("Milk Chocolate", 2.0, 180)
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Price: ${PricePerUnit:F2} per piece | Calories:{Calories}");
            Console.WriteLine("Smooth and creamy milk chocolate for everyone.");
        }
    }

    public class WhiteChocolate : Chocolate
    {
        public WhiteChocolate() : base("White Chocolate", 2.2, 170)
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Price: ${PricePerUnit:F2} per piece | Calories:{Calories}");
            Console.WriteLine("Sweet white chocolate with hints of vanilla.");
        }
    }

    public class NutChocolate : FlavoredChocolate
    {
        public NutChocolate() : base("Nutty Chocolate", 3.0, 200, "Hazelnut")
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Flavor: {Flavor} | Price: ${PricePerUnit:F2} per piece | Calories: {Calories}");
            Console.WriteLine("Crunchy hazelnuts packed inside smooth chocolate.");
        }

        public override string GetFlavorDescription()
        {
            return $"{Flavor} flavor with crunchy bits";
        }
    }

    public class FruitChocolate : FlavoredChocolate
    {
        public FruitChocolate() : base("Fruit Chocolate", 3.2, 190, "Strawberry")
        {
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Chocolate: {Name} | Flavor: {Flavor} | Price: ${PricePerUnit:F2} per piece | Calories: {Calories}");
            Console.WriteLine("Sweet strawberry infused chocolate with fruity aroma.");
        }
    }

    public class LimitedEditionChocolate : DarkChocolate
    {
        private string EditionName;

        public LimitedEditionChocolate(string editionName) : base()
        {
            EditionName = editionName;
            Name = $"Dark Chocolate ({EditionName} Edition)";
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Limited Edition Chocolate: {Name}");
            Console.WriteLine($"Price: ${PricePerUnit + 1.5:F2} per piece | Calories: {Calories}");
            Console.WriteLine("Exclusive limited edition with premium ingredients.");
        }

        public override double CalculatePrice(int quantity)
        {
            double basePrice = (PricePerUnit + 1.5) * quantity;
            if (quantity > 5)
            {
                basePrice *= 0.95;
            }
            return basePrice;
        }
    }

    public class Inventory
    {
        protected Dictionary<string, int> stock;

        public Inventory()
        {
            stock = new Dictionary<string, int>();
        }

        public void AddStock(string chocolateName, int quantity)
        {
            if (stock.ContainsKey(chocolateName))
            {
                stock[chocolateName] += quantity;
            }
            else
            {
                stock[chocolateName] = quantity;
            }
        }

        public bool IsAvailable(string chocolateName, int quantity)
        {
            return stock.ContainsKey(chocolateName) && stock[chocolateName] >= quantity;
        }

        public void ReduceStock(string chocolateName, int quantity)
        {
            if (IsAvailable(chocolateName, quantity))
            {
                stock[chocolateName] -= quantity;
            }
        }

        public void DisplayStock()
        {
            Console.WriteLine("\nCurrent Inventory:");
            foreach (var item in stock)
            {
                Console.WriteLine($"- {item.Key}: {item.Value} pieces");
            }
            Console.WriteLine();
        }
    }

    interface IPaymentProcessor
    {
        bool ProcessPayment(double amount);
    }

    public class PaymentProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing payment of ${amount:F2}...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Payment successful!");
            return true;
        }
    }

    public class Order
    {
        public string ChocolateName { get; private set; }
        public int Quantity { get; private set; }
        public double TotalPrice { get; private set; }

        public Order(string chocolateName, int quantity, double totalPrice)
        {
            ChocolateName = chocolateName;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }

        public void DisplayOrderSummary()
        {
            Console.Clear();
            Console.WriteLine("\nOrder Summary:");
            Console.WriteLine($"Chocolate: {ChocolateName}");
            Console.WriteLine($"Quantity: {Quantity}");
            Console.WriteLine($"Total Price: ${TotalPrice:F2}");
        }
    }

    public class OrderHistory
    {
        private List<Order> orders;

        public OrderHistory()
        {
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public void DisplayOrders()
        {
            Console.WriteLine("\nPrevious Orders:");
            if (orders.Count == 0)
            {
                Console.WriteLine("No previous orders found.");
                return;
            }

            foreach (var order in orders)
            {
                Console.WriteLine($"- {order.ChocolateName}: {order.Quantity} pieces | Total Price: ${order.TotalPrice:F2}");
            }
            Console.WriteLine();
        }
    }

    public static class InputHelper
    {
        public static string GetStringInput(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(input));
            return input;
        }

        public static int GetIntInput(string prompt, int min = 1, int max = int.MaxValue)
        {
            int number;
            bool isValid;
            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                isValid = int.TryParse(input, out number) && number >= min && number <= max;
                if (!isValid)
                    Console.WriteLine($"Please enter a valid number between {min} and {max}.");
            } while (!isValid);

            return number;
        }
    }

    public class ChocolateShop
    {
        private List<Chocolate> chocolates;
        private Inventory inventory;
        private IPaymentProcessor paymentProcessor;
        private OrderHistory orderHistory;

        public ChocolateShop()
        {
            chocolates = new List<Chocolate>()
            {
                new DarkChocolate(),
                new MilkChocolate(),
                new WhiteChocolate(),
                new NutChocolate(),
                new FruitChocolate(),
                new LimitedEditionChocolate("Winter")
            };

            inventory = new Inventory();
            paymentProcessor = new PaymentProcessor();
            orderHistory = new OrderHistory();

            foreach (var choc in chocolates)
            {
                inventory.AddStock(choc.Name, 20);
            }
        }

        public void DisplayChocolates()
        {
            Console.WriteLine("Welcome to the Chocolate Shop! Here are the available chocolates: ");
            int index = 1;
            foreach (var choc in chocolates)
            {
                Console.Write($"{index}. ");
                choc.DisplayInfo();
                Console.WriteLine();
                index++;
            }
        }

        private Chocolate GetChocolateByIndex(int index)
        {
            if (index >= 1 && index <= chocolates.Count)
                return chocolates[index - 1];
            else
                return null;
        }

        public void TakeOrder()
        {
            DisplayChocolates();

            int choice = InputHelper.GetIntInput($"Please enter the number of the chocolate you want to buy(1 -{chocolates.Count}): ", 1, chocolates.Count);
            Chocolate selectedChocolate = GetChocolateByIndex(choice);

            if (selectedChocolate == null)
            {
                Console.WriteLine("Invalid choice. Please try again.");
                return;
            }

            Console.WriteLine($"\nYou selected: {selectedChocolate.Name}");
            selectedChocolate.DisplayInfo();

            int quantity = InputHelper.GetIntInput($"How many pieces of {selectedChocolate.Name} would you like to buy?: ", 1);

            if (!inventory.IsAvailable(selectedChocolate.Name, quantity))
            {
                Console.WriteLine($"Sorry, we only have limited stock for {selectedChocolate.Name}.");
                return;
            }

            double totalPrice = selectedChocolate.CalculatePrice(quantity);

            Console.WriteLine($"Total price for {quantity} pieces of {selectedChocolate.Name}: ${ totalPrice: F2}");
            string confirm = InputHelper.GetStringInput("Do you want to proceed with payment? (yes / no): ").ToLower();
            if (confirm == "yes" || confirm == "y")
            {
                bool paymentSuccess = paymentProcessor.ProcessPayment(totalPrice);
                if (paymentSuccess)
                {
                    inventory.ReduceStock(selectedChocolate.Name, quantity);
                    Order order = new Order(selectedChocolate.Name, quantity, totalPrice);
                    orderHistory.AddOrder(order);
                    order.DisplayOrderSummary();
                    Console.WriteLine("\nThank you for your purchase! Enjoy your chocolate!");
                }
                else
                {
                    Console.WriteLine("Payment failed. Please try again later.");
                }
            }
            else
            {
                Console.WriteLine("Order cancelled.");
            }
        }

        public void ShowInventory()
        {
            inventory.DisplayStock();
        }

        public void ShowOrderHistory()
        {
            orderHistory.DisplayOrders();
        }
    }

    public class WelcomeScreen
    {
        public static void Show()
        {
            Console.WriteLine("><><><><><><><><><><><><><><><><><><><><><><><>");
            Console.WriteLine("*                                             *");
            Console.WriteLine("*    WELCOME TO THE CHOCOLATE CORNER          *");
            Console.WriteLine("*                                             *");
            Console.WriteLine("><><><><><><><><><><><><><><><><><><><><><><><>");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            WelcomeScreen.Show();

            ChocolateShop shop = new ChocolateShop();

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine("");
                Console.WriteLine("Menu:");
                Console.WriteLine("1. View chocolates and place order");
                Console.WriteLine("2. View inventory (Admin)");
                Console.WriteLine("3. View previous orders");
                Console.WriteLine("4. Exit");
                Console.WriteLine("");

                int option = InputHelper.GetIntInput("Please select an option (1-4): ", 1, 4);
                Console.WriteLine();
                switch (option)
                {
                    case 1:
                        shop.TakeOrder();
                        break;
                    case 2:
                        shop.ShowInventory();
                        break;
                    case 3:
                        shop.ShowOrderHistory();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine(">>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<");
                        Console.WriteLine(">      Thank you for visiting the Chocolate Corner!        <");
                        Console.WriteLine(">         God Bless and Come Again! Goodbye! >.<           <");
                        Console.WriteLine(">>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<>>>>><<<<<");
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}

