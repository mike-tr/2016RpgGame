using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour {
    // Use this for initialization
    public static AddItem MyPlayer;
    public AddSpriteN asn;
    public CharacterControl cc;

    public bool Dead
    {
        get { return cc.IsDead; }
    }

    public C_Stats stats;

    public Dictionary<item_type, game_item> worn_items =
        new Dictionary<item_type, game_item>();
    

    void Start () {
        stats = new C_Stats(GetComponent<Health>());
        asn = GetComponentInChildren<AddSpriteN>();
        cc = GetComponent<CharacterControl>();

        if (cc.IsMine)
        {
            stats_info.stats = stats;
            MyPlayer = this;
        }
	}
	
    public weapon_type CheckIfWeapon(item_type type)
    {
        switch(type)
        {
            case item_type.melee_weapons:
                return weapon_type.sword;
            case item_type.spears:
                return weapon_type.spear;
            case item_type.wand:
                return weapon_type.wand;
            case item_type.bow:
                return weapon_type.bow;
            default:
                return weapon_type.none;
        }
    }
	// Update is called once per frame
    public void IsDead()
    {
        foreach(item_type t in EnumCheck<item_type>.GetEnumValues())
        {
            game_item temp;
            if (worn_items.TryGetValue(t, out temp))
            {
                if(temp == null)
                    continue;
                if(Random.Range(0f,10f) >= 0)
                {
                    if(temp.slot_pointer != null)
                    {
                        temp.slot_pointer.DropItem( transform.position );
                    }
                    else
                    {   
                        ItemDrop.Global_ItemDrop.DropItem(temp.CopyItem() , transform.position);
                        Remove_Item(this.worn_items[t]); 
                    }  
                }
            }
            
        }
    }
	public void add_item (game_item item) {
        weapon_type type = CheckIfWeapon(item.type);

        game_item temp;
        item_type t = (type == weapon_type.none) ? item.type : item_type.melee_weapons;
        if (worn_items.TryGetValue(t, out temp))
        {
            if(temp != null)
            {
                stats.DecreseStats(temp.bstats);
                stats.DecresePStats(temp.pstats);
            }
            worn_items[t] = item;
        }
        else
            worn_items.Add(t, item);

        if (type != weapon_type.none)
            cc.weapon = type;
        asn.AddItem(item);
        stats.AddStats(item.bstats);
        stats.AddPStats(item.pstats);
        //items.Add(item);
	}

    public void Use_Consumable(game_item item)
    {
        stats.AddStats(item.bstats);
        stats.AddPStats(item.pstats);
    }

    public void Remove_Item(game_item item)
    {
        if (item.type == item_type.wand || item.type == item_type.spears
                || item.type == item_type.bow || item.type == item_type.melee_weapons)
        {
            asn.RemoveItem(item_type.melee_weapons);
            cc.weapon = weapon_type.none;
            this.worn_items[item_type.melee_weapons] = null;
        }
        else
        {
            asn.RemoveItem(item.type);
            this.worn_items[item.type] = null;
        }

        
        stats.DecreseStats(item.bstats);
        stats.DecresePStats(item.pstats);
    }
}

public class C_Stats
{
    public static float STAT_MIN = 8f;
    public static float ACT = 0.5f;
    private Health hp;
    public C_Stats(Health _hp)
    {
        stats = new Basic_Stats();
        pstats = new UseBoost( 0, 0, 4, 102, 0, 0, 0, 0, 0);
        hp = _hp;

        endurance = 8f;
        strength = 8f;
        intelligence = 8f;
        
        mobility = 130f;
            
        range = 12f;    
        agility = 8f;
    }

    public void AddPStats(UseBoost b)
    {
        pstats.AddBoost(b);
        pstats.healing = 0;
        walk_speed.sleep = (pstats.walk_speed * 0.01f + (agility * 0.001f)) * ACT;
        if(b.healing > 0)
            hp.GotHeal(pstats.healing);
        
    }

    public void DecresePStats(UseBoost b)
    {
        pstats.decBoost(b);
        pstats.healing = 0;
        walk_speed.sleep = (pstats.walk_speed * 0.01f + (agility * 0.001f)) * ACT;
    }
    public void AddStats(Basic_Stats stats)
    {
        agility += stats.agility;
        strength += stats.strength;
        intelligence += stats.intelligence;
        endurance += stats.endurance;
    }

    public void DecreseStats(Basic_Stats stats)
    {
        agility -= stats.agility;
        strength -= stats.strength;
        intelligence -= stats.intelligence;
        endurance -= stats.endurance;
    }
   
