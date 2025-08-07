using BusinessRestaurant;
using Data_Structure;

namespace WebRestaurant.Models;

public class ItemViewModel {
    public MWArrayList<Item> ItemList {get; set;} = new MWArrayList<Item>();
    public Item NewItem {get; set;} = new Item();
}