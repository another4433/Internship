using BusinessRestaurant;
using Data_Structure;

namespace WebRestaurant.Models;

public class OrderViewModel {
    public MWArrayList<Order> OrderList {get; set;} = new MWArrayList<Order>();
    public Order NewOrder {get; set;} = new Order();
}