    public SpriteSpeed cast_speed = new SpriteSpeed();
    public SpriteSpeed Attack_speed = new SpriteSpeed();
    public SpriteSpeed Shooting_speed = new SpriteSpeed();
    public SpriteSpeed walk_speed = new SpriteSpeed();

    private float sr = 10f;
    public float spell_range { get { return sr + pstats.range; } }
    private float pd = 5f;
    public float physical_dmg { get { return pd + pstats.physical_dmg; } }
    private float md = 5f;
    public float magic_dmg { get { return md + pstats.magical_dmg; } }
    private float hl = 500;
    public float health { get { return hl + pstats.health; } }
    private float hregen;
    public float health_regen { get { return hregen + pstats.health_regen; } }
    public float evasion { get { return pstats.evasion; } }
    private Basic_Stats stats;
    private UseBoost pstats;
    private float ArmorT = 0;
    public float armor
    {
        get { return ArmorT + pstats.armor; }
    }


    public float endurance
    {
        set
        {
            stats.endurance = value;
            hl = (strength * 1.5f + STAT_MIN * 7.5f + value * 4.5f) * ACT;
            ArmorT = (value * 0.45f + STAT_MIN * 2.5f) * ACT; 
            hregen = value / 10 + strength / 15;           
            hp.HealthChange(hl + pstats.health);
        }
        get
        {
            return stats.endurance;
        }
    }

    public float strength
    {
        set
        {
            stats.strength = value;
            pd = (value * 0.45f + STAT_MIN * 0.85f) * ACT;
            hl = (value * 1.5f + STAT_MIN * 7.5f + endurance * 4.5f) * ACT;
            hregen = value / 10 + strength / 15;
            hp.HealthChange(health);
        }
        get 
        {
            return stats.strength;
        }
    }

    public float intelligence
    {
        set
        {
            stats.intelligence = value;
            md =  (value * 0.35f + STAT_MIN) * ACT;
            cast_speed.sleep = (agility * 0.015f + value * 0.003f + STAT_MIN * 0.05f) * ACT; 
        }
        get
        {
            return stats.intelligence;
        }
    }
    public float mobility
    {
        set
        {
            pstats.walk_speed = value;
            walk_speed.sleep = (value * 0.01f + (agility * 0.001f)) * ACT;
        }
        get
        {
            return pstats.walk_speed;
        }
    }

    public float range
    {
        set
        {
            pstats.range = value;
            sr = value * 2f + STAT_MIN * 0.2f;
        }
        get
        {
            return pstats.range;
        }
    }

    public float agility
    {
        set
        {
            stats.agility = value;
            Shooting_speed.sleep = (value * 0.02f + STAT_MIN * 0.05f) * ACT;
            Attack_speed.sleep = (value * 0.02f + STAT_MIN * 0.05f) * ACT;
            cast_speed.sleep = (value * 0.015f + intelligence * 0.003f + STAT_MIN * 0.05f) * ACT;
            walk_speed.sleep = (pstats.walk_speed * 0.01f + (value * 0.001f)) * ACT;
        }
        get
        {
            return stats.agility;
        }
    }
}

//the purpose of this IS to overcome the fps!
//fps is no longer the factor of how fast you attack move etc...(+- no longer)
public class SpriteSpeed
{
    private int mj;
    private float ss;
    public float speed_t
    {
        get { return ss; }
    }

    private float speed;

    public int index_jump;
    public float sleep
    {
        get
        {
            return speed;
        }
        set
        {
            ss = value;
            mj = (int)(value / 1.1f) + 1;
            speed = value;
        }
    }

    public SpriteSpeed Recalculate()
    {
        index_jump = Random.Range(1, mj);
        speed = ss / index_jump;
        return this;   
    }
}



public class Basic_Stats
{
    public float strength;
    public float agility;
    public float intelligence;
    public float endurance;

    public float multiply_all_by
    {
        set
        {
            if (value == 0)
                return;

            strength *= value;
            agility *= value;
            intelligence *= value;
            endurance *= value;
        }
    }

    public Basic_Stats() 
    {
        strength = 0;
        agility = 0;
        intelligence = 0;
        endurance = 0;
    }

    static public Basic_Stats zero
    {
        get
        {
            return new Basic_Stats();
        }
    }
    public Basic_Stats(float str, float agl, float intelligence, float end)
    {
        strength = str;
        agility = agl;
        this.intelligence = intelligence;
        this.endurance = end;
    }

    public Basic_Stats copy_stats()
    {
        return new Basic_Stats(strength, agility, intelligence, endurance);
    }
}
