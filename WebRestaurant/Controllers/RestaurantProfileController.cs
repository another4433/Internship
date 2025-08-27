using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantProfileController(ILogger<RestaurantProfileController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantProfileController> _logger = logger;
    private User MainUser { get; set; } =  new User();
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);

    [HttpGet]
    public ActionResult Get()
    {
        try
        {
            if (ModelState.IsValid)
            {
                MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
                RestaurantModel.NewUser = SearchUser() ? MainUser : new User();
                RestaurantModel.NewCustomer = SearchCustomer() ? MainUser.GetCustomer() : new Customer();
                return RedirectToPage("~/Profile/RestaurantProfile");
            }
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing profile in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private bool SearchUser()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("SELECT * FROM RestaurantUser WHERE pID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", MainUser.GetUserId());
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows) return true;
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error viewing profile in AQWE Restaurant");
        }
        return false;
    }

    private bool SearchCustomer()
    {
        try
        {
            _connection.Open();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Customer WHERE ID = @Id", _connection))
            {
                command.Parameters.AddWithValue("@Id", MainUser.GetCustomer().GetId());
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows) return true;
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException exception)
        {
            _logger.LogError(exception, "Error viewing profile in AQWE Restaurant");
        }
        return false;
    }
}