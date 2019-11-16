using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteRect : MonoBehaviour {

    public Texture2D source;
    public GameObject spritesRoot;

    private Sprite[,] sprite;
    private SpriteRenderer sr;
    private AddSpriteN Parent;

    private bool Draw = false;

    private LoadExternalAsSprite LES;
    public int in_layer
    {
        get { return ml; }
        set
        {
            ml = value;
        }
    }

    private int ml = -1;
    public int SortingOrder
    {
        get
        {
            return sr.sortingOrder;
        }
        set
        {
            sr.sortingOrder = ml + value;
        }
    }

    // Use this for initialization
    bool loaded = false;
    IEnumerator Start()
    {
        while(DataBase.LoadedSpritesDataBase == null)
            yield return new WaitForSeconds(Time.deltaTime * 2);
            
        LES = DataBase.LoadedSpritesDataBase;
        Parent = transform.parent.GetComponent<AddSpriteN>();
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "all";
        loaded = true;
    }

    public void LoadSprites(int id, int _layer)
    {
        ml = _layer;
        StartCoroutine(getTexture(id));
    }

    IEnumerator getTexture(int id)
    {
        Draw = false;
        Sprite[,] Load = null;
        while(loaded == false)
            yield return new WaitForSeconds(Time.deltaTime * 3);
        while(Load == null)
        {
            Load = LES.GetSprite(id);
            yield return new WaitForSeconds(Time.deltaTime * 10);
        }

        Destroy(source);

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
            if(sr != null)
                sr.color = value;
            else
                {
                    StopCoroutine(SetColor(value));
                    StartCoroutine(SetColor(value));
                }
                
        }
    }


    IEnumerator SetColor(Color color)
    {
        while(sr == null)
            yield return null;
        sr.color = color;
    }

    Sprite set;
    public void UpdateAnimation()
    {
        if (Draw)
        {
            sr.sprite = sprite[Parent.ImageIndex, 20 - Parent.SpriteColumn];
        }
    }
}
