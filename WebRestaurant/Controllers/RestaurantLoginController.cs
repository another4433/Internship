using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantLoginController(ILogger<RestaurantLoginController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantLoginController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    public static User GlobalUser { get; set; } = new User();
    protected static UserViewModel TheUserViewModel = new UserViewModel();

    [HttpGet]
    public ActionResult<IEnumerable<UserViewModel>> GetUsers()
    {
        try
        {
            var users = UserViewModel.UserList;
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult<UserViewModel> Post([FromBody] UserViewModel user)
    {
        try
        {
            var theUser = CheckUser(user.NewUser.GetCustomer().GetEmail(), user.NewUser.GetPassword());
            if (ModelState.IsValid && !theUser.Equals(new UserViewModel()))
            {
                GlobalUser = theUser.NewUser;
                TheUserViewModel = theUser;
                RestaurantModel.NewUser = GlobalUser;
                _logger.LogInformation("Welcome, {GetEmail}", GlobalUser.GetCustomer().GetEmail());
                return RedirectToPage("~/Home/RestaurantHome");
            }
            _logger.LogError(message: "Failed to login! Invalid credentials.");
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in to AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private UserViewModel CheckUser(string Email,  string Password)
    {
        UserViewModel user = new UserViewModel();
        bool[] finders = [false, false];
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("select * from restaurantuser where Password = @Password", _connection))
            {
                command.Parameters.AddWithValue("@Password", Password);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            finders[0] = true;
                            SqlString password = new SqlString(reader["Password"].ToString());
                            SqlSingle userId = SqlSingle.Parse(reader["pID"].ToString()!);
                            SqlSingle customerId = SqlSingle.Parse(reader["customerID"].ToString()!);
                            user.NewUser.SetUserId((long)userId.Value);
                            user.NewUser.SetPassword(password.Value);
                            user.NewUser.GetCustomer().SetId((long)customerId.Value);
                            TheUserViewModel.NewUser = user.NewUser;
                        }
                    }
                }
            }
            using (MySqlCommand command = new MySqlCommand("select * from customer where Email = @Email", _connection))
            {
                command.Parameters.AddWithValue("@Email", Email);
                command.Prepare();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            finders[1] = true;
                            SqlString email = new SqlString(reader["Email"].ToString());
                            SqlString name = new SqlString(reader["Name"].ToString());
                            SqlString phone = new SqlString(reader["Phone"].ToString());
                            SqlSingle customerId = SqlSingle.Parse(reader["customerID"].ToString()!);
                            DateTime dob = DateTime.Parse(reader["DOB"].ToString()!);
                            user.NewUser.SetCustomer(name.Value, email.Value, phone.Value, (long)customerId.Value, dob.Date);
                            TheUserViewModel.NewUser = user.NewUser;
                        }
                    }
                }
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error checking login authentication of the user!");
        }
        return finders[0] && finders[1] ? user : new UserViewModel();
    }
}