using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inv_slot))]
[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : MonoBehaviour {

	Inv_slot slot;
	SpriteRenderer srenderer;
	Transform p;
	// Use this for initialization

	bool loaded = false;
	bool GotItem = false;
	IEnumerator Start () {
		slot = GetComponent<Inv_slot>();
		srenderer = GetComponent<SpriteRenderer>();
		
		while(INV_handle.main == null)
			yield return new WaitForSeconds(Time.deltaTime * 3);
		
		slot.ai = INV_handle.main;

		p = AddItem.MyPlayer.transform;

		ItemDrop.DroppedItems.Add(this);

		loaded = true;

		if(!GotItem)
		{
			RandomItem();
		}
	}
	
	public void SetItem(game_item item)
	{
		StartCoroutine(SET(item));
	}

	public void RandomItem()
	{
		StartCoroutine(SET(ITEMS.main.RandomItem(SpriteGender.male)));
	}

	IEnumerator SET(game_item item)
	{ 
		GotItem = true;
		yield return new WaitForSeconds(Time.deltaTime);
		while(!loaded)
			yield return new WaitForSeconds(Time.deltaTime);
		slot.SetItem(item);
	}
	void Update()
	{
		transform.eulerAngles += (new Vector3(0, Random.Range(0.5f, 1f), Random.Range(0.5f, 1f))) * 25 * Time.deltaTime;

		if(OnKeyBoard_ui.TakeItem && !AddItem.MyPlayer.Dead)
		{
			if(Vector2.Distance(transform.position, p.position) < 25f)
			{ 
				if(slot.item != null)
				{
					slot.GetItem();
					if(slot.item == null)
						gameObject.SetActive(false);
				}				
			}
		}
		
	}

}
