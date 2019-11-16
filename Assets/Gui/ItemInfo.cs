using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {
	static public ItemInfo InfoGui;
    public Text strength;
    public Text agility;
    public Text intelligence;
    public Text range;
    public Text mobiity;
    public Text endurance;
	public Text Item_name;
	public Text Item_Quantity;
	public Text Item_level;
    // Use this for initialization

	Inv_slot slot;
	Basic_Stats stats;
	UseBoost pstats;
    IEnumerator Start () {
        while (Info_update.instanse == null)
            yield return new WaitForSeconds(Time.deltaTime);
        Info_update.instanse.UpdateItemInfo = this;
		slot = Inv_slot.item_Poiner;
	}

	public void Update()
	{
		if(slot != Inv_slot.item_Poiner)
		{
			slot = Inv_slot.item_Poiner;
			if(slot != null && slot.item != null)
			{
				stats = slot.item.bstats;
				pstats = slot.item.pstats;
			}
		}
	}

	public void DropItem()
	{
		if(slot != null && slot.item != null)
			slot.DropItem();
	}
	public void UpgradeItem()
	{
		if(slot != null && slot.item != null)
			slot.UpgradeItem();
	}
    public void ReWriteStats()
    {
		if(slot == null || slot.item == null)
		{
			strength.text = "";
        	agility.text = "";
        	intelligence.text = "";
        	range.text = "";
        	mobiity.text = "";
        	endurance.text = "";
			Item_name.text = "none";
			Item_Quantity.text = "";
			Item_level.text = "";
			return;
		}
			
        strength.text = (stats.strength).ToString("F0");
        agility.text = (stats.agility).ToString("F0");
        intelligence.text = (stats.intelligence).ToString("F0");
        range.text = (pstats.range).ToString("F1");
        mobiity.text = (pstats.walk_speed).ToString("F0");
        endurance.text = (stats.endurance).ToString("F0");
		Item_name.text = slot.item.name;
		Item_Quantity.text = (slot.item.count).ToString("F0");
		Item_level.text = (slot.item.rarity.Item_Level).ToString("F0");
    }
}
