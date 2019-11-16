using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class game_item
{
    private static int ITEM_COUNTER = 0;
    private static int CONSUMABLE_SP_ID = -2;
    private int ID;
    public int count = 1;
    public int id { get { return ID; } }
    public Inv_slot slot_pointer;
    public game_item(item_type _type, int _id_sprite, SpriteGender _gender, 
        int _Order_ID, string _name, Basic_Stats stats, UseBoost pstats, item_rarity.rarity _rarity, bool Is_armor , int item_level = 0)
    {
        ID = ITEM_COUNTER;
        ITEM_COUNTER++;

        bstats = stats;
        this.pstats = pstats;
        IsArmor = Is_armor;

        NAME = _name;
        type = _type;
        id_sprite = _id_sprite;
        gender = _gender;
        ORD = _Order_ID;
        slot_pointer = null;
        count = 1;

       if(_id_sprite == -1)
        {
            id_sprite = CONSUMABLE_SP_ID;
            CONSUMABLE_SP_ID--;
        }
        if(ORD == -1)
        {
            ORD = CONSUMABLE_SP_ID;
        }

        rarity = new item_rarity(bstats, _rarity, item_level);

    }

    private string NAME;
    public string name
    {
        get
        {
            return NAME;
        }
    }

    private int ORD;
    public int Order_ID { get { return ORD; } }
    public item_type type;
    public bool IsArmor;
    public int id_sprite;
    public SpriteGender gender;
    public Basic_Stats bstats;
    public item_rarity rarity;
    public UseBoost pstats;

    public game_item CopyItem()
    {
        return new game_item(type, id_sprite, gender, ORD, NAME, bstats.copy_stats(), pstats.Copy(), rarity.Rarity, IsArmor, 0);
    }

}
	
public enum item_type
{
    
    consumable = -1,
    none = 0,
    wings,
    melee_weapons,
    spears,
    shilds,
    bow,
    wand,
    arrows,
    chain,
    chest,
    shoulders,
    legs,
    feet, 
}

public class ITEMS
{
    public static ITEMS main;
    public Dictionary<SpriteGender, Dictionary<Skin_type, Dictionary<body_part , List<Body_part>>>> body_parts 
        = new Dictionary<SpriteGender, Dictionary<Skin_type, Dictionary<body_part , List<Body_part>>>>();

    public Dictionary<SpriteGender, Dictionary<item_type, Dictionary<int ,game_item>>> Game_Items 
        = new Dictionary<SpriteGender, Dictionary<item_type, Dictionary<int,game_item>>>();

    public game_item walk_anim;

    public List<Body_part> GetPerson(SpriteGender gender, Skin_type type)
    {
        List<Body_part> ret = new List<Body_part>();
        foreach(body_part p in Enum.GetValues(typeof(body_part)))
        {
            int random = InGameData.random_int(0, body_parts[gender][type][p].Count);
            try
            {
                ret.Add(body_parts[gender][type][p][random]);
            }
            catch{}
        }
        return ret;
    }
    public List<Body_part> GetRandomPerson(SpriteGender gender)
    {
        Skin_type skin = (Skin_type)InGameData.random_int(0, 9);
        return GetPerson(gender, skin);
    }

    public game_item GetItemFrom(SpriteGender gender, item_type type, item_rarity.rarity rarity)
    {
        int chance = 0;
        game_item ret = null;
        foreach(game_item item in Game_Items[gender][type].Values)
        {
            if(item.rarity.Rarity == rarity)
                {
                    int temp = InGameData.random_int(0,100);
                    if(temp >= chance)
                    {
                        chance = temp;
                        ret = item;
                    }
                }
        }
        if(ret == null)
            Debug.Log(" No Items in " + gender + " ," + type + " with rarity " + rarity);
        return ret;
    }
    public game_item RandomItem(SpriteGender gender)
    {
        item_type t = item_type.consumable;
        int chance = 0;
        foreach(item_type type in Enum.GetValues(typeof(item_type)))
        {
            try
            {
            if(Game_Items[gender][type].Count <= 0)
                continue;
            
            
            int temp = InGameData.random_int(0, 100);
            if(temp > chance)
            {
                chance = temp;
                t = type;
            }
            }
            catch
            {

            }
        }
        int rand = InGameData.random_int(0, Game_Items[gender][t].Count - 1);

        return Game_Items[gender][t][rand].CopyItem();
    }

