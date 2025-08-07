using BusinessRestaurant;
using Data_Structure;

namespace WebRestaurant.Models;

public class ItemMasterViewModel {
    public LWLinkedList<ItemMaster> ItemMasterList {get; set;} = new LWLinkedList<ItemMaster>();
    public ItemMaster NewItemMaster {get; set;} = new ItemMaster();
}