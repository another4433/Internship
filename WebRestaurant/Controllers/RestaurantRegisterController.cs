using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient; 
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantRegisterController(ILogger<RestaurantRegisterController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantRegisterController> _logger = logger;
    private const string ConnectionString = "server=aqwe443346;database=therestaurant;user=born2ready;password=born2ready;";
    private readonly MySqlConnection _connection = new MySqlConnection(ConnectionString);
    protected static UserViewModel TheUserViewModel { get; set; } = new UserViewModel();
    protected static CustomerViewModel TheCustomerViewModel { get; set; } = new CustomerViewModel();
    public static User GlobalUser { get; set; } = new User();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult<User> Post([FromBody] UserViewModel user)
    {
        try
        {
            SqlString name = new SqlString(user.NewUser.GetCustomer().GetName());
            SqlSingle customerId = new SqlSingle(user.NewUser.GetCustomer().GetId());
            SqlString email = new SqlString(user.NewUser.GetCustomer().GetEmail());
            SqlString phone = new SqlString(user.NewUser.GetCustomer().GetPhone());
            DateTime dob = user.NewUser.GetCustomer().GetDob();
            SqlString password = new SqlString(user.NewUser.GetPassword());
            UserViewModel userViewModel = CreateUser(name, customerId, email, password, phone, dob);
            if (ModelState.IsValid && !userViewModel.Equals(new UserViewModel()))
            {
                TheUserViewModel = userViewModel;
                TheUserViewModel.NewUser = userViewModel.NewUser;
                CustomerViewModel customerViewModel = new CustomerViewModel();
                customerViewModel.NewCustomer.SetId((long)customerId.Value);
                customerViewModel.NewCustomer.SetEmail(email.Value);
                customerViewModel.NewCustomer.SetPhone(phone.Value);
                customerViewModel.NewCustomer.SetName(name.Value);
                customerViewModel.NewCustomer.SetDob(dob);
                TheCustomerViewModel = customerViewModel;
                TheCustomerViewModel.NewCustomer = customerViewModel.NewCustomer;
                RestaurantModel.NewCustomer = customerViewModel.NewCustomer;
                GlobalUser = TheUserViewModel.NewUser;
                RestaurantModel.NewUser = GlobalUser;
                _logger.LogInformation("The user {GetId} has been created.", GlobalUser.GetCustomer().GetId());
                return RedirectToPage("~/Login/RestaurantLogin");
            }
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

    private UserViewModel CreateUser(SqlString name, SqlSingle customerId, SqlString email, SqlString password, SqlString phone, DateTime dateOfBirth)
    {
        CustomerViewModel customer = new CustomerViewModel();
        UserViewModel user = new UserViewModel();
        try
        {
            _connection.Open();
            using (MySqlCommand command =
                   new MySqlCommand("INSERT INTO Customer VALUES (@Name, @ID, @DOB, @Email, @Phone)", _connection))
            {
                command.Parameters.Add(new MySqlParameter("@Name", name.Value));
                command.Parameters.Add(new MySqlParameter("@ID", customerId.Value));
                command.Parameters.Add(new MySqlParameter("@DOB", dateOfBirth.Date));
                command.Parameters.Add(new MySqlParameter("@Email", email.Value));
                command.Parameters.Add(new MySqlParameter("@Phone", phone.Value));
                command.Prepare();
                command.ExecuteNonQuery();
                customer.NewCustomer.SetId((long)customerId.Value);
                customer.NewCustomer.SetEmail(email.Value);
                customer.NewCustomer.SetPhone(phone.Value);
                customer.NewCustomer.SetName(name.Value);
                customer.NewCustomer.SetDob(dateOfBirth.Date);
                CustomerViewModel.CustomerList.Add(customer.NewCustomer);
            }
            using (MySqlCommand command = new MySqlCommand("INSERT INTO RestaurantUser VALUES (@UserId, @Password, @CustomerId)", _connection))
            {
                command.Parameters.Add(new MySqlParameter("@UserId", UserViewModel.UserList.Count));
                command.Parameters.Add(new MySqlParameter("@Password", password.Value));
                command.Parameters.Add(new MySqlParameter("@CustomerId", customerId.Value));
                command.Prepare();
                command.ExecuteNonQuery();
                user.NewUser.SetCustomer(name.Value, email.Value, phone.Value, (long)customerId.Value, dateOfBirth.Date);
                user.NewUser.SetUserId(UserViewModel.UserList.Count);
                user.NewUser.SetPassword(password.Value);
                UserViewModel.UserList.Add(user.NewUser);
            }
            _connection.Close();
        }
        catch (MySqlException ex)
        {
            _logger.LogError(ex, "Error creating user!");
        }
        return user;
    }
}