    public game_item RandomItem(SpriteGender gender, item_rarity.rarity rarity)
    {
        item_type t = item_type.consumable;
        int chance = 0;
        foreach(item_type type in Enum.GetValues(typeof(item_type)))
        {
            try
            {
            if(Game_Items[gender][type].Count <= 0)
                continue;
            
            
            int temp = InGameData.random_int(0, 100);
            if(temp > chance)
            {
                chance = temp;
                t = type;
            }
            }
            catch
            {

            }
        }

        return GetItemFrom(gender, t, rarity).CopyItem();
    }
    public List<game_item> GetRandomGear(SpriteGender gender)
    {
        List<game_item> ret = new List<game_item>();

        float Weapon = 0;
        item_type weap = item_type.none;
        foreach(item_type type in Enum.GetValues(typeof(item_type)))
        {
            if((int)type <= 0)
                continue;
            try
            {
                if (type == item_type.melee_weapons || type == item_type.spears || type == item_type.wand || type == item_type.bow)
                {
                    float t = InGameData.rand(0, 100);
                    if (t > Weapon)
                    {
                        Weapon = t;
                        weap = type;
                    }
                    continue;
                }
                else if (type == item_type.shilds || type == item_type.arrows)
                    continue;

                int temp = Game_Items[gender][type].Count;
                int rand = InGameData.random_int(0, (int)(temp * 1.5f));
                if (rand > temp)
                    continue;


                ret.Add(Game_Items[gender][type][rand].CopyItem());
            }
            catch
            {
                
            }
        }

        int temp2 = Game_Items[gender][weap].Count;
        int rand2 = InGameData.random_int(0, temp2);
        ret.Add(Game_Items[gender][weap][rand2].CopyItem());
        if (weap == item_type.bow)
        {
            ret.Add(Game_Items[gender][item_type.arrows][0].CopyItem());
        }
        else if (weap == item_type.melee_weapons || weap == item_type.spears)
        {
            temp2 = Game_Items[gender][item_type.shilds].Count;
            rand2 = InGameData.random_int(0, temp2 * 2);
            if (rand2 > temp2)
                return ret;

            ret.Add(Game_Items[gender][weap][rand2].CopyItem());
        }

        return ret;
    }

    public List<game_item> GetRandomGear(SpriteGender gender, item_rarity.rarity rarity)
    {
        List<game_item> ret = new List<game_item>();

        float Weapon = 0;
        item_type weap = item_type.none;
        foreach(item_type type in Enum.GetValues(typeof(item_type)))
        {
            if((int)type <= 0)
                continue;
            try
            {
                if (type == item_type.melee_weapons || type == item_type.spears || type == item_type.wand || type == item_type.bow)
                {
                    float t = InGameData.rand(0, 100);
                    if (t > Weapon)
                    {
                        Weapon = t;
                        weap = type;
                    }
                    continue;
                }
                else if (type == item_type.shilds || type == item_type.arrows)
                    continue;

                if (InGameData.random_int(0, 32) % 8 == 3)
                    continue;

                game_item ti = GetItemFrom(gender, type, rarity);
                if(ti != null)
                    ret.Add(GetItemFrom(gender, type, rarity).CopyItem());
            }
            catch
            {
                
            }
        }

        int temp2 = Game_Items[gender][weap].Count;
        int rand2 = InGameData.random_int(0, temp2);
        ret.Add(GetItemFrom(gender, weap, rarity).CopyItem());
        if (weap == item_type.bow)
        {
            ret.Add(GetItemFrom(gender, item_type.arrows, rarity).CopyItem());
        }
        else if (weap == item_type.melee_weapons || weap == item_type.spears)
        {
            if (InGameData.random_int(0, 32) % 8 == 3)
                return ret;

            ret.Add(GetItemFrom(gender, item_type.shilds, rarity).CopyItem());
        }

        return ret;
    }
    private Dictionary<string, int[]> ct_to_id = new Dictionary<string, int[]>();
    private List<SpriteData> lIcons = new List<SpriteData>();
    public Dictionary<int,Sprite> item_icons = new Dictionary<int, Sprite>();

