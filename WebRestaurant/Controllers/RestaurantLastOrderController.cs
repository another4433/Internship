using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestaurant.Models;
using BusinessRestaurant;

namespace WebRestaurant.Controllers;

public class RestaurantLastOrderController(ILogger<RestaurantLastOrderController> logger) : ControllerBase
{
    private readonly ILogger<RestaurantLastOrderController> _logger = logger;
    private User MainUser { get; set; } =  new User();
    private long _itemId;

    [HttpGet]
    public ActionResult Get()
    {
        try
        {
            if (!ModelState.IsValid) return NotFound();
            MainUser = !RestaurantRegisterController.GlobalUser.Equals(new User()) ? RestaurantLoginController.GlobalUser : RestaurantRegisterController.GlobalUser;
            RestaurantModel.NewOrder = GetOrder();
            RestaurantModel.NewItemMaster = GetItemMaster();
            RestaurantModel.NewItem = GetItem();
            return RedirectToPage("~/Last/RestaurantLastOrder");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing last order in AQWE Restaurant");
            return StatusCode(500);
        }
    }

    private Order GetOrder()
    {
        Order order = new Order();
        User user = MainUser;
        order.SetUser(user);
        order.SetOrderId(OrderLineViewModel.OrderLineDetail.CheckLastOrder().GetOrderId());
        order.SetQuantity(OrderLineViewModel.OrderLineDetail.CheckLastOrder().GetQuantity());
        return order;
    }

    private ItemMaster GetItemMaster()
    {
        ItemMaster itemMaster = new ItemMaster();
        Order useIt = GetOrder();
        itemMaster.SetOrder(useIt);
        int orderIndex = OrderViewModel.OrderList.search(useIt);
        _itemId = orderIndex;
        ItemMaster item = ItemMasterViewModel.ItemMasterList.get(orderIndex);
        item.SetOrder(useIt);
        return item;
    }

    private Item GetItem()
    {
        return ItemViewModel.ItemList.get((int)_itemId);
    }
}