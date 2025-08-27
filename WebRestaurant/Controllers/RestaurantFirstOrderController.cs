using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantFirstOrderController(ILogger<RestaurantFirstOrderController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantFirstOrderController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    private User MainUser { get; set; } =  new User();
    private long _itemId;

    [HttpGet]
    public ActionResult Get()
    {
        try
        {
            if (!ModelState.IsValid) return NotFound();
            MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
            RestaurantModel.NewItemMaster = GetItemMaster();
            RestaurantModel.NewOrder = GetOrder();
            RestaurantModel.NewOrderLine = OrderLineViewModel.OrderLineDetail;
            RestaurantModel.NewUser = MainUser;
            return RedirectToPage("~/First/RestaurantFirstOrder");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing first order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private Order GetOrder()
    {
        User user = MainUser;
        Order localOrder = new Order();
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM orders WHERE orderID = @id", _connection))
            {
                command.Parameters.AddWithValue("@id", OrderLineViewModel.OrderLineDetail.GetOrder().GetOrderId());
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            localOrder.SetUser(user);
                            SqlSingle theOrderId2 = SqlSingle.Parse(reader["orderID"].ToString()!);
                            SqlSingle quantity = SqlSingle.Parse(reader["quantity"].ToString()!);
                            localOrder.SetQuantity((int)quantity.Value);
                            localOrder.SetOrderId((long)theOrderId2.Value);
                            return localOrder;
                        }
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error viewing order in AQWE Restaurant");
        }
        return localOrder;
    }

    private ItemMaster GetItemMaster()
    {
        Order localOrder = new Order();
        Item localItem = new Item();
        Order useIt = GetOrder();
        bool theFound = false;
        long itemMasterId = 0;
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM ItemMaster WHERE toOrder = @OrderId", _connection))
            {
                command.Parameters.AddWithValue("@OrderId", useIt.GetOrderId());
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.HasRows) break;
                        SqlSingle theItemId = SqlSingle.Parse(reader["childID"].ToString()!);
                        _itemId = (long)theItemId.Value;
                        SqlSingle theMasterId = SqlSingle.Parse(reader["masterID"].ToString()!);
                        itemMasterId = (long)theMasterId.Value;
                        theFound = true;
                    }
                }
            }
            if (theFound)
            {
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
                {
                    command.Parameters.AddWithValue("@Id", _itemId);
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows) break;
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString category = new SqlString(reader["Category"].ToString());
                            localItem.SetName(name.Value);
                            localItem.SetCategory(category.Value);
                            localItem.SetItemId(_itemId);
                            RestaurantModel.NewItem = localItem;
                            return new ItemMaster(localItem, useIt, itemMasterId);
                        }
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error viewing item in AQWE Restaurant");
        }
        return new ItemMaster(localItem, localOrder, itemMasterId);
    }
}