    public IEnumerator load_icons()
    {
        int i = 0;

        foreach(SpriteData sd in lIcons)
        {
            Sprite ret = LoadIcon(sd.path, sd.id);
            item_icons.Add(sd.id,ret);
            i++;
            if (i % 5 == 0)
                yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private Sprite LoadIcon(string path, int id)
    {
        //WWW w = new WWW("file://" + Application.persistentDataPath + InGameData.Sprite_path + path + ".png");
        Texture2D source = LoadExternalAsSprite.GetTexture(id);

        if (!path.Contains("oversize"))
        {
            int c = 6;
            int r = 0;
            if (path.Contains("bow"))
            {
                c = 3;
                r = 6;
                return Sprite.Create(source, new Rect(64 * r, c * 64, 64, 64), new Vector2(0.5f, 0.5f));
            }
            else if(path.Contains("arrows"))
            {
                c = 3;
                r = 2;
                return Sprite.Create(source, new Rect(64 * r, c * 64, 64 * 0.75f, 64 * 0.75f), new Vector2(0.5f, 0.5f));
            }else if(path.Contains("spear"))
            {
                c = 11;
                r = 1;
                return Sprite.Create(source, new Rect(64 * r + 5, c * 64 + 5, 64, 64 * 0.66f), new Vector2(0.5f, 0.5f));
            }
            else if(path.Contains("feet"))
            {
                return Sprite.Create(source, new Rect(64 * r + 5, c * 64 - 5, 64 * 0.85f, 64 * 0.55f), new Vector2(0.5f, 0.5f));
            }
            else if(path.Contains("wings"))
            {
                c = 8;
                r = 0;
                return Sprite.Create(source, new Rect(64 * r + 5, c * 64 + 3, 64 * 0.8f, 64 * 0.8f), new Vector2(0.5f, 0.5f));
            }
            else if(path.Contains("dagger") || path.Contains("wand"))
            {
                c = 6;
                r = 2;
                return Sprite.Create(source, new Rect(64 * r + 10, c * 64 + 5, 64 * 0.55f, 64 * 0.55f), new Vector2(0.5f, 0.5f));
            }
            

            return Sprite.Create(source, new Rect(64 * r + 10, c * 64 + 5, 64 * 0.65f, 64 * 0.65f), new Vector2(0.5f, 0.5f));
        }
        else
        {
            int c = 1;
            int r = 2;
            if (path.Contains("spear"))
            {
                c = 2;
                r = 0;
                return Sprite.Create(source, new Rect(r * 192, c * 192 + 50, 162, 192 / 3), new Vector2(0.5f, 0.5f));
            }
            return Sprite.Create(source, new Rect(r * 192 + 50, c * 192 + 50, 192 / 3, 192 / 3), new Vector2(0.5f, 0.5f));
        }
    }

    public int[] GetMinMaxFromCatergory(string catergory)
    {
        return ct_to_id[catergory];
    }

    public int getRandomFromCatergory(string catergory)
    {
        int[] ret = ct_to_id[catergory];
        return InGameData.random_int(ret[0], ret[1]);     
    }

    public void CreateStatEnchanter(Basic_Stats stats, UseBoost pstats, string name, Sprite sprite)
    {
        game_item NEW_item = new game_item(item_type.consumable, -1, SpriteGender.none,
            -1, name, stats, pstats, item_rarity.rarity.simple, false);
        Game_Items[SpriteGender.none][item_type.consumable].Add(NEW_item.Order_ID, NEW_item);
        item_icons.Add(NEW_item.id_sprite, sprite);
    }

    public void CreateStatEnchanter(float agility, float endurance, float strength, float intelligence,
                                    float mobility, float health, float hregen,float healing, float magical_dmg,
                                    float evashion, float armor, float range, float physical_dmg,
                                    string name, Sprite sprite)
    {
        Basic_Stats stats = new Basic_Stats();
        stats.agility = agility;
        stats.endurance = endurance;
        stats.strength = strength;

        stats.intelligence = intelligence;

        UseBoost pstats = new UseBoost(armor, healing, health, hregen, evashion, physical_dmg, magical_dmg, range, mobility);
        
        game_item NEW_item = null;
        try
        {
            NEW_item = new game_item(item_type.consumable, -1, SpriteGender.none,
                -1, name, stats, pstats, item_rarity.rarity.simple, false);
        }
        catch
        {
            Debug.Log("(CreateStatEnchanter) : error creating item");
        }
        try
        {
            Game_Items[SpriteGender.none][item_type.consumable].Add(NEW_item.Order_ID, NEW_item);
        }
        catch
        {
            Debug.Log("(CreateStatEnchanter) : Error in storing item");
        }
        if(NEW_item != null)
            item_icons.Add(NEW_item.id_sprite, sprite);

    }

    private void CreateItems(SpriteData sd)
    {
        bool IsArmor = (sd.path.Contains("armor")) ? true : false;
        bool IsWeapon = (sd.path.Contains("weapons")) ? true : false;
        int MaxValue = (IsArmor) ? 14 : IsArmor ? 20 : 6;


        if (sd.path.Contains("walk_melee_weapons"))
        {
            walk_anim = new game_item(item_type.none, sd.id, sd.sg,
                0, sd.name, Basic_Stats.zero, UseBoost.zero, item_rarity.rarity.normal, false);
            return;
        }
        foreach(item_type type in Enum.GetValues(typeof(item_type)))
        {
            if(sd.key.Contains(type.ToString()))
            {
                foreach (item_rarity.rarity rare in Enum.GetValues(typeof(item_rarity.rarity)))
                {
                    if (!IsWeapon && !IsArmor && rare != item_rarity.rarity.normal)
                        continue;
                        
                    Basic_Stats stats = new Basic_Stats();
                    UseBoost pstats = UseBoost.zero;
                    stats.agility = InGameData.rand(0, MaxValue);
                    stats.endurance = InGameData.rand(0, MaxValue);
                    stats.strength = InGameData.rand(0, MaxValue);


                    if (sd.path.Contains("small_range"))
                        pstats.range = 2f;
                    else if (sd.path.Contains("normal_range"))
                        pstats.range = 10f;
                    else if (sd.path.Contains("large_range"))
                        pstats.range = 15f;
                    else if (type == item_type.wand)
                        pstats.range = 75.0f;
                    else if (type == item_type.bow)
                        pstats.range = 75.0f;                
                    else
                        pstats.range = 0;
                    stats.intelligence = InGameData.rand(0, MaxValue);
                    pstats.walk_speed = InGameData.rand(0, MaxValue);
                    game_item NEW_item = new game_item(type, sd.id, sd.sg, 
                        Game_Items[sd.sg][type].Count, sd.name, stats, pstats, rare, IsArmor);
                    if(sd.sg == SpriteGender.none)
                    {
                        Game_Items[SpriteGender.male][type].Add(NEW_item.Order_ID, NEW_item);
                        Game_Items[SpriteGender.female][type].Add(NEW_item.Order_ID, NEW_item);
                    }
                    Game_Items[sd.sg][type].Add(NEW_item.Order_ID, NEW_item);

                }
                break;
            }
        }
    }

    public void ADDITEMS()
    {
        foreach(SpriteGender g in Enum.GetValues(typeof(SpriteGender)))
        {
            body_parts.Add(g, new Dictionary<Skin_type, Dictionary<body_part , List<Body_part>>>());
            foreach(Skin_type skin in Enum.GetValues(typeof(Skin_type)))
            {
                body_parts[g].Add(skin, new Dictionary<body_part, List<Body_part>>());
                foreach(body_part p in Enum.GetValues(typeof(body_part)))
                {
                    body_parts[g][skin].Add(p, new List<Body_part>());
                }
            }

            Game_Items.Add(g, new Dictionary<item_type, Dictionary<int, game_item>>());
            foreach(item_type type in Enum.GetValues(typeof(item_type)))
            {
                Game_Items[g].Add(type, new Dictionary<int, game_item>());
            }
        }

        foreach (string sdl in DataBase.Sprite_DataBase.Data.Keys)
        {        
            foreach (SpriteData sd in DataBase.Sprite_DataBase.Data[sdl])
            {
                string pp = "";
                try
                {
                    pp = sd.path.Split(new string[] { "sub_", "\\" }, StringSplitOptions.None)[2];
                }
                catch
                {
                    pp = sd.path.Split(new string[] { "sub_", "/" }, StringSplitOptions.None)[2];
                }
                if (pp.Contains("body"))
                {
                    foreach(Skin_type s in Enum.GetValues(typeof(Skin_type)))
                    {
                        bool exception = sd.key.Contains(body_part.hair.ToString());

                        if (sd.name.Contains(s.ToString()) || exception)
                        {
                            foreach(body_part p in Enum.GetValues(typeof(body_part)))
                            {
                                if (sd.key.Contains(p.ToString()))
                                {

                                    if (p == body_part.hair)
                                        if (s != Skin_type.red_orc)
                                        {
                                            for (float r = 0; r < 1f; r += 0.2f)
                                                for (float g = 0; g < 1f; g += 0.2f)
                                                    for (float b = 0; b < 1f; b += 0.2f)
                                                    {
                                                        Body_part hair = new Body_part();
                                                        hair.id = sd.id;
                                                        hair.gender = sd.sg;
                                                        hair.skin = s;
                                                        hair.type = p;
                                                        hair.color = new Color(r, g, b);
                                                        hair.InListID = body_parts[sd.sg][s][p].Count;

                                                        body_parts[sd.sg][s][p].Add(hair);
                                                    }
                                            break;
                                        }
                                        else
                                            break;
                                    Body_part part = new Body_part();
                                    part.id = sd.id;
                                    part.gender = sd.sg;
                                    part.skin = s;
                                    part.type = p;
                                    part.InListID = body_parts[sd.sg][s][p].Count;

                                    body_parts[sd.sg][s][p].Add(part);

                                    break;
                                }
                                
                            }
                            if (!exception)
                                break;
                        }
                    }

                    continue;
                }

                CreateItems(sd);
                lIcons.Add(sd);
            }         
        }

        
        
    }

    public static weapon_type GetWeaponType(string key, string path)
    {
        weapon_type ret = weapon_type.none;
        if (key.Contains("melee"))
        {
            if (path.Contains("spear"))
            {
                ret = weapon_type.spear;
            }
            else
            {
                ret = weapon_type.sword;
            }

            if (path.Contains("oversize"))
                ret += 1;
        }
        else if (key.Contains("wand"))
            ret = weapon_type.wand;
        return ret;
    }
}


 public class ItemColor {
    private static Hashtable hueColourValues = new Hashtable{
         { item_color.white,     new Color32( 255 , 255 , 255, 255 ) },
         { item_color.green,     new Color32( 0 , 255 , 0, 255 ) },
         { item_color.blue,     new Color32( 0 , 0 , 255, 255 ) },
         { item_color.red,     new Color32( 255 , 0 , 0, 255 ) },
         { item_color.bronze,     new Color32( 150, 85, 20, 255 ) },
         { item_color.normal,     new Color32( 255, 255, 255, 255 ) },
         { item_color.silver,     new Color32( 199, 199, 199, 255 ) },
         { item_color.gold,     new Color32( 222, 200, 0, 255 ) },
         { item_color.diamond,     new Color32( 70, 130, 220, 255 ) },
     };

    private static Hashtable test2 = new Hashtable{
        { "body_parts" , ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => e == item_color.white)
                        .Select (e => (item_color)e)
                        .ToArray()
                },
        { "food" , ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => e == item_color.white)
                        .Select (e => (item_color)e)
                        .ToArray()
                },
        { "hair", ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => (e < item_color.rare))
                        .Select (e => (item_color)e)
                        .ToArray()
                },
        { "clothes", ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => (e < item_color.rare))
                        .Select (e => (item_color)e)
                        .ToArray()
            },
        { "armor", ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => (e > item_color.rare))
                        .Select (e => (item_color)e)
                        .ToArray()
            },
        { "weapons", ((item_color[])Enum.GetValues(typeof(item_color)))
                        .Where (e => (e > item_color.rare))
                        .Select (e => (item_color)e)
                        .ToArray()
            },
     };

    public static Color32 HueColourValue(item_color color ) {
        try
        {
            return (Color32)hueColourValues[color];
        }
        catch
        {
            Debug.Log(color);
            return default(Color32);
        }
     }
 
    public static item_color[] GetAllTypes(string key)
    {
        return (item_color[])test2[key];       

        //return (item_color[])Enum.GetValues(typeof(item_color));
    }
 }

