using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Inv_slot))]
public class ItemIconHandle : MonoBehaviour {

	// Use this for initialization
	public Sprite defs;
    public Image img;

	public Sprite sprite
	{
		set
		{
			if(IsDropped)
			{
				srd.sprite = value;
				sri.sprite = value;
			}
			else
			{
				img.sprite = value;
			}
		}
	}

	public Color color 
	{
		set
		{
			if(IsDropped)
			{
				srd.color = value;
				sri.color = new Color( value.r, value.g, value.b, 0.4f);
			}
			else
			{
				img.color = value;
			}
		}
	}

	public SpriteRenderer srd;
	public SpriteRenderer sri;
	bool IsDropped =false;
	void Start () {
		try{
			img = transform.GetChild(0).GetComponent<Image>();
			defs = img.sprite;
			IsDropped = false;
		}
		catch
		{
			srd = GetComponent<SpriteRenderer>();
			sri =  transform.GetChild(0).GetComponent<SpriteRenderer>();
			defs = null;
			IsDropped = true;
		}
	}
	public void SetSprite(Sprite s, Color color)
	{
		sprite = s;
		this.color = color;
	}

	public void SetSprite(int id_sprite, Color color)
	{
		ITEMS_load.Get.StartCoroutine(W8ForSprite(id_sprite, color));
	}

	public IEnumerator W8ForSprite(int id_sprite, Color color)
	{
		Sprite sd = null;
        while (sd == null)
        {
            if (ITEMS.main.item_icons.TryGetValue(id_sprite, out sd))
			{
				sprite = sd;
				this.color = color;
				break;
			}
            yield return new WaitForSeconds(3 * Time.deltaTime);
        }
		
	}
	public void ResetSprite()
	{
		sprite = defs;
		this.color = Color.white;
	}
}
