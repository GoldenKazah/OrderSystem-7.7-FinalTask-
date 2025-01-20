using static OrderSystem.Program;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace OrderSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer("Lev", "89194531866", new DateOnly(1993, 03, 24));
            HomeDelivery delivery = new HomeDelivery("Sanya", "89194531785", new DateOnly(2024, 6, 7), "Lenina, 47", new TimeOnly(14, 0));
            Food food = new Food("Milk", new DateOnly(2025, 1, 2));
            bool check1 = food.parametrcheck(delivery);
            bool check2 = customer.ageCheck(customer.BirthDate);
            Order<HomeDelivery, Food> order = null;
            if (OrderApprove.orderapprove(check1, check2))

            { order = new Order<HomeDelivery, Food>(customer, delivery, food); }
            
            Console.WriteLine(order.CustomerName);
        }
        public class Order<TDelivery, TProduct>
        where TDelivery : HomeDelivery
        where TProduct : Product
        {
            public Order(Customer customer, TDelivery delivery, TProduct product)
            {
                OrderID = Guid.NewGuid();
                CustomerName = customer.Name;
                CustomerTelephoneNumber = customer.TelephoneNumber;
                Delivery = delivery;
                Product = product;


            }

            public Guid OrderID { get; private set; }
            public string CustomerName { get; set; }
            public string CustomerTelephoneNumber { get; set; }
            public TDelivery Delivery { get; private set; }
            public TProduct Product { get; set; }



        }
        public abstract class Delivery
        {
            public Delivery(string manager_name, string manager_telephonenumber, DateOnly Date)
            {
                this.manager_name = manager_name;
                this.manager_telephonenumber = manager_telephonenumber;
                this.Date = Date;

            }
            public DateOnly Date { get; set; }
            public string manager_name;
            public string manager_telephonenumber;
        }
        public class HomeDelivery : Delivery
        {
            public HomeDelivery(string manager_name, string manager_telephonenumber, DateOnly Date, string customeradress, TimeOnly deliverytime) : base(manager_name, manager_telephonenumber, Date)
            {
                Customeradress = customeradress;
                Deliverytime = deliverytime;

            }
            public string Customeradress { get; private set; }
            private TimeOnly from = TimeOnly.Parse("8:00");
            private TimeOnly to = TimeOnly.Parse("22:00");
            public TimeOnly Deliverytime
            {
                get { return Deliverytime; }
                set
                {
                    if (value.IsBetween(from, to))
                        Deliverytime = value;
                    else Console.WriteLine("Courier doesn't work this time");
                }
            }
        }
        public class PickPointDelevery : Delivery
        {
            public string pickPointAdress = "Lenina street, 123";
            public PickPointDelevery(string manager_name, string manager_telephonenumber, DateOnly Date, PickPointAdress adress) : base(manager_name, manager_telephonenumber, Date)
            {
                switch (adress)
                {
                    case PickPointAdress.Adress1:
                        pickPointAdress = "Gorkogo street, 50";
                        break;
                    case PickPointAdress.Adress2:
                        pickPointAdress = "Lenina street, 59";
                        break;
                    case PickPointAdress.Adress3:
                        pickPointAdress = "Pushkina street, 23";
                        break;
                }
            }
            public enum PickPointAdress
            {
                Adress1,
                Adress2,
                Adress3
            }


        }
        public class ShopDelevery : Delivery
        {
            public string ShopAdress { get; private set; } = "Revolutsii street, 13";
            public ShopDelevery(string manager_name, string manager_telephonenumber, DateOnly Date) : base(manager_name, manager_telephonenumber, Date) { }
        }
        public abstract class Product
        {
            public string Name;
            public int Quantity = 1;
            public Product(string name)
            { Name = name; }
            public Product(string name, int quantity)
            {
                Name = name;
                Quantity = quantity;
            }
            public abstract bool parametrcheck(Delivery delivery);


        }
        public class Homeapplies : Product
        {
            public double weight = 0;
            private double maxweight = 100;
            public Homeapplies(string name, double weight) : base(name)
            { this.weight = weight; }
            public Homeapplies(string name, int quantity, double weight) : base(name, quantity)
            { this.weight = weight; }
            public override bool parametrcheck(Delivery delivery)
            {
                return weight > maxweight;
            }

        }
        public class Food : Product
        {
            public DateOnly shelflife = DateOnly.FromDateTime(DateTime.Today);

            public Food(string name, DateOnly shelflife) : base(name)
            { this.shelflife = shelflife; }
            public Food(string name, int quantity, DateOnly shelflife) : base(name, quantity)
            { this.shelflife = shelflife; }
            public override bool parametrcheck(Delivery delivery)
            {
                return shelflife > delivery.Date;

            }

        }
        public class Customer
        {
            public string Name;
            public string TelephoneNumber;
            public DateOnly BirthDate;
            public Customer(string name, string telephoneNumber, DateOnly birthDate)
            {
                Name = name;
                TelephoneNumber = telephoneNumber;
                BirthDate = birthDate;
            }
            public bool ageCheck(DateOnly birthDate)
            {
                return DateTime.Today.Year - birthDate.Year > 18; }

        }
        public static class OrderApprove
        { public static bool orderapprove(bool productcheck, bool birthDateCheck)
            { return productcheck & birthDateCheck; }
        }
    }
 }

