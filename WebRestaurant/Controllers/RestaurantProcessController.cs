using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantProcessController(ILogger<RestaurantProcessController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantProcessController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    private User MainUser { get; set; } =  new User();
    private long _orderId, _itemId, _itemMasterId;
    private int _counter1, _counter2;
    private bool _foundUser;

    private ItemViewModel TheItemViewModel { get; set; } = new ItemViewModel();
    private ItemMasterViewModel TheItemMasterViewModel { get; set; } = new ItemMasterViewModel();
    private OrderViewModel TheOrderViewModel { get; set; } = new OrderViewModel();
    private OrderLine TheOrderLine { get; set; } = new OrderLine();

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            if (!ModelState.IsValid) return NotFound();
            MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
            DeleteOrderLineFromDb();
            return RedirectToPage("~/Home/RestaurantHome");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error process order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private OrderLine SearchOrderLine()
    {
        try
        {
            do
            {
                using (MySqlCommand command =
                       new MySqlCommand("SELECT * FROM OrderLine WHERE toOrder = @Counter", _connection))
                {
                    command.Parameters.AddWithValue("@Counter", _counter1);
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                _counter1++;
                            }
                            else
                            {
                                TheOrderLine = RestaurantModel.NewOrderLine;
                                return TheOrderLine;
                            }
                        }
                    }
                }
            } while (_counter1 < 100);
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error searching for order line in AQWE Restaurant");
        }
        return TheOrderLine;
    }

    private OrderViewModel SearchOrder()
    {
        Order order = new Order();
        OrderLine orderLine = SearchOrderLine();
        try
        {
            if (orderLine.Equals(RestaurantModel.NewOrderLine))
            {
                using (MySqlCommand command =
                       new MySqlCommand("SELECT * FROM RestaurantOrder WHERE orderID = @Index", _connection))
                {
                    command.Parameters.AddWithValue("@Index", orderLine.GetOrder().GetOrderId());
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader["userID"].ToString()!.Equals(MainUser.GetUserId().ToString())) continue;
                            User user = RestaurantModel.NewUser;
                            SqlSingle quantity = SqlSingle.Parse(reader["Quantity"].ToString()!);
                            SqlSingle orderId = SqlSingle.Parse(reader["orderID"].ToString()!);
                            order.SetQuantity((int)quantity.Value);
                            order.SetUser(user);
                            order.SetOrderId((long)orderId.Value);
                            TheOrderViewModel.NewOrder = order;
                            RestaurantModel.NewOrder = TheOrderViewModel.NewOrder;
                            _orderId = (long)orderId.Value;
                            return TheOrderViewModel;
                        }
                    }
                }
            }
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error searching for order in AQWE Restaurant");
        }
        return TheOrderViewModel;
    }

    private ItemMasterViewModel SearchItemMaster()
    {
        Order order = SearchOrder().NewOrder;
        OrderLine orderLine = SearchOrderLine();
        bool found2 = false;
        try
        {
            _connection.Open();
            if (order.Equals(RestaurantModel.NewOrder) && orderLine.Equals(RestaurantModel.NewOrderLine) &&
                _orderId == orderLine.GetOrder().GetOrderId())
            {
                using (MySqlCommand command =
                       new MySqlCommand("SELECT * FROM ItemMaster WHERE toOrder = @Index", _connection))
                {
                    command.Parameters.AddWithValue("@Index", _orderId);
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                SqlSingle singleItemId = SqlSingle.Parse(reader["childID"].ToString()!);
                                SqlSingle singleMasterId = SqlSingle.Parse(reader["masterID"].ToString()!);
                                _itemId = (long)singleItemId.Value;
                                _itemMasterId = (long)singleMasterId.Value;
                                found2 = true;
                            }
                        }
                    }
                }
            }
            if (found2)
            {
                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Item WHERE qID = @Id", _connection))
                {
                    command.Parameters.AddWithValue("@Id", _itemId);
                    command.Prepare();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows) continue;
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString quantity = new SqlString(reader["Quantity"].ToString());
                            var item = new Item(name.Value, quantity.Value, _itemId);
                            var itemMaster = new ItemMaster(item, order, _itemMasterId);
                            TheItemViewModel.NewItem = item;
                            TheItemMasterViewModel.NewItemMaster = itemMaster;
                            RestaurantModel.NewItemMaster = TheItemMasterViewModel.NewItemMaster;
                            RestaurantModel.NewItem = TheItemViewModel.NewItem;
                            return TheItemMasterViewModel;
                        }
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error searching for item in AQWE Restaurant");
        }
        return TheItemMasterViewModel;
    }

    private Order PollOrderFromQueue()
    {
        if (SearchItemMaster().NewItemMaster.Equals(RestaurantModel.NewItemMaster) &&
            SearchOrder().NewOrder.Equals(TheOrderViewModel.NewOrder) &&
            SearchOrderLine().Equals(OrderLineViewModel.OrderLineDetail))
        {
            OrderLineViewModel.OrderLineDetail.PollOrder();
            SearchUser();
            if (_foundUser && _counter2 == 1)
            {
                PollUserFromQueue();
            }
            return RestaurantModel.NewOrderLine.PollOrder();
        }
        return new Order();
    }

    private void PollUserFromQueue()
    {
        OrderLineViewModel.OrderLineDetail.PollUser();
        RestaurantModel.NewOrderLine.PollUser();
    }

    private void SearchUser()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM OrderLine", _connection))
            {
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader["userID"].ToString()!.Equals(MainUser.GetUserId().ToString())) _foundUser = false;
                        else
                        {
                            _foundUser = true;
                            _counter2++;
                        }
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error searching for user in AQWE Restaurant");
        }
    }

    private void DeleteOrderLineFromDb()
    {
        try
        {
            _connection.Open();
            if (!PollOrderFromQueue().Equals(new Order()))
            {
                using (MySqlCommand command =
                       new MySqlCommand(
                           "DELETE FROM OrderLine WHERE toOrder = @OrderId AND userID = @UserId",
                           _connection))
                {
                    command.Parameters.AddWithValue("@OrderId", OrderLineViewModel.OrderLineDetail.CheckLastOrder().GetOrderId());
                    command.Parameters.AddWithValue("@UserId", MainUser.GetUserId());
                    command.Prepare();
                    command.ExecuteNonQuery();
                    DeleteItemFromDb();
                    DeleteOrderFromDb();
                    DeleteItemMasterFromDb();
                }
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting from DB");
        }
    }

    private void DeleteOrderFromDb()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("DELETE FROM RestaurantOrder WHERE orderID = @OrderId", _connection))
            {
                command.Parameters.AddWithValue("@OrderId",
                    OrderLineViewModel.OrderLineDetail.CheckLastOrder().GetOrderId());
                command.Prepare();
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting from DB");
        }
    }

    private void DeleteItemFromDb()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Item WHERE qID = @ItemId", _connection))
            {
                command.Parameters.AddWithValue("@ItemId", _itemId);
                command.Prepare();
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting from DB");
        }
    }

    private void DeleteItemMasterFromDb()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand(
                       "DELETE FROM ItemMaster WHERE masterID = @MasterId AND childID = @ChildId AND toOrder = @OrderId"))
            {
                command.Parameters.AddWithValue("@MasterId", _itemMasterId);
                command.Parameters.AddWithValue("@ChildId", _itemId);
                command.Parameters.AddWithValue("@OrderId",
                    OrderLineViewModel.OrderLineDetail.CheckLastOrder().GetOrderId());
                command.Prepare();
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error deleting from DB");
        }
    }
}