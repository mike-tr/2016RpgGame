using System.Collections.Generic;
using UnityEngine;
public class INV_handle {
    static public INV_handle main;
    List<Inv_slot> slots = new List<Inv_slot>();

    public Item_link link;

	// Use this for initialization
	public INV_handle() {
        link = new Item_link(this);
        main = this;
	}
	
    public void AddSlot(Inv_slot slot)
    {
        slots.Add(slot);
        slot.type = slot_type.inventory;
    }
	// Update is called once per frame

    public void item_add()
    {
        foreach(Inv_slot slot in slots)
        {
            if(slot.item == null)
            {
                if (slot.ItemAdd())
                    break;
            }
        }
    }

    public void PickItem(game_item item)
    {
        link.TakeItem(item);
    }
    public void UseItem(game_item item)
    {
        if(item.type == item_type.consumable)
        {
            AddItem.MyPlayer.Use_Consumable(item);
            Inv_slot.ConsumeItem();
        }
        else if (item.type != item_type.none)
        {
            link.SendItem(item);
            Debug.Log("done!");
        }
    }
}