public class Body_part
{
    public body_part type;
    public int id;
    public SpriteGender gender;
    public Skin_type skin;

    public Color color = Color.white;

    public int InListID = 0;
}

public enum Skin_type
{
    red_orc,
    darkelf2,
    darkelf,
    dark2,
    dark,
    light,
    tanned2,
    tanned,
    skeleton, 
}

public enum body_part
{
    body,
    ears,
    nose,
    hair
}

public enum item_color
{
    white,
    red,
    blue,
    green,

    rare,
    bronze,
    normal,
    silver,
    gold,
    diamond,
};


public class item_rarity
{
    public item_rarity(Basic_Stats st, rarity _rarity, int item_level)
    {
        this.st = st;
        nl = 0;
        tl = 1;
        Rarity = _rarity;
        
    }

    Basic_Stats st;
    private int tl;
    private int nl;
    public int Item_Level
    {
        get { return nl; }

        set
        {
            if(value > 0 && value <= 10)
                {
                    ChangeLevel(value);
                    nl = value;
                }
            
        }
    }

    public enum rarity
    {
        simple = 20,
        normal = 40,
        epic = 70,
        legendery = 90,
    }
    private void ChangeRarity(rarity _rarity)
    {
        float t = (int)_rarity + nl;
        st.multiply_all_by = 1 / ((float)tl * 0.01f + 1);
        st.multiply_all_by = (t * 0.01f + 1);
        tl = (int)t;
    }

    private void ChangeLevel(int new_v)
    {
        float t = (int)rar + new_v;
        st.multiply_all_by = 1 / ((float)tl * 0.08f + 1);
        st.multiply_all_by = (t * 0.08f + 1);
        tl = (int)t;
    }
    private rarity rar;
    public rarity Rarity
    {
        get { return rar; }
        set
        {
            switch(value)
            {
                case rarity.simple:
                    color = new Color(0.5f, 0.35f, 0.3f);
                    break;
                case rarity.normal:
                    color = Color.white;
                    break;
                case rarity.epic:
                    color = new Color(0.3f, 0.65f, 0.9f);
                    break;
                case rarity.legendery:
                    color = new Color(0.3f, 0.9f, 0.3f);
                    break;
            }
            ChangeRarity(value);
            rar = value;
        }
    }
    public Color color;
    
}