using BusinessRestaurant;

namespace WebRestaurant.Models;

public class HomeView
{
    public static Queue<OrderLine> queueOrders { get; set; } = new Queue<OrderLine>();
    public OrderLine orderLine { get; set; } = new OrderLine();
    public static List<ItemMaster> listItemMaster { get; set; } = new List<ItemMaster>();
    public ItemMaster itemMaster { get; set; } = new ItemMaster();
}