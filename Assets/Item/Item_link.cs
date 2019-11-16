using System.Collections.Generic;
using UnityEngine;
public class Item_link {
    static public Item_link main;
    public INV_handle inv;
    public Dictionary<item_type, Inv_slot> dec = new Dictionary<item_type, Inv_slot>();
    //public Inv_slot[] slots;

	// Use this for initialization
	public Item_link ( INV_handle inv ) {

        main = this;
        this.inv = inv;
	}

    public void AddWearableSlot(Inv_slot slot)
    {
        foreach (item_type type in slot.type_of_ITEM)
        {
            if (type == item_type.none)
                continue;
            slot.type = slot_type.wearable;
            slot.link = this;
            dec.Add(type, slot);              
        }
    }
    public void SendItem(game_item item)
    {
        Inv_slot temp;
        
        if (dec.TryGetValue(item.type, out temp))
        {       
            temp.ItemAdd();
            ConfirmItem(item);
        }
    }

    public bool EquipNewItem(game_item item)
    {
        Inv_slot temp;

        if (dec.TryGetValue(item.type, out temp))
        {
            temp.SetItem(item);
            ConfirmItem(item);
            return true;
        }
        return false;
    }

    public void TakeItem(game_item item)
    {
        Inv_slot temp;
        if (dec.TryGetValue(item.type, out temp))
        {
            if(temp.item == null)
            {
                temp.ItemAdd();
                ConfirmItem(item);
            }
            else
            {
                inv.item_add();
            }
        }
        else
        {
            inv.item_add();
        }
    }

    public void ConfirmItem(game_item item)
    {
        AddItem.MyPlayer.add_item(item);
    }

    public void RemoveItem(game_item item)
    {
        AddItem.MyPlayer.Remove_Item(item);
    }
}
