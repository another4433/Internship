using BusinessRestaurant;

namespace WebRestaurant.Models;

public class RestaurantModel
{
    public static Customer NewCustomer { get; set; } = new Customer();
    public static User NewUser { get; set; } = new User();
    public static Item NewItem { get; set; } = new Item();
    public static ItemMaster NewItemMaster { get; set; } = new ItemMaster();
    public static Order NewOrder { get; set; } = new Order();
    public static OrderLine NewOrderLine { get; set; } = new OrderLine();
}