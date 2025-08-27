using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantAddItemController(ILogger<RestaurantRegisterController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantRegisterController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    
    protected User MainUser { get; set; } =  new User();
    
    protected static ItemViewModel TheItemViewModel = new ItemViewModel();
    protected static ItemMasterViewModel TheItemMasterViewModel = new ItemMasterViewModel();
    protected static OrderViewModel TheOrderViewModel = new OrderViewModel();
    private int _counter;

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult<ItemMasterViewModel> Post([FromBody] ItemMasterViewModel item)
    {
        try
        {
            if (ModelState.IsValid)
            {
                MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
                SqlString itemName = new SqlString(item.NewItemMaster.GetItem().GetName());
                SqlString itemCategory = new SqlString(item.NewItemMaster.GetItem().GetCategory());
                SqlSingle itemQuantity = new SqlSingle(item.NewItemMaster.GetOrder().GetQuantity());
                Create(name: itemName, category: itemCategory, quantity: itemQuantity);
                RestaurantModel.NewItem = TheItemViewModel.NewItem;
                RestaurantModel.NewItemMaster = TheItemMasterViewModel.NewItemMaster;
                RestaurantModel.NewOrder = TheOrderViewModel.NewOrder;
                RestaurantModel.NewOrderLine = OrderLineViewModel.OrderLineDetail;
                return RedirectToPage("~/Home/RestaurantHome");
            }
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating an order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    protected ItemMasterViewModel Create(SqlString name, SqlSingle quantity, SqlString category)
    {
        ItemViewModel item = CreateItem(name, category);
        OrderViewModel order = CreateOrder(quantity, MainUser.GetUserId());
        bool orderLine = CreateOrderLineMain(MainUser.GetUserId(), order.NewOrder.GetOrderId());
        ItemMasterViewModel itemMaster = CreateItemMaster(item.NewItem.GetItemId(), order.NewOrder.GetOrderId(), name,
            category, quantity, MainUser.GetUserId());
        return orderLine ? itemMaster : new ItemMasterViewModel();
    }

    private ItemViewModel CreateItem(SqlString name, SqlString category)
    {
        ItemViewModel item = new ItemViewModel();
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("INSERT INTO Item VALUES (@Id, @Name, @Category)", _connection))
            {
                command.Parameters.AddWithValue("@Id", ItemViewModel.ItemList.getSize());
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Category", category);
                command.Prepare();
                command.ExecuteNonQuery();
                item.NewItem = new Item(name.Value, category.Value, ItemViewModel.ItemList.getSize());
                ItemViewModel.ItemList.add(item.NewItem);
                TheItemViewModel.NewItem = item.NewItem;
                TheItemViewModel = item;
            }
            _connection.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating an item in AQWE Restaurant");
        }
        return item;
    }

    private ItemMasterViewModel CreateItemMaster(SqlSingle childId, SqlSingle orderId, SqlString itemName, SqlString itemCategory, SqlSingle quantity, SqlSingle userId)
    {
        ItemMasterViewModel itemMaster = new ItemMasterViewModel();
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("INSERT INTO ItemMaster VALUES (@Master, @Child, @Order)", _connection))
            {
                command.Parameters.AddWithValue("@Master", ItemMasterViewModel.ItemMasterList.getSize());
                command.Parameters.AddWithValue("@Child", childId);
                command.Parameters.AddWithValue("@Order", orderId);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM Customer, RestaurantUser WHERE pID = @UserId", _connection))
            {
                command.Parameters.AddWithValue("@UserId", userId.Value);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            DateTime dob = DateTime.Parse(reader["DOB"].ToString()!);
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString email = new SqlString(reader["Email"].ToString());
                            SqlString password = new SqlString(reader["Password"].ToString());
                            SqlString phone = new SqlString(reader["Phone"].ToString());
                            SqlSingle id = SqlSingle.Parse(reader["ID"].ToString()!);
                            Item localItem = new Item(itemName.Value, itemCategory.Value,
                                ItemViewModel.ItemList.getSize());
                            Order localOrder = new Order(name.Value, email.Value, phone.Value, (long)id.Value,
                                dob.Year, dob.Month, dob.Day, (long)userId.Value, password.Value,
                                OrderViewModel.OrderList.getSize(), (int)quantity.Value);
                            itemMaster.NewItemMaster = new ItemMaster(localItem, localOrder,
                                ItemMasterViewModel.ItemMasterList.getSize());
                            ItemMasterViewModel.ItemMasterList.add(itemMaster.NewItemMaster);
                            TheItemMasterViewModel.NewItemMaster = itemMaster.NewItemMaster;
                            TheItemMasterViewModel = itemMaster;
                        }
                    }
                }
            }

            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error creating an item master in AQWE Restaurant");
        }
        return itemMaster;
    }

    private OrderViewModel CreateOrder(SqlSingle quantity, SqlSingle userId)
    {
        OrderViewModel order = new OrderViewModel();
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("INSERT INTO RestaurantOrder VALUES (@OrderId, @Quantity, @UserId)", _connection))
            {
                command.Parameters.AddWithValue("@OrderId", OrderViewModel.OrderList.getSize());
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Prepare();
                command.ExecuteNonQuery();
                order.NewOrder.SetOrderId(OrderViewModel.OrderList.getSize());
                order.NewOrder.SetQuantity((int)quantity.Value);
            }
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM Customer, RestaurantUser WHERE pID = @UserId", _connection))
            {
                command.Parameters.AddWithValue("@UserId", userId.Value);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            DateTime dob = DateTime.Parse(reader["DOB"].ToString()!);
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString  email = new SqlString(reader["Email"].ToString());
                            SqlString password = new SqlString(reader["Password"].ToString());
                            SqlString phone = new SqlString(reader["Phone"].ToString());
                            SqlSingle id = SqlSingle.Parse(reader["ID"].ToString()!);
                            User localUser = new User(name.Value, email.Value, phone.Value, (long)id.Value, dob.Year, dob.Month, dob.Day, (long)userId.Value, password.Value);
                            order.NewOrder.SetUser(localUser);
                        }
                    }
                }
            }
            OrderViewModel.OrderList.add(order.NewOrder);
            TheOrderViewModel.NewOrder = order.NewOrder;
            TheOrderViewModel = order;
            _connection.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating an order in AQWE Restaurant");
        }
        return order;
    }

    private bool CreateOrderLineMain(SqlSingle userId, SqlSingle orderId)
    {
        User localUser = new User();
        Order localOrder = new Order();
        bool[] theFinders = [false, false, false];
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM RestaurantUser, Customer", _connection))
            {
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SqlSingle id1 = SqlSingle.Parse(reader["pID"].ToString()!);
                        if (id1 == orderId)
                        {
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString email = new SqlString(reader["Email"].ToString());
                            SqlString password = new SqlString(reader["Password"].ToString());
                            SqlString phone = new SqlString(reader["Phone"].ToString());
                            SqlSingle id2 = SqlSingle.Parse(reader["ID"].ToString()!);
                            DateTime dob = DateTime.Parse(reader["DOB"].ToString()!);
                            localUser = new User(name.Value, email.Value, phone.Value, (long)id2.Value, dob.Year,
                                dob.Month, dob.Day, (long)id1.Value, password.Value);
                            theFinders[0] = true;
                        }
                    }
                }
            }
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM RestaurantOrder WHERE pID = @UserId", _connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows && theFinders[0])
                        {
                            if (long.Parse(reader["ID"].ToString()!) == userId)
                            {
                                _counter++;
                            }

                            SqlSingle quantity = SqlSingle.Parse(reader["Quantity"].ToString()!);
                            localOrder.SetUser(localUser);
                            localOrder.SetQuantity((int)quantity.Value);
                            localOrder.SetOrderId(OrderViewModel.OrderList.getSize());
                            OrderLineViewModel.OrderLineDetail.OfferOrder(localOrder);
                            theFinders[1] = true;
                        }
                    }
                }
            }
            if (_counter == 1 && theFinders[0])
            {
                OrderLineViewModel.OrderLineDetail.OfferUser(localUser);
                theFinders[2] = true;
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error inserting an order to order line in AQWE Restaurant");
        }
        return (theFinders[0] && theFinders[1]) || theFinders[2];
    }
}