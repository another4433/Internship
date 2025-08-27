using BusinessRestaurant;

namespace WebRestaurant.Models;

public class UserViewModel {
    public static List<User> UserList {get; set;} = new List<User>();
    public User NewUser {get; set;} = new User();
}