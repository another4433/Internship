using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantSearchController(ILogger<RestaurantSearchController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantSearchController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    private User MainUser { get; set; } =  new User();
    private long _orderId, _itemId;

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            if (!ModelState.IsValid) return NotFound();
            MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
            RestaurantModel.NewItemMaster = SearchItemMaster(id);
            RestaurantModel.NewItem = SearchItem(_itemId);
            RestaurantModel.NewOrder = SearchOrder(_orderId);
            RestaurantModel.NewUser = MainUser;
            return RedirectToPage("~/Order/RestaurantViewOrder");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching for an order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private Item SearchItem(long id)
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.HasRows) break;
                        SqlString name = new SqlString(reader["Name"].ToString()!);
                        SqlString category = new SqlString(reader["Category"].ToString()!);
                        return new Item(name.Value, category.Value, id);
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error searching for an item in AQWE Restaurant");
        }
        return new Item();
    }

    private Order SearchOrder(long id)
    {
        Order localOrder = new Order();
        User localUser = MainUser;
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Order WHERE orderID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.HasRows) break;
                        localOrder.SetUser(localUser);
                        SqlSingle quantity = SqlSingle.Parse(reader["Quantity"].ToString()!);
                        localOrder.SetQuantity((int)quantity.Value);
                        SqlSingle order = SqlSingle.Parse(reader["orderID"].ToString()!);
                        localOrder.SetOrderId((long)order.Value);
                        return localOrder;
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error searching for an order in AQWE Restaurant");
        }
        return localOrder;
    }

    private ItemMaster SearchItemMaster(long id)
    {
        ItemMaster itemMaster;
        Item localItem = new Item();
        Order localOrder = new Order();
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM ItemMaster WHERE masterID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.HasRows) break;
                        SqlSingle orderSingle = SqlSingle.Parse(reader["toOrder"].ToString()!);
                        _orderId = (long)orderSingle.Value;
                        SqlSingle itemSingle = SqlSingle.Parse(reader["childID"].ToString()!);
                        _itemId = (long)itemSingle.Value;
                        localItem = SearchItem(_itemId);
                        localOrder = SearchOrder(_orderId);
                        itemMaster = new  ItemMaster(localItem, localOrder, id);
                        return itemMaster;
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error searching for an item master in AQWE Restaurant");
        }
        itemMaster = new ItemMaster(localItem, localOrder, id);
        return itemMaster;
    }
}