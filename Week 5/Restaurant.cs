using System;
using System.Threading;
using System.Reflection;
using System.Runtime;
using System.Numerics;

namespace RestaurantApp
{
    internal class TheMain
    {
        Assembly assembly = new Assembly();
        static void main(String[] args)
        {
            DateTime myDate = new DateTime();
            Console.WriteLine(myDate);
            Date myDate1 = new Date();
            Console.WriteLine(myDate1);
            Console.WriteLine(assembly);
        }
    }
    public class Customer
    {
        private string _name, _email, _phone;
        private long _id;
        private DateTime _dob;

        public Customer()
        {
            _name = "";
            _email = "";
            _phone = "";
            _id = 0;
            _dob = DateTime.Now;
        }

        public Customer(string name, string email, string phone, long id, int year, int month, int day)
        {
            _name = name;
            _email = email;
            _phone = phone;
            _id = Math.Max(0, id);
            _dob = new DateTime(year, month, day);
        }

        public void SetName(string name)
        {
            _name = name;
        }
        public void SetId(long id)
        {
            _id = Math.Max(id, 0);
        }
        public void SetEmail(string email)
        {
            _email = email;
        }
        public void SetPhone(string phone)
        {
            _phone = phone;
        }
        public void SetDob(DateTime dob)
        {
            _dob = dob;
        }
        public string GetName()
        {
            return _name;
        }
        public string GetEmail()
        {
            return _email;
        }
        public string GetPhone()
        {
            return _phone;
        }
        public DateTime GetDob()
        {
            return _dob;
        }
        private int CalculateAge()
        {
            DateTime current = DateTime.Now;
            return (_dob.Month < current.Month) ? (current.Year - _dob.Year) - 1 : current.Year - _dob.Year;
        }
        public long GetId()
        {
            return _id;
        }
        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Customer details: ");
            Console.WriteLine($"Name: {GetName()}");
            Console.WriteLine($"ID: {GetId()}");
            Console.WriteLine($"Email: {GetEmail()}");
            Console.WriteLine($"Phone number: {GetPhone()}");
            Console.WriteLine($"Date of birth: {GetDob()}");
            Console.WriteLine($"Age: {CalculateAge()}");
        }
    }
    public class User
    {
        private long _userId;
        private string _password;
        private readonly Customer _customer;

        public User()
        {
            _customer = new Customer();
            _password = "";
            _userId = 0;
        }

        public User(string name, string email, string phone, long id, int year, int month, int day, long userId, string password)
        {
            _customer = new Customer(name, email, phone, id, year, month, day);
            _userId = Math.Max(userId, 0);
            _password = password;
        }

        public void SetUserId(long id)
        {
            _userId = Math.Max(id, 0);
        }
        public void SetPassword(string password)
        {
            _password = password;
        }
        public void SetCustomer(string name, string email, string phone, long id, DateTime dateTime)
        {
            _customer.SetDob(dateTime);
            _customer.SetEmail(email);
            _customer.SetName(name);
            _customer.SetPhone(phone);
            _customer.SetId(id);
        }
        public long GetUserId()
        {
            return _userId;
        }
        public Customer GetCustomer()
        {
            return _customer;
        }
        public string GetPassword()
        {
            return _password;
        }
        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("User Information: ");
            Console.WriteLine($"User ID: {GetUserId()}");
            Console.WriteLine($"Email: {GetCustomer().GetEmail()}");
            Console.WriteLine($"Password: {GetPassword()}");
            GetCustomer().Display();
        }
    }
    public class Item
    {
        private string _name, _category;
        private long _itemId;

        public Item()
        {
            _name = "";
            _category = "";
            _itemId = 0;
        }

        public Item(string name, string category, long itemId)
        {
            _category = category;
            _name = name;
            _itemId = Math.Max(itemId, 0);
        }

        public void SetItemId(long itemId)
        {
            _itemId = Math.Max(itemId, 0);
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public void SetCategory(string category)
        {
            _category = category;
        }
        public string GetName()
        {
            return _name;
        }
        public string GetCategory()
        {
            return _category;
        }
        public long GetItemId()
        {
            return _itemId;
        }
        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Item Information: ");
            Console.WriteLine($"Name: {GetName()}");
            Console.WriteLine($"Category: {GetCategory()}");
            Console.WriteLine($"ID: {GetItemId()}");
        }
    }
    public class Order
    {
        private long _orderId;
        private int _quantity;
        private User _user;

        public Order()
        {
            _orderId = 0;
            _quantity = 0;
            _user = new User();
        }

        public Order(string name, string email, string phone, long id, int year, int month, int day, long userId, string password, long orderId, int quantity)
        {
            _orderId = Math.Max(orderId, 0);
            _quantity = Math.Max(quantity, 0);
            _user = new User(name, email, phone, id, year, month, day, userId, password);
        }
        
        public void SetOrderId(long orderId)
        {
            _orderId = orderId;
        }
        public void SetQuantity(int quantity)
        {
            _quantity = Math.Max(quantity, 0);
        }
        public void SetUser(User user)
        {
            _user = user;
        }
        public long GetOrderId()
        {
            return _orderId;
        }
        public int GetQuantity()
        {
            return _quantity;
        }
        public User GetUser()
        {
            return  _user;
        }
        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("Order Information: ");
            Console.WriteLine($"Order ID: {GetOrderId()}");
            Console.WriteLine($"Quantity: {GetQuantity()}");
            GetUser().Display();
        }
    }
}