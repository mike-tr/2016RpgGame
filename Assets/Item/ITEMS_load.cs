using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ITEMS_load : MonoBehaviour {

    public static ITEMS_load Get;
    public static bool item_Loaded = false;
    public LoadExternalAsSprite les;


    public AddItem add;
    public static AddItem Add_items;

    public Sprite AgilityIcon;

    private ITEMS ItemCreator;
    // Use this for initialization
    void Start()
    {
        if(Application.isEditor)
            C_Stats.ACT = 1f;
        else if(Application.isMobilePlatform)
        {
            C_Stats.ACT = 1.0f;
            ChatGUI.addLine("MobilePlatform Spotted!");
        }
        Get = this;

        ChatGUI.addLine("This Is The Path for Textures!");
        ChatGUI.addLine("Put Textures folder in there, then reload everything to implement the change!");
        ChatGUI.addLine(Application.persistentDataPath);
        
        ItemCreator = new ITEMS();
        ITEMS.main = ItemCreator;
        ItemCreator.ADDITEMS();
        ITEMS.main.CreateStatEnchanter(0,0,0,0,0,0,0,0,0,0,0,3,0, " Range_Enchanter", AgilityIcon);
        //ITEMS.main.CreateStatEnchanter(0,55,0,0,0,0,0,0,0,0,0,3,0, " Rang2", AgilityIcon);
        //ITEMS.main.CreateStatEnchanter(0,0,55,0,0,0,0,0,0,0,0,3,0, " Rang3r", AgilityIcon);

        Add_items = add;
        les = GetComponent<LoadExternalAsSprite>();



        StartCoroutine(load_items());
    }

    void Awake()
    {
        INV_handle.main = new INV_handle();
        StartCoroutine(ItemLoad());
    }

    IEnumerator ItemLoad()
    {
        while (INV_handle.main == null)
            yield return new WaitForSeconds(Time.deltaTime);
        item_Loaded = true;
    }

    public void LoadItems(Transform _transform, GameObject UIButton, INV_handle add_t)
    {
        StartCoroutine(loadAllinv(_transform, UIButton, add_t));
    }

    IEnumerator load_items()
    {
        yield return ITEMS.main.load_icons();
    }


    public IEnumerator loadAllinv(Transform _transform, GameObject UIButton, INV_handle add_t)
    {
        foreach (Transform t in _transform)
            t.gameObject.AddComponent<DestroySelf>();
        yield return new WaitForSeconds(Time.deltaTime);

        int i = 0;
        foreach(SpriteGender gender in Enum.GetValues(typeof(SpriteGender)))
        {
            if(gender == SpriteGender.none)
                continue;
            foreach(item_type type in Enum.GetValues(typeof(item_type)))
                foreach(game_item item in ItemCreator.Game_Items[gender][type].Values)
                {
                    yield return Add_item(item, _transform, UIButton, add_t);

                    if (i % 7 == 0)
                        yield return new WaitForSeconds(Time.deltaTime);
                    i++;
                }
        }

        /*
        for (int i = 0; i < ITEMS.items.Count; i++)
        {
            yield return Add_item(ITEMS.items[i], _transform, UIButton, add_t);

            if (i % 7 == 0)
                yield return new WaitForSeconds(Time.deltaTime);
        }*/

    }

    public Sprite test;
    private IEnumerator Add_item(game_item item, Transform _transform, GameObject UIButton, INV_handle add_t)
    {
        if (DontDestroy.first.Loading)
            yield break;
        Transform child = Instantiate(UIButton).transform;
        Image image = child.GetChild(0).GetComponent<Image>();
        Inv_slot ci = child.GetComponent<Inv_slot>();
        ci.item = item;
        item.count = 9999;
        ci.ai = add_t;
        child.name = item.name;
        child.SetParent(_transform);

        Sprite sd = null;
        while (sd == null)
        {
            if (ITEMS.main.item_icons.TryGetValue(item.id_sprite, out sd))
                image.sprite = sd;
            yield return new WaitForSeconds(3 * Time.deltaTime);
        }

        image.color = item.rarity.color;
        

        child.localPosition = Vector3.zero;
        child.localScale = new Vector3(1, 1, 1);

        yield return null;
    }
}
