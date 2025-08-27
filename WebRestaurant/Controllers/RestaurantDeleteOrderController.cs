using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantDeleteOrderController(ILogger<RestaurantRegisterController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantRegisterController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    
    private User MainUser { get; set; } =  new User();
    
    protected static ItemViewModel TheItemViewModel = new ItemViewModel();
    protected static ItemMasterViewModel TheItemMasterViewModel = new ItemMasterViewModel();
    protected static OrderViewModel TheOrderViewModel = new OrderViewModel();

    protected long ItemId, OrderId;

    [HttpGet("{id}")]
    public ActionResult Delete(long id)
    {
        try
        {
            if (ModelState.IsValid)
            {
                MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
                SqlSingle itsOrderId = new SqlSingle(id);
                DeleteOrder(itsOrderId);
                return Ok();
            }
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting an order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private void DeleteOrder(SqlSingle id)
    {
        bool found = false;
        User localUser = MainUser;
        Order localOrder = new Order(localUser.GetCustomer().GetName(), localUser.GetCustomer().GetEmail(), localUser.GetCustomer().GetPhone(), localUser.GetCustomer().GetId(), localUser.GetCustomer().GetDob().Year, localUser.GetCustomer().GetDob().Month, localUser.GetCustomer().GetDob().Day, localUser.GetUserId(), localUser.GetPassword(), (long)id.Value, 1);
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM RestaurantOrder WHERE orderID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader["Quantity"].ToString()!.Equals(null))
                        {
                            SqlSingle quantity = SqlSingle.Parse(reader["Quantity"].ToString()!);
                            localOrder.SetQuantity((int)quantity.Value);
                            found = true;
                        }
                    }
                }
            }
            if (found)
            {
                using (MySqlCommand command = new MySqlCommand("DELETE FROM RestaurantOrder WHERE orderID = @id", _connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                OrderViewModel.OrderList.remove(localOrder);
                TheOrderViewModel.NewOrder = new Order();
                RestaurantModel.NewOrder = OrderLineViewModel.OrderLineDetail.GetOrder();
                DeleteItem();
                SqlSingle theOrderId = new SqlSingle(OrderId);
                SqlSingle theItemId = new SqlSingle(ItemId);
                DeleteItemMaster(theOrderId, theItemId);
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting an order in AQWE Restaurant");
        }
    }

    private void DeleteItem()
    {
        long orderId2 = SearchOrder();
        long itemId2 = SearchItem(orderId2);
        Item localItem = new Item();
        bool[] found = [false, false, false];
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", itemId2);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            SqlString category = new SqlString(reader["Category"].ToString()!);
                            SqlString itemName = new SqlString(reader["Name"].ToString()!);
                            localItem = new Item(itemName.Value, category.Value, itemId2);
                            found[0] = true;
                        }
                    }
                }
            }
            if (found[0])
            {
                using (MySqlCommand command = new MySqlCommand("DELETE FROM Item WHERE qID = @ItemId", _connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId2);
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
                ItemViewModel.ItemList.remove(localItem);
                TheItemViewModel.NewItem = new Item();
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM ItemMaster", _connection))
                {
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["toOrder"].ToString()!.Equals(OrderLineViewModel.OrderLineDetail.GetOrder()
                                    .GetOrderId().ToString()))
                            {
                                itemId2 = (long)reader["itemID"];
                                found[1] = true;
                            }
                        }
                    }
                }
                if (found[1])
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
                    {
                        command.Parameters.AddWithValue("@Id", itemId2);
                        command.Prepare();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                {
                                    SqlString category = new SqlString(reader["Category"].ToString()!);
                                    SqlString itemName = new SqlString(reader["Name"].ToString()!);
                                    localItem = new Item(itemName.Value, category.Value, itemId2);
                                    found[2] = true;
                                }
                            }
                        }
                    }
                }
                RestaurantModel.NewItem = found[2] ? localItem : new Item();
            }
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting an item in AQWE Restaurant");
        }
    }

    private long SearchOrder()
    {
        OrderId = TheOrderViewModel.NewOrder.GetOrderId();
        return OrderId;
    }

    private long SearchItem(SqlSingle id)
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM ItemMaster WHERE toOrder = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            SqlSingle childId = SqlSingle.Parse(reader["childID"].ToString()!);
                            ItemId = (long)childId.Value;
                        }
                    }
                }
            }
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting an item in AQWE Restaurant");
        }
        return ItemId;
    }

    private void DeleteItemMaster(SqlSingle orderId2, SqlSingle itemId2)
    {
        ItemMaster theItemMaster = new ItemMaster();
        Order localOrder = new Order();
        Item localItem = new Item();
        bool[] theFounders = [false, false, false, false];
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", itemId2);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            SqlString category = new SqlString(reader["Category"].ToString()!);
                            SqlString itemName = new SqlString(reader["Name"].ToString()!);
                            localItem = new Item(itemName.Value, category.Value, (long)itemId2.Value);
                            theFounders[0] = true;
                        }
                    }
                }
            }
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM RestaurantOrder WHERE orderID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", orderId2);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            User localUser = MainUser;
                            SqlSingle quantity = SqlSingle.Parse(reader["Quantity"].ToString()!);
                            localOrder = new Order(localUser.GetCustomer().GetName(), localUser.GetCustomer().GetEmail(), localUser.GetCustomer().GetPhone(), localUser.GetCustomer().GetId(), localUser.GetCustomer().GetDob().Year, localUser.GetCustomer().GetDob().Month, localUser.GetCustomer().GetDob().Day, localUser.GetUserId(), localUser.GetPassword(), (long)orderId2.Value, (int)quantity);
                            theFounders[1] = true;
                        }
                    }
                }
            }
            using (MySqlCommand command =
                   new MySqlCommand("DELETE FROM ItemMaster WHERE toOrder = @OrderId", _connection))
            {
                command.Parameters.AddWithValue("@OrderId", OrderId);
                command.Prepare();
                command.ExecuteNonQuery();
                theFounders[2] = true;
            }
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM ItemMaster WHERE orderID = @OrderId", _connection))
            {
                command.Parameters.AddWithValue("@OrderId", OrderId);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            SqlSingle masterId = SqlSingle.Parse(reader["masterID"].ToString()!);
                            theItemMaster = new ItemMaster(localItem, localOrder, (long)masterId.Value);
                            theFounders[3] = true;
                        }
                    }
                }
            }
            if (theFounders[0] && theFounders[1] && theFounders[2] && theFounders[3])
            {
                TheItemMasterViewModel.NewItemMaster = new ItemMaster();
                ItemMasterViewModel.ItemMasterList.remove(theItemMaster);
                RestaurantModel.NewItemMaster = theItemMaster;
            }
            else if (theFounders[2])
            {
                TheItemMasterViewModel.NewItemMaster = new ItemMaster();
                RestaurantModel.NewItemMaster = new ItemMaster();
            }
            else
            {
                RestaurantModel.NewItemMaster = new ItemMaster();
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting an item item in AQWE Restaurant");
        }
    }
}