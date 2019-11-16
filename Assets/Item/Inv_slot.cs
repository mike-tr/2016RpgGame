using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Inv_slot : MonoBehaviour,
     IPointerDownHandler,
     IPointerUpHandler,
     IDropHandler
{


    public static Inv_slot item_Poiner;
    public game_item item;
    public INV_handle ai;
    public item_type[] type_of_ITEM;

    public slot_type type = slot_type.all_items_inv;
    public Item_link link;


    public item_type ShowType_ITEM;
    public item_type get_type_ITEM
    {
        get
        {
            return item.type;
        }
        set
        {
            ShowType_ITEM = value;
        }
    }

    ItemIconHandle IIH;
    IEnumerator Start()
    {   
        while(INV_handle.main == null && Item_link.main == null)
            yield return new WaitForSeconds(Time.deltaTime * 3);
        ai = INV_handle.main;
        if(type == slot_type.inventory)
            ai.AddSlot(this);
        else if(type == slot_type.wearable)
            Item_link.main.AddWearableSlot(this);
        IIH = GetComponent<ItemIconHandle>();
    }

    void LoadAll()
    {

    }

    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            if (item_Poiner == null)
                return;
            if (item_Poiner.item != null && type != slot_type.all_items_inv)
            {           
                ItemAdd();         
            }
        }
    }

    public game_item SwapSlot(game_item sw_item)
    {
        game_item temp = item; 
        this.item = sw_item;
        item.slot_pointer = this;

        IIH.SetSprite(ITEMS.main.item_icons[item.id_sprite], sw_item.rarity.color);

        get_type_ITEM = item.type;
        return temp;
    }

    public bool ItemAdd()
    {
        bool swap = false;
        if (type_of_ITEM.Length <= 0 || EnumCheck<item_type>.EnumArrayContains(type_of_ITEM, item_Poiner.get_type_ITEM))
        {
            if (item != null && item_Poiner.type != slot_type.all_items_inv)
            {
                print(item_Poiner.type);
                swap = true;
                this.item = item_Poiner.SwapSlot(item);
                item.slot_pointer = this;
            }
            else
            {
                this.item = (item_Poiner.type == slot_type.all_items_inv) ?
                             item_Poiner.item.CopyItem() : item_Poiner.item;
                item.slot_pointer = this;           
            }
        }
        else
            return false;

        if (!swap)
        {
            if(item.count > 1 || item_Poiner.type == slot_type.all_items_inv)
            {
                item = item.CopyItem();
                item.count = 1;
                item_Poiner.ShareItem(1);
            }
            else
            {
                item.count = 1;
                item_Poiner.RemoveItem();
            }
        }

        IIH.SetSprite( ITEMS.main.item_icons[item.id_sprite] ,item.rarity.color );

        get_type_ITEM = item.type;

        if (type == slot_type.wearable)
            link.ConfirmItem(item);


        return true;
    }

    bool click = false;
    IEnumerator doubleclick()
    {
        click = true;
        yield return new WaitForSeconds(1.0f);
        click = false;
    }

    public void SetItem(game_item cItem)
    {
        this.item = cItem;
        item.slot_pointer = this;

        IIH.SetSprite(item.id_sprite, cItem.rarity.color);

        get_type_ITEM = item.type;
    }

    public void GetItem()
    {
        item_Poiner = this;
        switch(type)
        {
            case slot_type.all_items_inv:
                ai.item_add();
                break;
            case slot_type.inventory:
                ai.UseItem(item);
                break;
            case slot_type.wearable:
                ai.item_add();
                break;
            case slot_type.dropped:
                ai.PickItem(item);
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (click == true)
                {
                    GetItem();           
                }
                else
                {
                    item_Poiner = this;
                    StartCoroutine(doubleclick());
                }
            }
            
        }
        
        else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                if(item != null)
                {
                    UpgradeItem();
                }

            }
            
        else if (eventData.button == PointerEventData.InputButton.Right)
            {
                DropItem();
            }
            
    }

    public void UpgradeItem()
    {
        if(type == slot_type.wearable || type == slot_type.all_items_inv)
            return;
        item.rarity.Item_Level++;
        ChatGUI.addLine(item.name + " was improvep - " + item.rarity.Item_Level + "lvl" );
    }

    public void DropItem(Vector3 pos)
    {
        ItemDrop.Global_ItemDrop.DropItem(item, pos);
        //ChatGUI.addLine(item.name + " |" + item.rarity.Item_Level + "lvl|" + " was Dropped!" );
        IIH.ResetSprite();
        if(type == slot_type.wearable)
            link.RemoveItem(item);
        this.item = null;
    }

    public void DropItem()
    {
        if(type == slot_type.all_items_inv)
            return;
        ItemDrop.Global_ItemDrop.DropItem(item, AddItem.MyPlayer.transform.position);
        ChatGUI.addLine(item.name + " |" + item.rarity.Item_Level + "lvl|" + " was Dropped!" );
        IIH.ResetSprite();
        if(type == slot_type.wearable)
            link.RemoveItem(item);
        this.item = null;
    }
    public void ShareItem(int quantity)
    {
        if (type == slot_type.all_items_inv)
            return;
        item.count--;
        if (item.count <= 0)
        {
            RemoveItem();
        }
    }

    public void RemoveItem()
    {
        IIH.ResetSprite();

        if(type == slot_type.wearable)
            link.RemoveItem(item);
        this.item = null;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            try
            {
                Debug.Log("Left " + item.name);
            }
            catch { }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle");
        else if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("Right");
    }

    static public void ConsumeItem()
    {
        item_Poiner.ShareItem(1);
    }
}

public enum slot_type
{
    inventory,
    all_items_inv,
    wearable,
    dropped,
}


