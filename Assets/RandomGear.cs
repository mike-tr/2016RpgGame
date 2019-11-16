using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGear : MonoBehaviour {

	// Use this for initialization
    AddItem item;
	public bool Basic_dager = false;
	IEnumerator Start () {
		
		item = GetComponent<AddItem>();
		if(!Basic_dager)
		{
            if (GetComponent<CharacterControl>().IsMine)
            {
                foreach (game_item itemg in ITEMS.main.GetRandomGear(SpriteGender.male))
                {
                    int i = 0;
                    while (!Item_link.main.EquipNewItem(itemg))
                    {
                        yield return new WaitForSeconds(Time.deltaTime * 3);
                        i++;
                        if (i > 50)
                        {
                            print("cannot find slot!");
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (game_item itemg in ITEMS.main.GetRandomGear(SpriteGender.male))
                {
                    item.add_item(itemg);
                }
            }
			//item.add_item(ITEMS.main.GetItemFrom());
		}
		else
		{
			game_item starting_weapon = ITEMS.main.GetItemFrom(SpriteGender.male, 
										item_type.melee_weapons, item_rarity.rarity.simple).CopyItem();
			
			if(GetComponent<CharacterControl>().IsMine)
			{

				//item.add_item(starting_weapon);
				
				int i = 0;
				while(!Item_link.main.EquipNewItem(starting_weapon))
				{
					yield return new WaitForSeconds(Time.deltaTime * 3);
					i++;
					if(i > 50)
					{
						print("cannot find slot!");
						break;
					}
				}

			}
			else
			{
				item.add_item(starting_weapon);
			}
		}

	}	

}
