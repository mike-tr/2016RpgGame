using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_draw : MonoBehaviour {

    private Body_part p;
    public Body_part part
    {
        set
        {
            if (p == value)
                return;

            p = value;
            StartCoroutine(ReLoad());
        }
    }

    public SpriteGender gender
    {
        set
        {
            if(value != p.gender)
            {
                part = ITEMS.main.body_parts[gender][skin][type][p.InListID];
            }
        }
        get
        {
            return p.gender;
        }
    }

    public Skin_type skin
    {
        set
        {
            if (value != p.skin)
            {
                part = ITEMS.main.body_parts[gender][skin][type][p.InListID];
            }
        }
        get
        {
            return p.skin;
        }
    }

    public body_part type
    {
        get
        {
            return p.type;
        }
    }

    public Texture2D source;
    public GameObject spritesRoot;

    private Sprite[,] sprite;
    private SpriteRenderer sr;
    private AddSpriteN Parent;

    private bool Draw = false;

    private LoadExternalAsSprite LES;

    public int SortingOrder
    {
        get
        {
            return sr.sortingOrder;
        }
        set
        {
            sr.sortingOrder = value;
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "all";
    }

    public void LoadSR()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "all";
    }

    private bool LoadSprites(int id)
    {
        LES = DataBase.LoadedSpritesDataBase;
        if (LES == null)
            return false;

        Parent = transform.parent.GetComponent<AddSpriteN>();
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "all";
        StartCoroutine(getTexture(id));

        Destroy(source);

        return true;
    }

    IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        sr.color = p.color;
        LoadSprites(p.id);
    }

    IEnumerator getTexture(int id)
    {
        Draw = false;
        Sprite[,] Load = null;
        while (Load == null)
        {
            Load = LES.GetSprite(id);
            yield return new WaitForSeconds(Time.deltaTime * 10);
        }
        sprite = Load;
        Draw = true;
    }

    public Color color
    {
        get
        {
            return sr.color;
        }
        set
        {
            if (sr != null)
                sr.color = value;
        }
    }


    public void UpdateAnimation()
    {
        if (Draw)
            sr.sprite = sprite[Parent.ImageIndex, 20 - Parent.SpriteColumn];
    }
}
