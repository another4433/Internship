using BusinessRestaurant;

namespace WebRestaurant.Models;

public class OrderLineSideViewModel {
    public Stack<OrderLine> TheOrderLine {get; set;} = new Stack<OrderLine>();
    public OrderLine OrderLineDetail {get; set;} = new OrderLine();
}