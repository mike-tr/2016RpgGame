using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {
	public static Transform itT;
	static public ItemDrop Global_ItemDrop;
	static public List<DroppedItem> DroppedItems = new List<DroppedItem>();
	// Use this for initialization
	private GameObject item_dropped;

	private Transform Items_t;
	IEnumerator Start()
	{
		item_dropped = (GameObject)Resources.Load("Other/prefabs/Item");
		Global_ItemDrop = this;		

		Items_t = new GameObject().transform;
		Items_t.name = "DroppedItems";
		Items_t.position = Vector2.zero;
		Items_t.eulerAngles = Vector3.zero;
		itT = Items_t;

		while( Grid_creator.instance == null || !Grid_creator.instance.Loaded)
			yield return new WaitForSeconds(Time.deltaTime * 3);
		for(int i = 0; i <  10; i ++)
		{
			Vector2 pos = new Vector2(Random.Range(-400f,400f), Random.Range(-400f, 400f));
			while(!Grid_creator.instance.NodeFromWorldPoint(pos).walkable)
				pos = new Vector2(Random.Range(-400f,400f), Random.Range(-400f, 400f));
			DropItem(pos);
		}
	}
	public void DropItem(game_item item, Vector2 pos)
	{

		foreach(DroppedItem item_d in DroppedItems)
		{
			if(!item_d.gameObject.activeSelf)
				{
					item_d.gameObject.SetActive(true);
					item_d.SetItem(item);
					item_d.transform.position = pos;
					return;
				}
		}

		Transform child = Instantiate(item_dropped).transform;
        DroppedItem ditem = child.GetComponent<DroppedItem>(); 
		ditem.SetItem(item);

		child.SetParent(Items_t);
        child.localPosition = pos;
        child.localScale = new Vector3(50, 50, 1);

	}

	public void DropItem(Vector2 pos)
	{
		foreach(DroppedItem item_d in DroppedItems)
		{
			if(!item_d.gameObject.activeSelf)
				{
					item_d.gameObject.SetActive(true);
					item_d.RandomItem();
					item_d.transform.position = pos;
					return;
				}
		}

		Transform child = Instantiate(item_dropped).transform;
        DroppedItem ditem = child.GetComponent<DroppedItem>(); 
		ditem.RandomItem();

		child.SetParent(Items_t);
        child.localPosition = pos;
        child.localScale = new Vector3(50, 50, 1);
	}
}
