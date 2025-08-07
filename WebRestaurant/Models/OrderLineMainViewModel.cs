using BusinessRestaurant;

namespace WebRestaurant.Models;

public class OrderLineMainViewModel {
    public Queue<OrderLine> TheOrderLine {get; set;} = new Queue<OrderLine>();
    public OrderLine OrderLineDetail {get; set;} = new OrderLine();
}