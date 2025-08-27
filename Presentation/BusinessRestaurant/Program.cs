using System.Data.Common;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using System.Diagnostics;
using Data_Structure;

namespace BusinessRestaurant;

internal abstract class Program
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
        bool[] authenticate = [false, false, false];
        User admin = new User("Ali Mohamed Ali", "alimohamedhassan9@gmail.com", "+97339266642", 11110821, 2001, 11, 29, 3, "ali");
        users.Add(admin);
        Console.WriteLine("Welcome to the restaurant!");
        Console.WriteLine("Choose an option below:");
        Console.WriteLine("(0) Exit");
        Console.WriteLine("(1) Register");
        Console.WriteLine("(2) Login");
        Console.Write("Answer: ");
        try
        {
            int answer1 = Convert.ToInt32(Console.ReadLine());
            string connectionString = "server=aqwe443346;database=therestaurant;user=another_4433;password=sha256;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            Console.WriteLine("Database connection opened successfully!");
            if (answer1 == 1)
            {
                Console.Write("Please enter a name: ");
                string name1 = Console.ReadLine()!;
                SqlString sqlName1 = new SqlString(name1);
                Console.Write("Please enter a email: ");
                string email1 = Console.ReadLine()!;
                SqlString sqlEmail1 = new SqlString(email1);
                Console.Write("Please enter a phone number: ");
                string phone1 = Console.ReadLine()!;
                SqlString sqlPhone1 = new SqlString(phone1);
                Console.Write("Please enter an ID: ");
                long id1 = Convert.ToInt64(Console.ReadLine());
                SqlSingle sqlId1 = id1;
                Console.Write("Please enter a birthYear: ");
                int year1 = Convert.ToInt32(Console.ReadLine());
                SqlSingle sqlYear1 = year1;
                Console.Write("Please enter a birthMonth: ");
                int month1 = Convert.ToInt32(Console.ReadLine());
                SqlSingle sqlMonth1 = month1;
                Console.Write("Please enter a birthDay: ");
                int day1 = Convert.ToInt32(Console.ReadLine());
                SqlSingle sqlDay1 = day1;
                DateTime dateTime =
                    new DateTime((int)sqlYear1.Value, (int)sqlMonth1.Value, (int)sqlDay1.Value, 12, 0, 0);
                customer1 = new Customer(name1, email1, phone1, id1, year1, month1, day1);
                try
                {
                    using (MySqlCommand sqlCommand1 =
                           new MySqlCommand("INSERT INTO Customer VALUES (@Name, @Id, @BirthDate, @Email, @Phone)",
                               conn))
                    {
                        sqlCommand1.Parameters.AddWithValue("@Name", sqlName1.Value);
                        sqlCommand1.Parameters.AddWithValue("@Id", sqlId1.Value);
                        sqlCommand1.Parameters.AddWithValue("@BirthDate", dateTime.Date);
                        sqlCommand1.Parameters.AddWithValue("@Email", sqlEmail1.Value);
                        sqlCommand1.Parameters.AddWithValue("@Phone", sqlPhone1.Value);
                        sqlCommand1.Prepare();
                        int rows = sqlCommand1.ExecuteNonQuery();
                        if (rows == 1)
                        {
                            Console.WriteLine($"Operation successful with {rows} row affected.");
                            authenticate[2] = true;
                        }
                        else if (rows > 1)
                        {
                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                            authenticate[2] = true;
                        }
                        else
                        {
                            Console.WriteLine("There is a problem with database manipulation!");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to execute an operation in database!");
                }
                customers.Add(customer1);
                Console.Write("Please enter a password: ");
                string pass1 = Console.ReadLine()!;
                SqlString sqlPass1 = new SqlString(pass1);
                user1 = new User(name1, email1, phone1, id1, year1, month1, day1, users.Count, pass1);
                try
                {
                    using (MySqlCommand sqlCommand2 =
                           new MySqlCommand(
                               "INSERT INTO RestaurantUser VALUES (@UserCount, @Password, @Id)", conn))
                    {
                        sqlCommand2.Parameters.AddWithValue("@UserCount", users.Count);
                        sqlCommand2.Parameters.AddWithValue("@Password", sqlPass1.Value);
                        sqlCommand2.Parameters.AddWithValue("@Id", sqlId1.Value);
                        sqlCommand2.Prepare();
                        int rows = sqlCommand2.ExecuteNonQuery();
                        if (rows == 1)
                        {
                            Console.WriteLine($"Operation successful with {rows} row affected.");
                            authenticate[1] = true;
                        }
                        else if (rows > 1)
                        {
                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                            authenticate[1] = true;
                        }
                        else
                        {
                            Console.WriteLine("There is a problem with database manipulation!");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to execute an operation from the database!");
                }
                users.Add(user1);
                Console.WriteLine(args);
                Console.WriteLine(customers);
                Console.WriteLine("Thank you for registering!");
                Console.WriteLine("We're giving you access to the system now.");
                authenticate[0] = true;
            }
            else if (answer1 == 2)
            {
                Console.Write("Please enter your email: ");
                string email2 = Console.ReadLine()!;
                SqlString sqlEmail2 = new SqlString(email2);
                Console.Write("Please enter your password: ");
                string pass2 = Console.ReadLine()!;
                SqlString sqlPass2 = new SqlString(pass2);
                try
                {
                    using (MySqlCommand theCommand = new MySqlCommand("SELECT * FROM RestaurantUser", conn))
                    {
                        theCommand.Prepare();
                        using (MySqlDataReader reader = theCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["Password"].Equals(sqlPass2.Value))
                                {
                                    Console.WriteLine("Account found!");
                                    user1.SetPassword(reader["Password"].ToString()!);
                                    users.Add(user1);
                                    authenticate[1] = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Account not found!");
                    Console.WriteLine("Failed to execute an operation in the database by SQL command!");
                }
                try
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM Customer", conn))
                    {
                        command.Prepare();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["Email"].Equals(sqlEmail2.Value))
                                {
                                    User oldUser = user1;
                                    Console.WriteLine("Account found!");
                                    user1.GetCustomer().SetEmail(reader["Email"].ToString()!);
                                    user1.GetCustomer().SetName(reader["Name"].ToString()!);
                                    user1.GetCustomer().SetPhone(reader["Phone"].ToString()!);
                                    user1.GetCustomer().SetId(Convert.ToInt64(reader["Id"]));
                                    users.Remove(oldUser);
                                    users.Add(user1);
                                    authenticate[2] = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to execute an operation in the database by SQL command!");
                    Console.WriteLine("Account not found!");
                }
                foreach (User myVariable in users)
                {
                    if (myVariable.GetPassword().Equals(pass2) && myVariable.GetCustomer().GetEmail().Equals(email2))
                    {
                        user1 = myVariable;
                        customer1 = user1.GetCustomer();
                        Console.WriteLine($"Welcome, {customer1.GetName()}");
                        Console.WriteLine("You have been logged in to the system.");
                        authenticate[0] = true;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("You are about to exit this system.");
                Console.WriteLine("Thank you for using this system!");
                conn.Close();
                Environment.Exit(0);
            }
            if ((authenticate[0] || authenticate[1] || authenticate[2]) && (user1.Equals(admin) || user1.GetCustomer().GetId() == admin.GetCustomer().GetId()))
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
                            string itemName1 = Console.ReadLine()!;
                            SqlString sqlItemName1 = new SqlString(itemName1);
                            Console.Write("Please enter an item category: ");
                            string itemCategory1 = Console.ReadLine()!;
                            SqlString sqlItemCategory1 = new SqlString(itemCategory1);
                            item = new Item(name: itemName1, category: itemCategory1, itemId: itemMasters.Count);
                            try
                            {
                                using (MySqlCommand itemCommand = new MySqlCommand(
                                           "INSERT INTO Item VALUES (@Count, @Name, @Category)", conn))
                                {
                                    itemCommand.Parameters.AddWithValue("@Count", itemMasters.Count);
                                    itemCommand.Parameters.AddWithValue("@Name", sqlItemName1.Value);
                                    itemCommand.Parameters.AddWithValue("@Category", sqlItemCategory1.Value);
                                    itemCommand.Prepare();
                                    var rows = itemCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException e)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine($"Message: \n{e.Message}");
                            }
                            Console.Write("Please enter an item quantity: ");
                            int itemQuantity1 = Convert.ToInt32(Console.ReadLine());
                            SqlSingle sqlItemQuantity1 = new SqlSingle(itemQuantity1);
                            order = new Order(customer1.GetName(), customer1.GetEmail(), customer1.GetPhone(),
                                customer1.GetId(), customer1.GetDob().Year, customer1.GetDob().Month,
                                customer1.GetDob().Day,
                                user1.GetUserId(), user1.GetPassword(), orders.Count, itemQuantity1);
                            try
                            {
                                using (MySqlCommand orderCommand = new MySqlCommand(
                                           "INSERT INTO RestaurantOrder VALUES (@Count, @Quantity, @UserId)",
                                           conn))
                                {
                                    orderCommand.Parameters.AddWithValue("@Count", orders.Count);
                                    orderCommand.Parameters.AddWithValue("@Quantity", sqlItemQuantity1.Value);
                                    orderCommand.Parameters.AddWithValue("@UserId", user1.GetUserId());
                                    orderCommand.Prepare();
                                    var rows = orderCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException ex)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine($"Message: \n{ex.Message}");
                            }
                            orders.Add(order);
                            itemMaster = new ItemMaster(item, order, itemMasters.Count);
                            try
                            {
                                using (MySqlCommand itemMasterCommand = new MySqlCommand(
                                           "INSERT INTO ItemMaster VALUES (@Count, @Child, @Toy)", conn))
                                {
                                    itemMasterCommand.Parameters.AddWithValue("@Count", itemMasters.Count);
                                    itemMasterCommand.Parameters.AddWithValue("@Child", item.GetItemId());
                                    itemMasterCommand.Parameters.AddWithValue("@Toy", order.GetOrderId());
                                    itemMasterCommand.Prepare();
                                    var rows = itemMasterCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException exception)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine(exception.Message);
                            }
                            itemMasters.Add(itemMaster);
                            foreach (ItemMaster theVariable in itemMasters)
                            {
                                if (theVariable.GetMasterId() == item.GetItemId())
                                {
                                    long newItemId = item.GetItemId() + 1;
                                    try
                                    {
                                        using (MySqlCommand editItem = new MySqlCommand(
                                                   "UPDATE Item SET qID = @New WHERE qID = @Old",
                                                   conn))
                                        {
                                            editItem.Parameters.AddWithValue("@New", newItemId);
                                            editItem.Parameters.AddWithValue("@Old", item.GetItemId());
                                            editItem.Prepare();
                                            var rows = editItem.ExecuteNonQuery();
                                            switch (rows)
                                            {
                                                case 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} row affected.");
                                                    break;
                                                case > 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} rows affected.");
                                                    break;
                                                default:
                                                    Console.WriteLine("No changes happened to the database.");
                                                    break;
                                            }
                                        }
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine("Failed to execute an operation in database!");
                                        Console.WriteLine("There is a problem with database manipulation!");
                                        Console.WriteLine(ex.Message);
                                    }
                                    try
                                    {
                                        using (MySqlCommand editItemMaster =
                                               new MySqlCommand(
                                                   "UPDATE ItemMaster SET childID = @New WHERE masterID = @Old AND childID = @Child",
                                                   conn))
                                        {
                                            editItemMaster.Parameters.AddWithValue("@New", newItemId);
                                            editItemMaster.Parameters.AddWithValue("@Old", itemMaster.GetMasterId());
                                            editItemMaster.Parameters.AddWithValue("@Child", item.GetItemId());
                                            editItemMaster.Prepare();
                                            var rows = editItemMaster.ExecuteNonQuery();
                                            switch (rows)
                                            {
                                                case 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} row affected.");
                                                    break;
                                                case > 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} rows affected.");
                                                    break;
                                                default:
                                                    Console.WriteLine("No changes happened to the database.");
                                                    break;
                                            }
                                        }
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine("Failed to execute an operation in database!");
                                        Console.WriteLine("There is a problem with database manipulation!");
                                        Console.WriteLine(ex.Message);
                                    }
                                    item.SetItemId(newItemId);
                                    theVariable.SetItemId(item); //Refer to Item Master class
                                }
                            }
                            try
                            {
                                DateTime date = DateTime.Now;
                                using (MySqlCommand lineCommand = new MySqlCommand(
                                           "INSERT INTO OrderLine VALUES (@UserId, @OrderId, @Date)", conn))
                                {
                                    lineCommand.Parameters.AddWithValue("@UserId", user1.GetUserId());
                                    lineCommand.Parameters.AddWithValue("@OrderId", order.GetOrderId());
                                    lineCommand.Parameters.AddWithValue("@Date", date.Date);
                                    lineCommand.Prepare();
                                    var rows = lineCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException exception)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine(exception.Message);
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
                                Console.WriteLine(
                                    "You have not been added to the queue because either you ordered previously or you did not type \"yes\" exactly as it is.");
                            }
                            Console.WriteLine(
                                $"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                            Console.WriteLine("yes or no?");
                            Console.Write("Answer: ");
                            string? orderAnswer = Console.ReadLine();
                            while (orderAnswer!.Equals("yes"))
                            {
                                Console.Write("Please enter an item name: ");
                                string itemName2 = Console.ReadLine()!;
                                SqlString sqlItemName2 = new SqlString(itemName2);
                                Console.Write("Please enter an item category: ");
                                string itemCategory2 = Console.ReadLine()!;
                                SqlString sqlItemCategory2 = new SqlString(itemCategory2);
                                item = new Item(name: itemName2, category: itemCategory2, itemId: itemMasters.Count+1);
                                try
                                {
                                    using (MySqlCommand itemCommand = new MySqlCommand(
                                               "INSERT INTO Item VALUES (@Count, @Name, @Category)", conn))
                                    {
                                        itemCommand.Parameters.AddWithValue("@Count", itemMasters.Count+1);
                                        itemCommand.Parameters.AddWithValue("@Name", sqlItemName2.Value);
                                        itemCommand.Parameters.AddWithValue("@Category", sqlItemCategory2.Value);
                                        itemCommand.Prepare();
                                        var rows = itemCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                }
                                itemMaster = new ItemMaster(item, order, itemMasters.Count+1);
                                itemMasters.Add(itemMaster);
                                try
                                {
                                    using (MySqlCommand itemMasterCommand =
                                           new MySqlCommand(
                                               "INSERT INTO ItemMaster VALUES (@MasterId, @ItemId, @OrderId)",
                                               conn))
                                    {
                                        itemMasterCommand.Parameters.AddWithValue("@ItemId", item.GetItemId());
                                        itemMasterCommand.Parameters.AddWithValue("@OrderId", order.GetOrderId());
                                        itemMasterCommand.Parameters.AddWithValue("@MasterId",
                                            itemMaster.GetMasterId());
                                        itemMasterCommand.Prepare();
                                        var rows = itemMasterCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine(exception.Message);
                                }
                                Console.WriteLine(
                                    $"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
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
                                Console.WriteLine("Information from the database: ");
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Item Master details: ");
                                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ItemMaster", conn))
                                    {
                                        command.Prepare();
                                        using (MySqlDataReader reader = command.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Master ID\t\tChild ID\t\tTo Order");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["masterID"]}\t\t\t{reader["childID"]}\t\t\t{reader["toOrder"]}");
                                            }
                                            Console.WriteLine();
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                }
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Orders details: ");
                                    using (MySqlCommand sqlCommand =
                                           new MySqlCommand("SELECT * FROM RestaurantOrder", conn))
                                    {
                                        sqlCommand.Prepare();
                                        using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Order ID\t\tQuantity\t\tTo User");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["orderID"]}\t\t\t{reader["Quantity"]}\t\t\t{reader["userID"]}");
                                            }
                                            Console.WriteLine();
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                }
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Items details: ");
                                    using (MySqlCommand theCommand =
                                           new MySqlCommand("SELECT * FROM Item", conn))
                                    {
                                        theCommand.Prepare();
                                        using (MySqlDataReader reader = theCommand.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Item ID\t\tName\t\tCategory");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["qID"]}\t\t{reader["Name"]}\t\t{reader["Category"]}");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
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
                                        try
                                        {
                                            using (MySqlCommand command = new MySqlCommand(
                                                       "UPDATE ItemMaster SET toOrder = null WHERE masterID = @MasterID",
                                                       conn))
                                            {
                                                command.Parameters.AddWithValue("@MasterID", itemMaster.GetMasterId());
                                                command.Prepare();
                                                var rows = command.ExecuteNonQuery();
                                                switch (rows)
                                                {
                                                    case 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} row affected.");
                                                        break;
                                                    case > 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} rows affected.");
                                                        break;
                                                    default:
                                                        Console.WriteLine("No changes happened to the database.");
                                                        break;
                                                }
                                            }
                                        }
                                        catch (MySqlException exception)
                                        {
                                            Console.WriteLine("Failed to execute an operation in database!");
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            Console.WriteLine($"Error message: \n{exception.Message}");
                                        }
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
                                try
                                {
                                    using (MySqlCommand sqlCommand = new MySqlCommand(
                                               "DELETE FROM RestaurantOrder WHERE orderID = @OrderID", conn))
                                    {
                                        sqlCommand.Parameters.AddWithValue("@OrderID", order.GetOrderId());
                                        sqlCommand.Prepare();
                                        var rows = sqlCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine($"Error message: \n{exception.Message}");
                                }
                                bool removed = itemMasters.Remove(itemMaster);
                                try
                                {
                                    using (MySqlCommand sqlCommand = new MySqlCommand(
                                               "DELETE FROM ItemMaster WHERE masterID = @MasterID", conn))
                                    {
                                        sqlCommand.Parameters.AddWithValue("@MasterID", itemMaster.GetMasterId());
                                        sqlCommand.Prepare();
                                        var rows = sqlCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException e)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine($"Error message: \n{e.Message}");
                                }
                                try
                                {
                                    using (MySqlCommand command =
                                           new MySqlCommand("DELETE FROM Item WHERE qID = @ItemId", conn))
                                    {
                                        command.Parameters.AddWithValue("@ItemId", itemMaster.GetMasterId());
                                        command.Prepare();
                                        int rows = command.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine(
                                                    $"Successfully deleted item and {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine(
                                                    $"Successfully deleted item and {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("Nothing happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in the database.");
                                    Console.WriteLine($"Message: {exception.Message}");
                                }
                                if (success && isRemoved && !operation.Equals(null) && removed)
                                {
                                    Console.WriteLine("The order have been successfully deleted.");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to delete the order!");
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
                                        try
                                        {
                                            using (MySqlCommand theCommand =
                                                   new MySqlCommand("DELETE FROM Item WHERE qID = @ItemID", conn))
                                            {
                                                theCommand.Parameters.AddWithValue("@ItemID",
                                                    penders[j].GetItem().GetItemId());
                                                theCommand.Prepare();
                                                var rows = theCommand.ExecuteNonQuery();
                                                switch (rows)
                                                {
                                                    case 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} row affected.");
                                                        break;
                                                    case > 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} rows affected.");
                                                        break;
                                                    default:
                                                        Console.WriteLine("No changes happened to the database.");
                                                        break;
                                                }
                                            }
                                        }
                                        catch (MySqlException exception)
                                        {
                                            Console.WriteLine("Failed to execute an operation in database!");
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            Console.WriteLine($"Error message: \n{exception.Message}");
                                        }
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
                                    Console.WriteLine(
                                        $"The order is found at location {myItem.GetMasterId()} in item master list.");
                                    theDictionary[1] = true;
                                    break;
                                }
                            }
                            int theSearcher = user1.SearchOrder(order);
                            if (theSearcher > -1)
                            {
                                Console.WriteLine(
                                    $"The order is found at location {user1.SearchOrder(order)} in your order list.");
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
                                    Console.WriteLine("Information found from the database: ");
                                    try
                                    {
                                        using (MySqlCommand printCommand = new MySqlCommand(
                                                   "SELECT M.masterID, I.qID, I.Name, I.Category, O.orderID, O.Quantity FROM Item AS I, ItemMaster AS M, RestaurantOrder AS O WHERE I.qID = @ItemID AND M.masterID = @MasterID AND O.orderID = @OrderID",
                                                   conn))
                                        {
                                            printCommand.Parameters.AddWithValue("@MasterID", t.GetMasterId());
                                            printCommand.Parameters.AddWithValue("@ItemID", t.GetItem().GetItemId());
                                            printCommand.Parameters.AddWithValue("@OrderID", t.GetOrder().GetOrderId());
                                            printCommand.Prepare();
                                            using (MySqlDataReader reader = printCommand.ExecuteReader())
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("Master ID\t\tChild ID\t\tName\t\tCategory\t\tOrder ID\t\tQuantity");
                                                while (reader.Read())
                                                {
                                                    Console.WriteLine($"{reader["masterID"]}\t\t\t{reader["qID"]}\t\t\t{reader["Name"]}\t\t{reader["Category"]}\t\t\t{reader["orderID"]}\t\t\t{reader["Quantity"]}");
                                                }
                                                Console.WriteLine();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Failed to execute an operation from the database!");
                                    }
                                    Console.WriteLine("Another information retrieved from the database: ");
                                    try
                                    {
                                        using (MySqlCommand command = new MySqlCommand(
                                                   "SELECT L.userID, U.customerID, L.toOrder, O.Quantity, L.historyDate FROM OrderLine AS L, RestaurantUser AS U, RestaurantOrder AS O WHERE L.toOrder = @Order AND L.userID = U.pID AND L.toOrder = O.orderID",
                                                   conn))
                                        {
                                            command.Parameters.AddWithValue("@Order", t.GetOrder().GetOrderId());
                                            command.Prepare();
                                            using (MySqlDataReader reader = command.ExecuteReader())
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("User ID\t\tFor Customer ID\t\tTo Order ID\t\tQuantity\t\tLast Created On");
                                                while (reader.Read())
                                                {
                                                    Console.WriteLine($"{reader["userID"]}\t\t\t{reader["customerID"]}\t\t{reader["toOrder"]}\t\t\t{reader["Quantity"]}\t\t\t{reader["historyDate"]}");
                                                }
                                                Console.WriteLine();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Failed to execute an operation from the database!");
                                    }
                                    break;
                                }
                            }
                            break;
                        case 6:
                            user1.ClearOrder();
                            try
                            {
                                using (MySqlCommand dangerousCommand =
                                       new MySqlCommand("DELETE FROM RestaurantOrder", conn))
                                {
                                    dangerousCommand.Prepare();
                                    var rows = dangerousCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            break;
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            break;
                        case 7:
                            orderLine.ClearOrderLine();
                            try
                            {
                                using (MySqlCommand clearingCommand =
                                       new MySqlCommand("DELETE FROM OrderLine", conn))
                                {
                                    clearingCommand.Prepare();
                                    var rows = clearingCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            break;
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            try
                            {
                                using (MySqlCommand itemDeleterCommand = new MySqlCommand("DELETE FROM Item", conn))
                                {
                                    itemDeleterCommand.Prepare();
                                    int rows = itemDeleterCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1: 
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1: 
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("Nothing happened in the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException e){
                                Console.WriteLine("Failed to execute an operation in the database!");
                                Console.WriteLine($"Message: \n{e.Message}");
                            }
                            break;
                        case 8:
                            Console.WriteLine("Details found: ");
                            user1.Display();
                            Console.WriteLine();
                            Console.WriteLine("Information found from the database: ");
                            try
                            {
                                using (MySqlCommand printerCommand =
                                       new MySqlCommand("SELECT * FROM Customer, RestaurantUser", conn))
                                {
                                    printerCommand.Prepare();
                                    using (MySqlDataReader reader = printerCommand.ExecuteReader())
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Name\t\tID\t\tDate of birth\t\tEmail\t\t\t\tPhone\t\tPassword");
                                        while (reader.Read())
                                        {
                                            if (Convert.ToInt64(reader["ID"]) == user1.GetCustomer().GetId() ||
                                                Convert.ToInt64(reader["pID"]) == user1.GetUserId())
                                            {
                                                Console.WriteLine($"{reader["Name"]}\t{reader["ID"]}\t{reader["DOB"]}\t{reader["Email"]}\t{reader["Phone"]}\t{reader["Password"]}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Hidden\t\tHidden\t\tThe Hidden\t\tHidden\t\t\t\tHidden\t\tHidden");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            break;
                        case 9:
                            try 
                            {
                                using (MySqlCommand makingSure = new MySqlCommand("DELETE FROM OrderLine WHERE toOrder = null OR userID = null", conn))
                                {
                                    makingSure.Prepare();
                                    int rows = makingSure.ExecuteNonQuery();
                                    if (rows == 1)
                                    {
                                        Console.WriteLine($"Successfully executed operation and {rows} row affected.");
                                    }
                                    else if (rows > 1)
                                    {
                                        Console.WriteLine($"Successfully executed operation and {rows} rows affected.");
                                    }
                                    else 
                                    {
                                        Console.WriteLine("No changes happened to the database.");
                                    }
                                }
                            }
                            catch 
                            {
                                Console.WriteLine("Failed to execute database command!");
                                Console.WriteLine("There is a problem with the database!");
                            }
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
                                try
                                {
                                    using (MySqlCommand deleteCommand = new MySqlCommand(
                                               "DELETE FROM RestaurantOrder WHERE orderID = @Order", conn))
                                    {
                                        deleteCommand.Parameters.AddWithValue("@Order", order.GetOrderId());
                                        deleteCommand.Prepare();
                                        int rows = deleteCommand.ExecuteNonQuery();
                                        if (rows == 1)
                                        {
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                        }
                                        else if (rows > 1)
                                        {
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("No changes happened to the database.");
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation from the database!");
                                }
                                orders.Remove(order);
                            }
                            if (!dictionary[1])
                            {
                                servedUser = orderLine.PollUser();
                            }
                            try
                            {
                                using (MySqlCommand pollingCommand = new MySqlCommand(
                                           "DELETE FROM OrderLine WHERE toOrder = @Order", conn))
                                {
                                    pollingCommand.Parameters.AddWithValue("@Order", orderLine.GetOrder().GetOrderId());
                                    pollingCommand.Prepare();
                                    int rows = pollingCommand.ExecuteNonQuery();
                                    if (rows == 1)
                                    {
                                        Console.WriteLine($"Operation successful with {rows} row affected.");
                                    }
                                    else if (rows > 1)
                                    {
                                        Console.WriteLine($"Operation successful with {rows} rows affected.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No changes happened to the database.");
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            Order servedOrder = orderLine.PollOrder();
                            Console.WriteLine(
                                $"The order {servedOrder.GetOrderId()} has been served to {servedUser.GetCustomer().GetName()}.");
                            break;
                        case 10:
                            orderLine.GetOrder().Display(); //This function works when first order in the Queue exists
                            try
                            {
                                Console.WriteLine("Order Line details retrieved from the database: \n");
                                using (MySqlCommand tryCommand = new MySqlCommand("SELECT * FROM OrderLine", conn))
                                {
                                    tryCommand.Prepare();
                                    using (MySqlDataReader tryReader = tryCommand.ExecuteReader())
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Order ID\t\tTo User\t\tCreation Date");
                                        while (tryReader.Read())
                                        {
                                            if (Convert.ToInt64(tryReader["toOrder"]) <= 1 ||
                                                Convert.ToInt64(tryReader["userID"]) <= 1)
                                            {
                                                Console.WriteLine(
                                                    $"{tryReader["toOrder"]}\t\t\t{tryReader["userID"]}\t\t\t{tryReader["historyDate"]}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Hidden\t\tHidden\t\tHidden");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                            }
                            catch (MySqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case 11:
                            orderLine.CheckLastOrder().Display(); //This function works when the order gets processed
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
            else if (authenticate[0] || (authenticate[1] && authenticate[2]))
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
                            string itemName1 = Console.ReadLine()!;
                            SqlString sqlItemName1 = new SqlString(itemName1);
                            Console.Write("Please enter an item category: ");
                            string itemCategory1 = Console.ReadLine()!;
                            SqlString sqlItemCategory1 = new SqlString(itemCategory1);
                            item = new Item(name: itemName1, category: itemCategory1, itemId: itemMasters.Count);
                            try
                            {
                                using (MySqlCommand itemCommand = new MySqlCommand(
                                           "INSERT INTO Item VALUES (@Count, @Name, @Category)", conn))
                                {
                                    itemCommand.Parameters.AddWithValue("@Count", itemMasters.Count);
                                    itemCommand.Parameters.AddWithValue("@Name", sqlItemName1.Value);
                                    itemCommand.Parameters.AddWithValue("@Category", sqlItemCategory1.Value);
                                    itemCommand.Prepare();
                                    var rows = itemCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException e)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine($"Message: \n{e.Message}");
                            }
                            Console.Write("Please enter an item quantity: ");
                            int itemQuantity1 = Convert.ToInt32(Console.ReadLine());
                            SqlSingle sqlItemQuantity1 = new SqlSingle(itemQuantity1);
                            order = new Order(customer1.GetName(), customer1.GetEmail(), customer1.GetPhone(),
                                customer1.GetId(), customer1.GetDob().Year, customer1.GetDob().Month,
                                customer1.GetDob().Day,
                                user1.GetUserId(), user1.GetPassword(), orders.Count, itemQuantity1);
                            try
                            {
                                using (MySqlCommand orderCommand = new MySqlCommand(
                                           "INSERT INTO RestaurantOrder VALUES (@Count, @Quantity, @UserId)",
                                           conn))
                                {
                                    orderCommand.Parameters.AddWithValue("@Count", orders.Count);
                                    orderCommand.Parameters.AddWithValue("@Quantity", sqlItemQuantity1.Value);
                                    orderCommand.Parameters.AddWithValue("@UserId", user1.GetUserId());
                                    orderCommand.Prepare();
                                    var rows = orderCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException ex)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine($"Message: \n{ex.Message}");
                            }
                            orders.Add(order);
                            itemMaster = new ItemMaster(item, order, itemMasters.Count);
                            try
                            {
                                using (MySqlCommand itemMasterCommand = new MySqlCommand(
                                           "INSERT INTO ItemMaster VALUES (@Count, @Child, @Toy)", conn))
                                {
                                    itemMasterCommand.Parameters.AddWithValue("@Count", itemMasters.Count);
                                    itemMasterCommand.Parameters.AddWithValue("@Child", item.GetItemId());
                                    itemMasterCommand.Parameters.AddWithValue("@Toy", order.GetOrderId());
                                    itemMasterCommand.Prepare();
                                    var rows = itemMasterCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException exception)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine(exception.Message);
                            }
                            itemMasters.Add(itemMaster);
                            foreach (ItemMaster theVariable in itemMasters)
                            {
                                if (theVariable.GetMasterId() == item.GetItemId())
                                {
                                    long newItemId = item.GetItemId() + 1;
                                    try
                                    {
                                        using (MySqlCommand editItem = new MySqlCommand(
                                                   "UPDATE Item SET qID = @New WHERE qID = @Old",
                                                   conn))
                                        {
                                            editItem.Parameters.AddWithValue("@New", newItemId);
                                            editItem.Parameters.AddWithValue("@Old", item.GetItemId());
                                            editItem.Prepare();
                                            var rows = editItem.ExecuteNonQuery();
                                            switch (rows)
                                            {
                                                case 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} row affected.");
                                                    break;
                                                case > 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} rows affected.");
                                                    break;
                                                default:
                                                    Console.WriteLine("No changes happened to the database.");
                                                    break;
                                            }
                                        }
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine("Failed to execute an operation in database!");
                                        Console.WriteLine("There is a problem with database manipulation!");
                                        Console.WriteLine(ex.Message);
                                    }
                                    try
                                    {
                                        using (MySqlCommand editItemMaster =
                                               new MySqlCommand(
                                                   "UPDATE ItemMaster SET childID = @New WHERE masterID = @Old AND childID = @Child",
                                                   conn))
                                        {
                                            editItemMaster.Parameters.AddWithValue("@New", newItemId);
                                            editItemMaster.Parameters.AddWithValue("@Old", itemMaster.GetMasterId());
                                            editItemMaster.Parameters.AddWithValue("@Child", item.GetItemId());
                                            editItemMaster.Prepare();
                                            var rows = editItemMaster.ExecuteNonQuery();
                                            switch (rows)
                                            {
                                                case 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} row affected.");
                                                    break;
                                                case > 1:
                                                    Console.WriteLine(
                                                        $"Operation successful with {rows} rows affected.");
                                                    break;
                                                default:
                                                    Console.WriteLine("No changes happened to the database.");
                                                    break;
                                            }
                                        }
                                    }
                                    catch (MySqlException ex)
                                    {
                                        Console.WriteLine("Failed to execute an operation in database!");
                                        Console.WriteLine("There is a problem with database manipulation!");
                                        Console.WriteLine(ex.Message);
                                    }
                                    item.SetItemId(newItemId);
                                    theVariable.SetItemId(item); //Refer to Item Master class
                                }
                            }
                            try
                            {
                                DateTime date = DateTime.Now;
                                using (MySqlCommand lineCommand = new MySqlCommand(
                                           "INSERT INTO OrderLine VALUES (@UserId, @OrderId, @Date)", conn))
                                {
                                    lineCommand.Parameters.AddWithValue("@UserId", user1.GetUserId());
                                    lineCommand.Parameters.AddWithValue("@OrderId", order.GetOrderId());
                                    lineCommand.Parameters.AddWithValue("@Date", date.Date);
                                    lineCommand.Prepare();
                                    var rows = lineCommand.ExecuteNonQuery();
                                    switch (rows)
                                    {
                                        case 1:
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                            break;
                                        case > 1:
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                            break;
                                        default:
                                            Console.WriteLine("No changes happened to the database.");
                                            break;
                                    }
                                }
                            }
                            catch (MySqlException exception)
                            {
                                Console.WriteLine("Failed to execute an operation in database!");
                                Console.WriteLine("There is a problem with database manipulation!");
                                Console.WriteLine(exception.Message);
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
                                Console.WriteLine(
                                    "You have not been added to the queue because either you ordered previously or you did not type \"yes\" exactly as it is.");
                            }
                            Console.WriteLine(
                                $"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
                            Console.WriteLine("yes or no?");
                            Console.Write("Answer: ");
                            string? orderAnswer = Console.ReadLine();
                            while (orderAnswer!.Equals("yes"))
                            {
                                Console.Write("Please enter an item name: ");
                                string itemName2 = Console.ReadLine()!;
                                SqlString sqlItemName2 = new SqlString(itemName2);
                                Console.Write("Please enter an item category: ");
                                string itemCategory2 = Console.ReadLine()!;
                                SqlString sqlItemCategory2 = new SqlString(itemCategory2);
                                item = new Item(name: itemName2, category: itemCategory2, itemId: itemMasters.Count+1);
                                try
                                {
                                    using (MySqlCommand itemCommand = new MySqlCommand(
                                               "INSERT INTO Item VALUES (@Count, @Name, @Category)", conn))
                                    {
                                        itemCommand.Parameters.AddWithValue("@Count", itemMasters.Count+1);
                                        itemCommand.Parameters.AddWithValue("@Name", sqlItemName2.Value);
                                        itemCommand.Parameters.AddWithValue("@Category", sqlItemCategory2.Value);
                                        itemCommand.Prepare();
                                        var rows = itemCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                }
                                itemMaster = new ItemMaster(item, order, itemMasters.Count+1);
                                itemMasters.Add(itemMaster);
                                try
                                {
                                    using (MySqlCommand itemMasterCommand =
                                           new MySqlCommand(
                                               "INSERT INTO ItemMaster VALUES (@MasterId, @ItemId, @OrderId)",
                                               conn))
                                    {
                                        itemMasterCommand.Parameters.AddWithValue("@ItemId", item.GetItemId());
                                        itemMasterCommand.Parameters.AddWithValue("@OrderId", order.GetOrderId());
                                        itemMasterCommand.Parameters.AddWithValue("@MasterId",
                                            itemMaster.GetMasterId());
                                        itemMasterCommand.Prepare();
                                        var rows = itemMasterCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine(exception.Message);
                                }
                                Console.WriteLine(
                                    $"Would you like to add more items to the same order as quantity of {order.GetQuantity()}?");
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
                                Console.WriteLine("Information from the database: ");
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Item Master details: ");
                                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM ItemMaster", conn))
                                    {
                                        command.Prepare();
                                        using (MySqlDataReader reader = command.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Master ID\t\tChild ID\t\tTo Order");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["masterID"]}\t\t\t{reader["childID"]}\t\t\t{reader["toOrder"]}");
                                            }
                                            Console.WriteLine();
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                }
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Orders details: ");
                                    using (MySqlCommand sqlCommand =
                                           new MySqlCommand("SELECT * FROM RestaurantOrder", conn))
                                    {
                                        sqlCommand.Prepare();
                                        using (MySqlDataReader reader = sqlCommand.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Order ID\t\tQuantity\t\tTo User");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["orderID"]}\t\t\t{reader["Quantity"]}\t\t\t{reader["userID"]}");
                                            }
                                            Console.WriteLine();
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                }
                                try
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Items details: ");
                                    using (MySqlCommand theCommand =
                                           new MySqlCommand("SELECT * FROM Item", conn))
                                    {
                                        theCommand.Prepare();
                                        using (MySqlDataReader reader = theCommand.ExecuteReader())
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("Item ID\t\tName\t\tCategory");
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader["qID"]}\t\t{reader["Name"]}\t\t{reader["Category"]}");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
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
                                        try
                                        {
                                            using (MySqlCommand command = new MySqlCommand(
                                                       "UPDATE ItemMaster SET toOrder = null WHERE masterID = @MasterID",
                                                       conn))
                                            {
                                                command.Parameters.AddWithValue("@MasterID", itemMaster.GetMasterId());
                                                command.Prepare();
                                                var rows = command.ExecuteNonQuery();
                                                switch (rows)
                                                {
                                                    case 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} row affected.");
                                                        break;
                                                    case > 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} rows affected.");
                                                        break;
                                                    default:
                                                        Console.WriteLine("No changes happened to the database.");
                                                        break;
                                                }
                                            }
                                        }
                                        catch (MySqlException exception)
                                        {
                                            Console.WriteLine("Failed to execute an operation in database!");
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            Console.WriteLine($"Error message: \n{exception.Message}");
                                        }
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
                                try
                                {
                                    using (MySqlCommand sqlCommand = new MySqlCommand(
                                               "DELETE FROM RestaurantOrder WHERE orderID = @OrderID", conn))
                                    {
                                        sqlCommand.Parameters.AddWithValue("@OrderID", order.GetOrderId());
                                        sqlCommand.Prepare();
                                        var rows = sqlCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine($"Error message: \n{exception.Message}");
                                }
                                bool removed = itemMasters.Remove(itemMaster);
                                try
                                {
                                    using (MySqlCommand sqlCommand = new MySqlCommand(
                                               "DELETE FROM ItemMaster WHERE masterID = @MasterID", conn))
                                    {
                                        sqlCommand.Parameters.AddWithValue("@MasterID", itemMaster.GetMasterId());
                                        sqlCommand.Prepare();
                                        var rows = sqlCommand.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine($"Operation successful with {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine($"Operation successful with {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("No changes happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException e)
                                {
                                    Console.WriteLine("Failed to execute an operation in database!");
                                    Console.WriteLine("There is a problem with database manipulation!");
                                    Console.WriteLine($"Error message: \n{e.Message}");
                                }
                                try
                                {
                                    using (MySqlCommand command =
                                           new MySqlCommand("DELETE FROM Item WHERE qID = @ItemId", conn))
                                    {
                                        command.Parameters.AddWithValue("@ItemId", itemMaster.GetMasterId());
                                        command.Prepare();
                                        int rows = command.ExecuteNonQuery();
                                        switch (rows)
                                        {
                                            case 1:
                                                Console.WriteLine(
                                                    $"Successfully deleted item and {rows} row affected.");
                                                break;
                                            case > 1:
                                                Console.WriteLine(
                                                    $"Successfully deleted item and {rows} rows affected.");
                                                break;
                                            default:
                                                Console.WriteLine("Nothing happened to the database.");
                                                break;
                                        }
                                    }
                                }
                                catch (MySqlException exception)
                                {
                                    Console.WriteLine("Failed to execute an operation in the database.");
                                    Console.WriteLine($"Message: {exception.Message}");
                                }
                                if (success && isRemoved && !operation.Equals(null) && removed)
                                {
                                    Console.WriteLine("The order have been successfully deleted.");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to delete the order!");
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
                                        try
                                        {
                                            using (MySqlCommand theCommand =
                                                   new MySqlCommand("DELETE FROM Item WHERE qID = @ItemID", conn))
                                            {
                                                theCommand.Parameters.AddWithValue("@ItemID",
                                                    penders[j].GetItem().GetItemId());
                                                theCommand.Prepare();
                                                var rows = theCommand.ExecuteNonQuery();
                                                switch (rows)
                                                {
                                                    case 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} row affected.");
                                                        break;
                                                    case > 1:
                                                        Console.WriteLine(
                                                            $"Operation successful with {rows} rows affected.");
                                                        break;
                                                    default:
                                                        Console.WriteLine("No changes happened to the database.");
                                                        break;
                                                }
                                            }
                                        }
                                        catch (MySqlException exception)
                                        {
                                            Console.WriteLine("Failed to execute an operation in database!");
                                            Console.WriteLine("There is a problem with database manipulation!");
                                            Console.WriteLine($"Error message: \n{exception.Message}");
                                        }
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
                                    Console.WriteLine(
                                        $"The order is found at location {myItem.GetMasterId()} in item master list.");
                                    theDictionary[1] = true;
                                    break;
                                }
                            }
                            int theSearcher = user1.SearchOrder(order);
                            if (theSearcher > -1)
                            {
                                Console.WriteLine(
                                    $"The order is found at location {user1.SearchOrder(order)} in your order list.");
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
                                    Console.WriteLine("Information found from the database: ");
                                    try
                                    {
                                        using (MySqlCommand printCommand = new MySqlCommand(
                                                   "SELECT M.masterID, I.qID, I.Name, I.Category, O.orderID, O.Quantity FROM Item AS I, ItemMaster AS M, RestaurantOrder AS O WHERE I.qID = @ItemID AND M.masterID = @MasterID AND O.orderID = @OrderID",
                                                   conn))
                                        {
                                            printCommand.Parameters.AddWithValue("@MasterID", t.GetMasterId());
                                            printCommand.Parameters.AddWithValue("@ItemID", t.GetItem().GetItemId());
                                            printCommand.Parameters.AddWithValue("@OrderID", t.GetOrder().GetOrderId());
                                            printCommand.Prepare();
                                            using (MySqlDataReader reader = printCommand.ExecuteReader())
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("Master ID\t\tChild ID\t\tName\t\tCategory\t\tOrder ID\t\tQuantity");
                                                while (reader.Read())
                                                {
                                                    Console.WriteLine($"{reader["masterID"]}\t\t\t{reader["qID"]}\t\t\t{reader["Name"]}\t\t{reader["Category"]}\t\t\t{reader["orderID"]}\t\t\t{reader["Quantity"]}");
                                                }
                                                Console.WriteLine();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Failed to execute an operation from the database!");
                                    }
                                    Console.WriteLine("Another information retrieved from the database: ");
                                    try
                                    {
                                        using (MySqlCommand command = new MySqlCommand(
                                                   "SELECT L.userID, U.customerID, L.toOrder, O.Quantity, L.historyDate FROM OrderLine AS L, RestaurantUser AS U, RestaurantOrder AS O WHERE L.toOrder = @Order AND L.userID = U.pID AND L.toOrder = O.orderID",
                                                   conn))
                                        {
                                            command.Parameters.AddWithValue("@Order", t.GetOrder().GetOrderId());
                                            command.Prepare();
                                            using (MySqlDataReader reader = command.ExecuteReader())
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine("User ID\t\tFor Customer ID\t\tTo Order ID\t\tQuantity\t\tLast Created On");
                                                while (reader.Read())
                                                {
                                                    Console.WriteLine($"{reader["userID"]}\t\t\t{reader["customerID"]}\t\t{reader["toOrder"]}\t\t\t{reader["Quantity"]}\t\t\t{reader["historyDate"]}");
                                                }
                                                Console.WriteLine();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Failed to execute an operation from the database!");
                                    }
                                    break;
                                }
                            }
                            break;
                        case 6:
                            Console.WriteLine("Details found: ");
                            user1.Display();
                            Console.WriteLine();
                            Console.WriteLine("Information found from the database: ");
                            try
                            {
                                using (MySqlCommand printerCommand =
                                       new MySqlCommand("SELECT * FROM Customer, RestaurantUser", conn))
                                {
                                    printerCommand.Prepare();
                                    using (MySqlDataReader reader = printerCommand.ExecuteReader())
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Name\t\tID\t\tDate of birth\t\tEmail\t\t\t\tPhone\t\tPassword");
                                        while (reader.Read())
                                        {
                                            if (Convert.ToInt64(reader["ID"]) == user1.GetCustomer().GetId() ||
                                                Convert.ToInt64(reader["pID"]) == user1.GetUserId())
                                            {
                                                Console.WriteLine($"{reader["Name"]}\t{reader["ID"]}\t{reader["DOB"]}\t{reader["Email"]}\t{reader["Phone"]}\t{reader["Password"]}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Hidden\t\tHidden\t\tThe Hidden\t\tHidden\t\t\t\tHidden\t\tHidden");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            break;
                        case 7:
                            try 
                            {
                                using (MySqlCommand makingSure = new MySqlCommand("DELETE FROM OrderLine WHERE toOrder = null OR userID = null", conn))
                                {
                                    makingSure.Prepare();
                                    int rows = makingSure.ExecuteNonQuery();
                                    if (rows == 1)
                                    {
                                        Console.WriteLine($"Successfully executed operation and {rows} row affected.");
                                    }
                                    else if (rows > 1)
                                    {
                                        Console.WriteLine($"Successfully executed operation and {rows} rows affected.");
                                    }
                                    else 
                                    {
                                        Console.WriteLine("No changes happened to the database.");
                                    }
                                }
                            }
                            catch 
                            {
                                Console.WriteLine("Failed to execute database command!");
                                Console.WriteLine("There is a problem with the database!");
                            }
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
                                try
                                {
                                    using (MySqlCommand deleteCommand = new MySqlCommand(
                                               "DELETE FROM RestaurantOrder WHERE orderID = @Order", conn))
                                    {
                                        deleteCommand.Parameters.AddWithValue("@Order", order.GetOrderId());
                                        deleteCommand.Prepare();
                                        int rows = deleteCommand.ExecuteNonQuery();
                                        if (rows == 1)
                                        {
                                            Console.WriteLine($"Operation successful with {rows} row affected.");
                                        }
                                        else if (rows > 1)
                                        {
                                            Console.WriteLine($"Operation successful with {rows} rows affected.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("No changes happened to the database.");
                                        }
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Failed to execute an operation from the database!");
                                }
                                orders.Remove(order);
                            }
                            if (!dictionary[1])
                            {
                                servedUser = orderLine.PollUser();
                            }
                            try
                            {
                                using (MySqlCommand pollingCommand = new MySqlCommand(
                                           "DELETE FROM OrderLine WHERE toOrder = @Order", conn))
                                {
                                    pollingCommand.Parameters.AddWithValue("@Order", orderLine.GetOrder().GetOrderId());
                                    pollingCommand.Prepare();
                                    int rows = pollingCommand.ExecuteNonQuery();
                                    if (rows == 1)
                                    {
                                        Console.WriteLine($"Operation successful with {rows} row affected.");
                                    }
                                    else if (rows > 1)
                                    {
                                        Console.WriteLine($"Operation successful with {rows} rows affected.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No changes happened to the database.");
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed to execute an operation from the database!");
                            }
                            Order servedOrder = orderLine.PollOrder();
                            Console.WriteLine(
                                $"The order {servedOrder.GetOrderId()} has been served to {servedUser.GetCustomer().GetName()}.");
                            break;
                        case 8:
                            orderLine.GetOrder().Display(); //This function works when first order in the Queue exists
                            try
                            {
                                Console.WriteLine("Order Line details retrieved from the database: \n");
                                using (MySqlCommand tryCommand = new MySqlCommand("SELECT * FROM OrderLine", conn))
                                {
                                    tryCommand.Prepare();
                                    using (MySqlDataReader tryReader = tryCommand.ExecuteReader())
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Order ID\t\tTo User\t\tCreation Date");
                                        while (tryReader.Read())
                                        {
                                            if (Convert.ToInt64(tryReader["toOrder"]) <= 1 ||
                                                Convert.ToInt64(tryReader["userID"]) <= 1)
                                            {
                                                Console.WriteLine(
                                                    $"{tryReader["toOrder"]}\t\t\t{tryReader["userID"]}\t\t\t{tryReader["historyDate"]}");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Hidden\t\tHidden\t\tHidden");
                                            }
                                        }
                                        Console.WriteLine();
                                    }
                                }
                            }
                            catch (MySqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                conn.Close();
                Process.GetCurrentProcess().Kill();
            }
            conn.Close();
        }
        catch (MySqlException theSqlException)
        {
            Console.WriteLine("Failed to open Database connection!");
            Console.WriteLine("There's a problem with sql as stated in the message below: ");
            Console.WriteLine(theSqlException.Message);
        }
        catch (DbException theDbException)
        {
            Console.WriteLine("There's a problem with the database as stated in the message below: ");
            Console.WriteLine(theDbException.Message);
        }
        catch (NullReferenceException theNullException)
        {
            Console.WriteLine("Empty value detected as stated in the message below: ");
            Console.WriteLine(theNullException.Message);
        }
        catch (IndexOutOfRangeException theIndexOutOfRangeException)
        {
            Console.WriteLine(
                "An item has been inserted into an array outside of its size range as stated in the message below: ");
            Console.WriteLine(theIndexOutOfRangeException.Message);
        }
        catch (InvalidCastException theInvalidCastException)
        {
            Console.WriteLine("Cannot convert the data from one type to another as stated in the message below: ");
            Console.WriteLine(theInvalidCastException.Message);
        }
        catch (InvalidDataException theInvalidDataException)
        {
            Console.WriteLine("This error might have something to do with mismatch data type.");
            Console.WriteLine("Check out the message below for accurate detail: ");
            Console.WriteLine(theInvalidDataException.Message);
        }
        catch (ApplicationException theApplicationException)
        {
            Console.WriteLine("There is a problem with this application as stated in the message below: ");
            Console.WriteLine(theApplicationException.Message);
        }
        catch (SystemException theSystemException)
        {
            Console.WriteLine("There is a problem with this system as stated in the message below: ");
            Console.WriteLine(theSystemException.Message);
        }
        finally
        {
            Console.WriteLine("You're about to exit this system.");
            Console.WriteLine("Thank you for using this system!");
            Environment.Exit(0);
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