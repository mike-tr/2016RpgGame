using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AddSpriteN : MonoBehaviour {

    Dictionary<string, SpriteRect> sprites = new Dictionary<string, SpriteRect>();
    Dictionary<body_part, Body_draw> Body_sprites = new Dictionary<body_part, Body_draw>();
    Dictionary<item_type, SpriteRect> Wearable_items = new Dictionary<item_type, SpriteRect>();

    
    private SpriteRect weapon_anim; 

    public GameObject SpriteContainer;
    public float BasicAnimationSpeed = 10f;
    public item_type[] Layer0;
    public CharacterControl hm;
    private CharacterAnimation action_s = CharacterAnimation.idle;
    public CharacterAnimation action
    {
        get
        {
            return action_s;
        }
        set
        {
            if (new_anim == value)
                return;
            new_anim = value;
            if(value == CharacterAnimation.Hurt || action_s == CharacterAnimation.Walk)
            {
                ImageIndex = 0;
                action_s = value;
                MaxRow = CharacterAnimations.GetAnimationLenght(action_s);

                if (action_s == CharacterAnimation.Slash || action_s == CharacterAnimation.Thrust)
                {
                    hm.Attacks.Add(skill_type.attack);
                }
            }
        }
    }
    public CharacterDirection direction
    {
        get
        {
            return hm.direction;
        }
    }

    private CharacterAnimation new_anim = CharacterAnimation.idle;

    private int MaxRow = 0;
    public int ImageIndex = 0;
    public int SpriteColumn = 0;

    int layers = 0;
    int hair_l = 0;

    SpriteSpeed ssp;
    CalcLayer layerC;
    // Use this for initialization
    IEnumerator Start()
    {
        layerC = new CalcLayer(transform.eulerAngles.z);
        ssp = new SpriteSpeed();
        ssp.sleep = 0.7f;
        ssp.index_jump = 1;
        SpriteContainer = (GameObject)Resources.Load("Prefabs/SpriteH");
        hm = transform.parent.GetComponent<CharacterControl>();
        MaxRow = CharacterAnimations.GetAnimationLenght(action_s);
        int i = 1;
        layers = Layer0.Length + 1;
        foreach (item_type s in Layer0)
        {
            if (s == item_type.shilds)
            {
                AddEmptyOrRemoveSprite(s, (i + 2));
                hair_l = i;
                i += 2;
            }
            else
                AddEmptyOrRemoveSprite(s, i);
            i++;
        }


        foreach(Body_part part in  ITEMS.main.GetRandomPerson(SpriteGender.male))
        {
            AddBodyPart(part);
        }

        weapon_anim = AnimLoad(ITEMS.main.walk_anim, hair_l + 3);

        enabled = false;
        yield return new WaitForSeconds(Time.deltaTime);
        enabled = true;
        StartCoroutine(ChangeImage());
    }

    public SpriteRect AnimLoad(game_item item, int _layer)
    {
        Transform child = Instantiate(SpriteContainer).transform;
        child.name = "weapon_anim_walk";
        SpriteRect sr = child.gameObject.AddComponent<SpriteRect>();

        child.SetParent(transform);
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;
        child.localScale = new Vector3(1, 1, 1);

        sr.LoadSprites(item.id_sprite, _layer);

        sr.in_layer = _layer;
        sr.color = new Color(1,1,1,0);

        return sr;
    }
    
    public void AddEmptyOrRemoveSprite(item_type type, int layer)
    {
        try
        {
            SpriteRect sr;
            if (type == item_type.wand || type == item_type.spears
                || type == item_type.bow)
            {
                sr = Wearable_items[item_type.melee_weapons];
            }
            else
                sr = Wearable_items[type];

            sr.color = new Color(1, 1, 1, 0);
        }
        catch
        {
            Transform child = Instantiate(SpriteContainer).transform;
            if (type == item_type.wand || type == item_type.melee_weapons
                || type == item_type.spears || type == item_type.bow)
                child.name = "(weapon slot)";
            else
                child.name = type.ToString();
            SpriteRect sr = child.gameObject.AddComponent<SpriteRect>();

            child.SetParent(transform);
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
            child.localScale = new Vector3(1, 1, 1);

            sr.color = new Color(1, 1, 1, 0);
            sr.in_layer = layer;

            if (type == item_type.wand || type == item_type.spears)
                Wearable_items.Add(item_type.melee_weapons, sr);
            else
                Wearable_items.Add(type, sr);

        }
    }

    public void AddItem(game_item item)
    {
        try
        {
            SpriteRect sr;
            if (item.type == item_type.wand || item.type == item_type.spears 
                || item.type == item_type.bow || item.type == item_type.melee_weapons)
            {
                sr = Wearable_items[item_type.melee_weapons];

                if (item.type == item_type.melee_weapons)
                {
                    sr.in_layer = hair_l + 3;
                    weapon_anim.color = Color.white;
                }
                else
                {
                    sr.in_layer = hair_l + 1;
                    weapon_anim.color = new Color(1,1,1,0);
                }
            }
            else
                sr = Wearable_items[item.type];


            
            sr.LoadSprites(item.id_sprite, sr.in_layer);
            sr.color = item.rarity.color;
        }
        catch
        {
            Transform child = Instantiate(SpriteContainer).transform;
            if (item.type == item_type.wand || item.type == item_type.melee_weapons 
                || item.type == item_type.spears || item.type == item_type.bow)
                child.name = "(weapon slot)";
            else
                child.name = item.type.ToString();
            SpriteRect sr = child.gameObject.AddComponent<SpriteRect>();

            child.SetParent(transform);
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
            child.localScale = new Vector3(1, 1, 1);

            sr.LoadSprites(item.id_sprite, layers); 
                    
            sr.in_layer = layer;
            sr.color = item.rarity.color;
            layers++;




            if (item.type == item_type.wand || item.type == item_type.spears)
                Wearable_items.Add(item_type.melee_weapons, sr);
            else
                Wearable_items.Add(item.type, sr);
            

        }
    }

    public void RemoveItem(item_type type)
    {
        try
        {
            SpriteRect sr;
            if (type == item_type.wand || type == item_type.spears 
                || type == item_type.bow || type == item_type.melee_weapons)
            {
                weapon_anim.color = new Color(1, 1, 1, 0);

                sr = Wearable_items[item_type.melee_weapons];
            }
            else
                sr = Wearable_items[type];

            sr.color = new Color(1, 1, 1, 0);

        }
        catch
        {
            
        }
    }

    public void AddBodyPart(Body_part part)
    {
        try
        {
            Body_draw sr = Body_sprites[part.type];
            sr.LoadSR();
        } 
        catch
        {
            Transform child = Instantiate(SpriteContainer).transform;
            child.name = part.type.ToString();
            Body_draw sr = child.gameObject.AddComponent<Body_draw>();
            sr.part = part;

            child.SetParent(transform);
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;
            child.localScale = new Vector3(1, 1, 1);

            Body_sprites.Add(part.type, sr);
            //sprites.Add(part.category, sr);
        }
    }

    int layer = 0;
    public void CheckAnimation()
    {
        //layer = -(((int)transform.position.y - 2) * 30) + ((int)transform.position.x / 3);
        layer = layerC.GetSortingLayer(transform.position);
        foreach(Body_draw bd in Body_sprites.Values)
        {
            if (bd.type == body_part.hair)
                bd.SortingOrder = layer + hair_l;
            else
                bd.SortingOrder = layer;
        }
        foreach (SpriteRect sr in Wearable_items.Values)
        {
            sr.SortingOrder = layer;
        }

        if(weapon_anim != null)
            weapon_anim.SortingOrder = layer;
        //transform.localPosition = new Vector3(-ImageIndex * 64 - 32, SpriteColumn * 64 + 32, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckAnimation();
    }

    SpriteSpeed AnimSpeed()
    {
        switch(action_s)
        {
            case CharacterAnimation.Shoot:
                return hm.Shooting_speed.Recalculate();
            case CharacterAnimation.Slash:
            case CharacterAnimation.Thrust:
                return hm.Attack_speed.Recalculate();
            case CharacterAnimation.SpellCast:
                return hm.cast_speed.Recalculate();
            case CharacterAnimation.Walk:
                return hm.walk_speed.Recalculate();
        }
        return ssp;
    }

    SpriteSpeed index_jump;

    void UpdateAnyway()
    {
        weapon_anim.UpdateAnimation();
        foreach( Body_draw bd in Body_sprites.Values)
        {
            bd.UpdateAnimation();
        }
        foreach (SpriteRect sr in Wearable_items.Values)
        {
            sr.UpdateAnimation();
        }  
    }
    IEnumerator ChangeImage()
    {
        while (true)
        {
            index_jump = AnimSpeed();
            yield return new WaitForSeconds(1 / (index_jump.sleep * BasicAnimationSpeed));
            ImageIndex+= index_jump.index_jump;
            if (ImageIndex >= MaxRow)
            {
                if(hm.IsDead)
                {
                    if(action != CharacterAnimation.Hurt)
                    {
                        ImageIndex = 0;
                        action_s = CharacterAnimation.Hurt;
                        MaxRow = CharacterAnimations.GetAnimationLenght(action_s);
                        UpdateAnyway();
                        continue;
                    }
                    else
                    {
                        ImageIndex--;
                        UpdateAnyway();
                        continue;
                    }
                }

                if (action_s == CharacterAnimation.Shoot)
                {
                    ImageIndex = 4 + (ImageIndex % (MaxRow - 4));
                }
                else if(action_s == CharacterAnimation.idle)
                    ImageIndex = 0;
                else
                    ImageIndex = ImageIndex - MaxRow;
                if(action_s == CharacterAnimation.SpellCast)
                {
                    hm.Attacks.Add(skill_type.spell);
                    new_anim = CharacterAnimation.idle;
                }
                if (new_anim != action_s)
                {
                    ImageIndex = 0;
                    action_s = new_anim;
                    MaxRow = CharacterAnimations.GetAnimationLenght(action_s);
                    if(action_s != CharacterAnimation.Hurt)
                        new_anim = CharacterAnimation.idle;
                  
                }
                if (action_s == CharacterAnimation.Slash || action_s == CharacterAnimation.Thrust)
                {
                    if(hm.weapon == weapon_type.wand)
                        hm.Attacks.Add(skill_type.spell);
                    else
                        hm.Attacks.Add(skill_type.attack);
                    
                }else if(action_s == CharacterAnimation.Shoot)
                {
                    hm.Attacks.Add(skill_type.arrow_shot);
                }
            }


            if (action_s != CharacterAnimation.idle)
            {
                SpriteColumn = ((int)action_s * 4) + (int)direction;
                if (SpriteColumn >= CharacterAnimations.MaxColumn - 1)
                    SpriteColumn = CharacterAnimations.MaxColumn - 1;
            }else
                SpriteColumn = ((int)CharacterAnimation.Walk * 4) + (int)direction;

            UpdateAnyway();
            //(call change image)
        }
    }
}
