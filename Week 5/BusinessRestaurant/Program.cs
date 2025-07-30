using System.Diagnostics;
using Data_Structure;

namespace BusinessRestaurant;

public abstract class Program
{
    static void Main(string[] args)
    {
        List<Customer> customers = new List<Customer>(2);
        List<User> users = new List<User>(2);
        List<ItemMaster> itemMasters = new List<ItemMaster>();
        List<Order> orders = new List<Order>();
        Customer customer1 = new Customer();
        User user1 = new User();
        Order order = new Order();
        ItemMaster itemMaster = new ItemMaster();
        OrderLine orderLine = new OrderLine();
        Item item;
        bool auth = false;
        User admin = new User("Ali Mohamed Ali", "alimohamedhassan9@gmail.com", "+97339266642", 11110821, 2001, 11, 29, 3, "ali");
        users.Add(admin);
        Console.WriteLine("Welcome to the restaurant!");
        Console.WriteLine("Choose an option below:");
        Console.WriteLine("(0) Exit");
        Console.WriteLine("(1) Register");
        Console.WriteLine("(2) Login");
        Console.Write("Answer: ");
        int answer1 = Convert.ToInt32(Console.ReadLine());
        if (answer1 == 1)
        {
            Console.Write("Please enter a name: ");
            string name1 = Console.ReadLine()!;
            Console.Write("Please enter a email: ");
            string  email1 = Console.ReadLine()!;
            Console.Write("Please enter a phone number: ");
            string phone1 = Console.ReadLine()!;
            Console.Write("Please enter an ID: ");
            long id1 = Convert.ToInt64(Console.ReadLine());
            Console.Write("Please enter a birthYear: ");
            int year1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter a birthMonth: ");
            int month1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter a birthDay: ");
            int day1 = Convert.ToInt32(Console.ReadLine());
            customer1 =  new Customer(name1, email1, phone1, id1, year1, month1, day1);
            customers.Add(customer1);
            Console.Write("Please enter a password: ");
            string pass1 =  Console.ReadLine()!;
            user1 = new User(name1, email1, phone1,  id1, year1, month1, day1, users.Count, pass1);
            users.Add(user1);
            Console.WriteLine(args);
            Console.WriteLine("Thank you for registering!");
            Console.WriteLine("We're giving you access to the system now.");
            auth = true;
        }
        else if (answer1 == 2)
        {
            Console.Write("Please enter your email: ");
            string email2 = Console.ReadLine()!;
            Console.Write("Please enter your password: ");
            string pass2 = Console.ReadLine()!;
            foreach (User myVariable in users)
            {
                if (myVariable.GetPassword().Equals(pass2) && myVariable.GetCustomer().GetEmail().Equals(email2))
                {
                    user1 = myVariable;
                    customer1 = user1.GetCustomer();
                    Console.WriteLine($"Welcome, {customer1.GetName()}");
                    Console.WriteLine("You have been logged in to the system.");
                    auth = true;
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("Thank you for using this system!");
            Console.Clear();
            Environment.Exit(0);
        }
        if (auth && user1.Equals(admin))
        {
            Console.WriteLine("Choose from the restaurant menu below: ");
            Console.WriteLine("(0) Exit");
            Console.WriteLine("(1) Add Order");
            Console.WriteLine("(2) View Orders");
            Console.WriteLine("(3) Delete Order");
            Console.WriteLine("(4) Search for an order");
            Console.WriteLine("(5) View Order Details");
            Console.WriteLine("(6) Clear Order");
            Console.WriteLine("(7) Clear Order Line");
            Console.WriteLine("(8) View Profile");
            Console.WriteLine("(9) Process Order");
            Console.WriteLine("(10) View First Order");
            Console.WriteLine("(11) View Last Order");
            Console.Write("Answer: ");
            int answer2 = Convert.ToInt32(Console.ReadLine());
            while (answer2 > 0)
            {
                switch (answer2)
                {
                    case 1:  
                        Console.Write("Please enter an item name: ");
                        string itemName1 =  Console.ReadLine()!;
                        Console.Write("Please enter an item category: ");
                        string itemCategory1 = Console.ReadLine()!;
                        item = new Item(name: itemName1, category: itemCategory1, itemId: itemMasters.Count);
                        Console.Write("Please enter an item quantity: ");
                        int itemQuantity1 = Convert.ToInt32(Console.ReadLine());
                        order = new Order(customer1.GetName(), customer1.GetEmail(), customer1.GetPhone(),
                            customer1.GetId(), customer1.GetDob().Year, customer1.GetDob().Month, customer1.GetDob().Day,
                            user1.GetUserId(), user1.GetPassword(), orders.Count, itemQuantity1);
                        orders.Add(order);
                        itemMaster = new ItemMaster(item, order, itemMasters.Count);
                        itemMasters.Add(itemMaster);
                        foreach (ItemMaster theVariable in itemMasters)
                        {
                            if (theVariable.GetMasterId() == item.GetItemId())
                            {
                                long newItemId = item.GetItemId() + 1;
                                item.SetItemId(newItemId);
                                theVariable.SetItemId(item); //Refer to Item Master class
                            }
                        }
                        orderLine.OfferOrder(order);
                        user1.AddOrder(order);
                        Console.WriteLine("Is this your first order?");
                        Console.WriteLine("yes or no?");
                        Console.Write("Answer: ");
                        string answer3 = Console.ReadLine()!;
                        if (answer3.Equals("yes"))
                        {
                            orderLine.OfferUser(user1);
                            Console.WriteLine("You have been added to the queue.");
                            Console.WriteLine("You can add more orders as long you're in the queue.");
                        }
                        else
                        {
                            Console.WriteLine("You have not been added to the queue because either you ordered previously or you did not type \"yes\" exactly as it is.");
                        }
                        Console.WriteLine($"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                        Console.WriteLine("yes or no?");
                        Console.Write("Answer: ");
                        string? orderAnswer = Console.ReadLine();
                        while (orderAnswer!.Equals("yes"))
                        {
                            Console.Write("Please enter an item name: ");
                            string itemName2 =  Console.ReadLine()!;
                            Console.Write("Please enter an item category: ");
                            string itemCategory2 = Console.ReadLine()!;
                            item = new Item(name: itemName2, category: itemCategory2, itemId: itemMasters.Count);
                            item.SetItemId(itemMasters.Count+1);
                            itemMaster = new ItemMaster(item, order, itemMasters.Count);
                            itemMasters.Add(itemMaster);
                            Console.WriteLine($"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                            Console.WriteLine("yes or no?");
                            Console.Write("Answer: ");
                            orderAnswer = Console.ReadLine();
                        }
                        Console.WriteLine("Congrats! We've received your order.");
                        Console.WriteLine("The order will be processed later.");
                        break;
                    case 2:
                        if (orders.Count <= 0)
                        {
                            Console.WriteLine("Order list is empty.");
                        }
                        else
                        {
                            orderLine.PrintLine();
                            int index = 0;
                            foreach (ItemMaster theMaster in itemMasters)
                            {
                                Console.Write($"{index}.");
                                theMaster.Display();
                                index++;
                            }
                        }
                        break;
                    case 3:
                        List<Int32> pendings = new List<int>();
                        List<ItemMaster> penders = new List<ItemMaster>();
                        Console.Write("Please enter the order ID: ");
                        long orderId2 = Convert.ToInt64(Console.ReadLine());
                        bool success = false;
                        bool breaker = false;
                        foreach (ItemMaster myItem in itemMasters)
                        {
                            if (myItem.GetOrder().GetOrderId() == orderId2)
                            {
                                Console.WriteLine("Would you like to delete item only?");
                                Console.WriteLine("yes or no?");
                                Console.Write("Answer: ");
                                string? itemAnswer = Console.ReadLine();
                                while (itemAnswer!.Equals("yes"))
                                {
                                    int indexer = 0;
                                    Console.WriteLine($"List of items in the order with id {orderId2}: ");
                                    while (myItem.GetOrder().GetOrderId() == orderId2)
                                    {
                                        Console.Write($"{indexer}. ");
                                        myItem.GetItem().Display();
                                        indexer++;
                                    }
                                    Console.Write("\nSelect the item by typing the number: ");
                                    int itemSelectorX = Convert.ToInt32(Console.ReadLine());
                                    if (myItem.GetItem().GetItemId() == itemSelectorX)
                                    {
                                        pendings.Add(itemSelectorX);
                                        penders.Add(myItem);
                                        Console.WriteLine("Your item will be deleted soon.");
                                        breaker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("An error occured!");
                                        Console.WriteLine("Your item will not be deleted.");
                                        if (penders.Count == 0 && pendings.Count == 0)
                                        {
                                            breaker = false;
                                        }
                                    }
                                    Console.WriteLine("Would you like to delete another item?");
                                    Console.WriteLine("yes or no?");
                                    Console.Write("Answer: ");
                                    itemAnswer = Console.ReadLine();
                                }
                                if (!breaker)
                                {
                                    order = myItem.GetOrder();
                                    itemMaster = myItem;
                                    Order emptyOrder = new Order();
                                    myItem.SetOrder(emptyOrder);
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("Your order will not be deleted upon your request.");
                                    success = false;
                                }
                                break;
                            }
                        }
                        if (!breaker)
                        {
                            Order operation = user1.DeleteOrder(order);
                            bool isRemoved = orders.Remove(order);
                            bool removed = itemMasters.Remove(itemMaster);
                            if (success && isRemoved && !operation.Equals(null) && removed)
                            {
                                Console.WriteLine("The order have been successfully deleted.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to delete the order.");
                            }
                        }
                        else
                        {
                            bool isDone = false;
                            for (int j = 0; j < pendings.Count; j++)
                            {
                                if (penders[j].GetItem().GetItemId() == pendings[j])
                                {
                                    itemMasters.Remove(penders[j]);
                                    isDone = true;
                                }
                            }
                            if (!isDone)
                            {
                                int sizer = pendings.Count;
                                while (sizer-- > 0)
                                {
                                    penders.RemoveAt(sizer);
                                    pendings.RemoveAt(sizer);
                                }
                                Console.WriteLine("The items upon your request has been removed.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to remove an item only.");
                            }
                        }
                        break;
                    case 4:
                        Dictionary<int, bool> theDictionary = new Dictionary<int, bool>();
                        Console.Write("Please enter the order ID: ");
                        long orderId1 = Convert.ToInt64(Console.ReadLine());
                        foreach (Order theItem in orders)
                        {
                            if (theItem.GetOrderId() == orderId1)
                            {
                                order = theItem;
                                Console.WriteLine($"The order is found at location {orderId1} in the main list.");
                                theDictionary[0] = true;
                                break;
                            }
                        }
                        foreach (ItemMaster myItem in itemMasters)
                        {
                            if (myItem.GetOrder().Equals(order))
                            {
                                Console.WriteLine($"The order is found at location {myItem.GetMasterId()} in item master list.");
                                theDictionary[1] = true;
                                break;
                            }
                        }
                        int theSearcher = user1.SearchOrder(order);
                        if (theSearcher > -1)
                        {
                            Console.WriteLine($"The order is found at location {user1.SearchOrder(order)} in your order list.");
                            theDictionary[2] = true;
                        }
                        if (!theDictionary[0] && !theDictionary[1] && !theDictionary[2])
                        {
                            Console.WriteLine("The order is not found!");
                        }
                        break;
                    case 5: 
                        Console.Write("Please enter a number: ");
                        int num1 = Convert.ToInt32(Console.ReadLine());
                        user1.GetOrder(num1).Display();
                        foreach (var t in itemMasters)
                        {
                            if (t.GetOrder().GetOrderId() == num1)
                            {
                                t.GetItem().Display();
                                break;
                            }
                        }
                        break;
                    case 6:
                        user1.ClearOrder();
                        break;
                    case 7:  
                        orderLine.ClearOrderLine();
                        break;
                    case 8:
                        Console.WriteLine("Details found: ");
                        foreach (User myVariable in users)
                        {
                            if (myVariable.Equals(user1))
                            {
                                myVariable.Display();
                                break;
                            }
                        }
                        break;
                    case 9:
                        User servedUser = user1;
                        Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
                        foreach (Order myItem in orders)
                        {
                            if (orderLine.GetOrder().Equals(myItem))
                            {
                                order = myItem;
                                dictionary[0] = true;
                                break;
                            }
                        }
                        Order isDeleted = user1.DeleteOrder(order);
                        if (!isDeleted.Equals(null) && isDeleted.Equals(order))
                        {
                            dictionary[1] = true;
                        }
                        if (dictionary[0])
                        {
                            orders.Remove(order);
                        }
                        if (!dictionary[1])
                        {
                            servedUser = orderLine.PollUser();
                        }
                        Order servedOrder = orderLine.PollOrder();
                        Console.WriteLine($"The order {servedOrder.GetOrderId()} has been served to {servedUser.GetCustomer().GetName()}.");
                        break;
                    case 10:
                        orderLine.GetOrder().Display(); //This function works when first order in the Queue exists
                        break;
                    case 11:
                        orderLine.CheckLastOrder().Display(); //This function works when the order gets processed from the Queue
                        break;
                }
                Console.WriteLine("Choose from the restaurant menu below: ");
                Console.WriteLine("(0) Exit");
                Console.WriteLine("(1) Add Order");
                Console.WriteLine("(2) View Orders");
                Console.WriteLine("(3) Delete Order");
                Console.WriteLine("(4) Search for an order");
                Console.WriteLine("(5) View Order Details");
                Console.WriteLine("(6) Clear Order");
                Console.WriteLine("(7) Clear Order Line");
                Console.WriteLine("(8) View Profile");
                Console.WriteLine("(9) Process Order");
                Console.WriteLine("(10) View First Order");
                Console.WriteLine("(11) View Last Order");
                Console.Write("Answer: ");
                answer2 = Convert.ToInt32(Console.ReadLine());
            }
        }
        else if (auth)
        {
            Console.WriteLine("Choose from the restaurant menu below: ");
            Console.WriteLine("(0) Exit");
            Console.WriteLine("(1) Add Order");
            Console.WriteLine("(2) View Orders");
            Console.WriteLine("(3) Delete Order");
            Console.WriteLine("(4) Search for an order");
            Console.WriteLine("(5) View Order Details");
            Console.WriteLine("(6) View Profile");
            Console.WriteLine("(7) Process Order");
            Console.WriteLine("(8) View First Order");
            Console.Write("Answer: ");
            int answer2 = Convert.ToInt32(Console.ReadLine());
            while (answer2 > 0)
            {
                switch (answer2)
                {
                    case 1:  
                        Console.Write("Please enter an item name: ");
                        string itemName1 =  Console.ReadLine()!;
                        Console.Write("Please enter an item category: ");
                        string itemCategory1 = Console.ReadLine()!;
                        item = new Item(name: itemName1, category: itemCategory1, itemId: itemMasters.Count);
                        Console.Write("Please enter an item quantity: ");
                        int itemQuantity1 = Convert.ToInt32(Console.ReadLine());
                        order = new Order(customer1.GetName(), customer1.GetEmail(), customer1.GetPhone(),
                            customer1.GetId(), customer1.GetDob().Year, customer1.GetDob().Month, customer1.GetDob().Day,
                            user1.GetUserId(), user1.GetPassword(), orders.Count, itemQuantity1);
                        orders.Add(order);
                        itemMaster = new ItemMaster(item, order, itemMasters.Count);
                        itemMasters.Add(itemMaster);
                        foreach (ItemMaster theVariable in itemMasters)
                        {
                            if (theVariable.GetMasterId() == item.GetItemId())
                            {
                                long newItemId = item.GetItemId() + 1;
                                item.SetItemId(newItemId);
                                theVariable.SetItemId(item); //Refer to Item Master class
                            }
                        }
                        orderLine.OfferOrder(order);
                        user1.AddOrder(order);
                        Console.WriteLine("Is this your first order?");
                        Console.WriteLine("yes or no?");
                        Console.Write("Answer: ");
                        string answer3 = Console.ReadLine()!;
                        if (answer3.Equals("yes"))
                        {
                            orderLine.OfferUser(user1);
                            Console.WriteLine("You have been added to the queue.");
                            Console.WriteLine("You can add more orders as long you're in the queue.");
                        }
                        else
                        {
                            Console.WriteLine("You have not been added to the queue because either you ordered previously or you did not type \"yes\" exactly as it is.");
                        }
                        Console.WriteLine($"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                        Console.WriteLine("yes or no?");
                        Console.Write("Answer: ");
                        string? orderAnswer = Console.ReadLine();
                        while (orderAnswer!.Equals("yes"))
                        {
                            Console.Write("Please enter an item name: ");
                            string itemName2 =  Console.ReadLine()!;
                            Console.Write("Please enter an item category: ");
                            string itemCategory2 = Console.ReadLine()!;
                            item = new Item(name: itemName2, category: itemCategory2, itemId: itemMasters.Count);
                            item.SetItemId(itemMasters.Count+1);
                            itemMaster = new ItemMaster(item, order, itemMasters.Count);
                            itemMasters.Add(itemMaster);
                            Console.WriteLine($"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                            Console.WriteLine("yes or no?");
                            Console.Write("Answer: ");
                            orderAnswer = Console.ReadLine();
                        }
                        Console.WriteLine("Congrats! We've received your order.");
                        Console.WriteLine("The order will be processed later.");
                        break;
                    case 2:
                        if (orders.Count <= 0)
                        {
                            Console.WriteLine("Order list is empty.");
                        }
                        else
                        {
                            orderLine.PrintLine();
                            int index = 0;
                            foreach (ItemMaster theMaster in itemMasters)
                            {
                                Console.Write($"{index}.");
                                if (user1.GetOrder((int)theMaster.GetOrder().GetOrderId()).Equals(theMaster.GetOrder()) && !user1.GetOrder((int)theMaster.GetOrder().GetOrderId()).Equals(null))
                                {
                                    theMaster.GetOrder().Display();
                                    theMaster.GetItem().Display();
                                }
                            }
                        }
                        break;
                    case 3:
                        List<Int32> pendings = new List<int>();
                        List<ItemMaster> penders = new List<ItemMaster>();
                        Console.Write("Please enter the order ID: ");
                        long orderId2 = Convert.ToInt64(Console.ReadLine());
                        bool success = false;
                        bool breaker = false;
                        foreach (ItemMaster myItem in itemMasters)
                        {
                            if (myItem.GetOrder().GetOrderId() == orderId2)
                            {
                                Console.WriteLine("Would you like to delete item only?");
                                Console.WriteLine("yes or no?");
                                Console.Write("Answer: ");
                                string? itemAnswer = Console.ReadLine();
                                while (itemAnswer!.Equals("yes"))
                                {
                                    int indexer = 0;
                                    Console.WriteLine($"List of items in the order with id {orderId2}: ");
                                    while (myItem.GetOrder().GetOrderId() == orderId2)
                                    {
                                        Console.Write($"{indexer}. ");
                                        myItem.GetItem().Display();
                                        indexer++;
                                    }
                                    Console.Write("\nSelect the item by typing the number: ");
                                    int itemSelectorX = Convert.ToInt32(Console.ReadLine());
                                    if (myItem.GetItem().GetItemId() == itemSelectorX)
                                    {
                                        pendings.Add(itemSelectorX);
                                        penders.Add(myItem);
                                        Console.WriteLine("Your item will be deleted soon.");
                                        breaker = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("An error occured!");
                                        Console.WriteLine("Your item will not be deleted.");
                                        if (penders.Count == 0 && pendings.Count == 0)
                                        {
                                            breaker = false;
                                        }
                                    }
                                    Console.WriteLine("Would you like to delete another item?");
                                    Console.WriteLine("yes or no?");
                                    Console.Write("Answer: ");
                                    itemAnswer = Console.ReadLine();
                                }
                                if (!breaker)
                                {
                                    order = myItem.GetOrder();
                                    itemMaster = myItem;
                                    Order emptyOrder = new Order();
                                    myItem.SetOrder(emptyOrder);
                                    success = true;
                                }
                                else
                                {
                                    Console.WriteLine("Your order will not be deleted upon your request.");
                                    success = false;
                                }
                                break;
                            }
                        }
                        if (!breaker)
                        {
                            Order operation = user1.DeleteOrder(order);
                            bool isRemoved = orders.Remove(order);
                            bool removed = itemMasters.Remove(itemMaster);
                            if (success && isRemoved && !operation.Equals(null) && removed)
                            {
                                Console.WriteLine("The order have been successfully deleted.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to delete the order.");
                            }
                        }
                        else
                        {
                            bool isDone = false;
                            for (int j = 0; j < pendings.Count; j++)
                            {
                                if (penders[j].GetItem().GetItemId() == pendings[j])
                                {
                                    itemMasters.Remove(penders[j]);
                                    isDone = true;
                                }
                            }
                            if (!isDone)
                            {
                                int sizer = pendings.Count;
                                while (sizer-- > 0)
                                {
                                    penders.RemoveAt(sizer);
                                    pendings.RemoveAt(sizer);
                                }
                                Console.WriteLine("The items upon your request has been removed.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to remove an item only.");
                            }
                        }
                        break;
                    case 4:
                        Dictionary<int, bool> theDictionary = new Dictionary<int, bool>();
                        Console.Write("Please enter the order ID: ");
                        long orderId1 = Convert.ToInt64(Console.ReadLine());
                        foreach (Order theItem in orders)
                        {
                            if (theItem.GetOrderId() == orderId1)
                            {
                                order = theItem;
                                Console.WriteLine($"The order is found at location {orderId1} in the main list.");
                                theDictionary[0] = true;
                                break;
                            }
                        }
                        foreach (ItemMaster myItem in itemMasters)
                        {
                            if (myItem.GetOrder().Equals(order))
                            {
                                Console.WriteLine($"The order is found at location {myItem.GetMasterId()} in item master list.");
                                theDictionary[1] = true;
                                break;
                            }
                        }
                        int theSearcher = user1.SearchOrder(order);
                        if (theSearcher > -1)
                        {
                            Console.WriteLine($"The order is found at location {user1.SearchOrder(order)} in your order list.");
                            theDictionary[2] = true;
                        }
                        if (!theDictionary[0] && !theDictionary[1] && !theDictionary[2])
                        {
                            Console.WriteLine("The order is not found!");
                        }
                        break;
                    case 5: 
                        Console.Write("Please enter a number: ");
                        int num1 = Convert.ToInt32(Console.ReadLine());
                        user1.GetOrder(num1).Display();
                        foreach (var t in itemMasters)
                        {
                            if (t.GetOrder().GetOrderId() == num1)
                            {
                                t.GetItem().Display();
                                break;
                            }
                        }
                        break;
                    case 6:
                        Console.WriteLine("Details found: ");
                        foreach (Customer variable in customers)
                        {
                            if (variable.Equals(customer1))
                            {
                                variable.Display();
                                break;
                            }
                        }
                        foreach (User myVariable in users)
                        {
                            if (myVariable.Equals(user1))
                            {
                                Console.WriteLine($"Password: {myVariable.GetPassword()}");
                                break;
                            }
                        }
                        break;
                    case 7:
                        User servedUser = user1;
                        Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
                        foreach (Order myItem in orders)
                        {
                            if (orderLine.GetOrder().Equals(myItem))
                            {
                                order = myItem;
                                dictionary[0] = true;
                                break;
                            }
                        }
                        Order isDeleted = user1.DeleteOrder(order);
                        if (!isDeleted.Equals(null) && isDeleted.Equals(order))
                        {
                            dictionary[1] = true;
                        }
                        if (dictionary[0])
                        {
                            orders.Remove(order);
                        }
                        if (!dictionary[1])
                        {
                            servedUser = orderLine.PollUser();
                        }
                        Order servedOrder = orderLine.PollOrder();
                        Console.WriteLine($"The order {servedOrder.GetOrderId()} has been served to {servedUser.GetCustomer().GetName()}.");
                        break;
                    case 8:
                        orderLine.GetOrder().Display(); //This function works when first order in the Queue exists
                        break;
                }
                Console.WriteLine("Choose from the restaurant menu below: ");
                Console.WriteLine("(0) Exit");
                Console.WriteLine("(1) Add Order");
                Console.WriteLine("(2) View Orders");
                Console.WriteLine("(3) Delete Order");
                Console.WriteLine("(4) Search for an order");
                Console.WriteLine("(5) View Order Details");
                Console.WriteLine("(6) View Profile");
                Console.WriteLine("(7) Process Order");
                Console.WriteLine("(8) View First Order");
                Console.Write("Answer: ");
                answer2 = Convert.ToInt32(Console.ReadLine());
            }
        }
        else
        {
            Console.WriteLine("Incorrect email or password.");
            Console.WriteLine("You're about to exit this system.");
            Console.WriteLine("Restart this system to login again.");
            Console.WriteLine("Thank you for using this system!");
            Process.GetCurrentProcess().Kill();
        }
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
    public override string ToString()
    {
        return $"Customer details: \nName: {GetName()} \nEmail: {GetEmail()} \nPhone number: {GetPhone()} \nAge: {CalculateAge()}";
    }
}
public class User
{
    private long _userId;
    private string _password;
    private readonly Customer _customer;
    private readonly LWLinkedList<Order> _orders = new LWLinkedList<Order>();

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
    public void AddOrder(Order order)
    {
        _orders.add(order);
    }

    public Order DeleteOrder(Order order)
    {
        Order? backup = _orders.get(_orders.search(order));
        _orders.remove(order);
        for (int i = 0; i < _orders.getSize(); i++)
        {
            if (_orders.get(i).Equals(order))
            {
                backup = null;
                break;
            }
        }
        return backup!;
    }

    public Order ReplaceOrder(Order oldOrder, Order newOrder)
    {
        Order? backup = null;
        _orders.replace(oldOrder, newOrder);
        for (int i = 0; i < _orders.getSize(); i++)
        {
            if (_orders.get(i).Equals(newOrder))
            {
                backup = oldOrder;
                break;
            }
            if (_orders.get(i).Equals(oldOrder))
            {
                backup = null;
                break;
            }
        }
        return backup ?? oldOrder;
    }
    public int SearchOrder(Order item)
    {
        return _orders.search(item);
    }
    public Order GetOrder(int index)
    {
        return _orders.get(index);
    }
    public void ClearOrder()
    {
        while (_orders.isEmpty())
        {
            _orders.remove(_orders.getLast());
        }
        Console.WriteLine("All orders have been cleared!");
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
    public override string ToString()
    {
        return $"{GetCustomer()} \nPassword: {GetPassword()}";
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
    private string GetName()
    {
        return _name;
    }
    private string GetCategory()
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
    public override string ToString()
    {
        return $"Item Information: \nName: {GetName()} \nCategory: {GetCategory()}\nID: {GetItemId()}";
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
    private User GetUser()
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
    public override string ToString()
    {
        return $"Order Information: \nID: {GetOrderId() } \nQuantity: {GetQuantity()} \nUser: {GetUser()}";
    }
}

public class ItemMaster(Item item, Order order, long masterId)
{
    private Item _item = item;
    private Order _order = order;
    private long _masterId = masterId;
    private readonly DateTime _date = DateTime.Now;

    public ItemMaster() : this(new Item(), new Order(), 0)
    {
    }

    //This SetItemId changes the whole item, not just the item id
    public void SetItemId(Item item)
    {
        _item = item;
    }
    public Item GetItem()
    {
        return _item;
    }
    public Order GetOrder()
    {
        return _order;
    }
    public long GetMasterId()
    {
        return _masterId;
    }
    private DateTime GetDate()
    {
        return _date;
    }
    public void SetOrder(Order order)
    {
        _order = order;
    }
    public void SetMasterId(long masterId)
    {
        _masterId = masterId;
    }
    public void Display()
    {
        Console.WriteLine();
        Console.WriteLine("Item Master Information: ");
        Console.WriteLine($"Item Master ID: {GetMasterId()}");
        GetItem().Display();
        GetOrder().Display();
        Console.WriteLine($"Created on: {GetDate()}");
    }
    public override string ToString()
    {
        return $"Item Master Information: \nID: {GetMasterId()} \nItem: {GetItem()} \nOrder: {GetOrder()} \nCreated On: {GetDate()}";
    }
}
public class OrderLine
{
    private DateTime _historyDate = DateTime.Now;
    private readonly System.Collections.Generic.Queue<User> _users = new();
    private readonly Data_Structure.Queue<Order> _orders = new();
    private readonly Data_Structure.Stack<Order> _orderHistory = new();

    private DateTime GetHistoryDate()
    {
        return _historyDate;
    }
    public void OfferUser(User user)
    {
        _users.Enqueue(user);
    }
    public void OfferOrder(Order order)
    {
        _orders.offer(order);
    }
    //Cannot check all users due to Queue policy
    private User GetUser()
    {
        return _users.Peek();
    }
    public User PollUser()
    {
        return _users.Dequeue();
    }
    //Cannot check all orders due to Queue policy
    public Order GetOrder()
    {
        return _orders.isEmpty() ? new Order() : _orders.element();
    }
    private void PushRemovedOrder(Order order)
    {
        _orderHistory.push(order);
    }
    public Order PollOrder()
    {
        PushRemovedOrder(GetOrder());
        return _orders.take();
    }
    //Do this before ending the main program
    private void ClearHistory()
    {
        while (!_orderHistory.isEmpty())
        {
            _orderHistory.pop();
        }
        Console.WriteLine("All order histories have been cleared.");
    }
    //Just checking the last archived order
    //Not allowed to check all archived orders due to Stack policy
    public Order CheckLastOrder()
    {
        return _orderHistory.isEmpty() ? new Order() : _orderHistory.peek();
    }
    public void ClearOrderLine()
    {
        ClearHistory();
        ClearOrders();
        ClearUsers();
        CorruptHistoryDate();
        Console.WriteLine("The order line has been cleared.");
    }
    private void ClearOrders()
    {
        while (!_orders.isEmpty())
        {
            PollOrder();
        }
        Console.WriteLine("All orders have been cleared.");
    }
    private void ClearUsers()
    {
        _users.Clear();
        Console.WriteLine("All users have been cleared.");
    }
    private void CorruptHistoryDate()
    {
        _historyDate = new DateTime(9);
    }
    public void PrintLine()
    {
        Console.WriteLine();
        Console.WriteLine("Order Line: ");
        _orders.printQueue(); //Replace it with SQL Query
        Console.WriteLine("Users:");
        int counter = 0;
        foreach (User user in _users)
        {
            Console.Write($"{counter}. ");
            user.Display();
        }
        Console.WriteLine("Order History:");
        _orderHistory.printStack();
        Console.WriteLine($"Created On: {GetHistoryDate()}");
        Console.WriteLine($"Created By: {GetUser().GetCustomer().GetName()}");
        Console.WriteLine();
    }
}