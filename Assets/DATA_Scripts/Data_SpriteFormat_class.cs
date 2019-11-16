using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Data_sprites
{
    private Dictionary<string, List<SpriteData>> sprite = new Dictionary<string, List<SpriteData>>();
    private int id = 0;

    public Dictionary<string, List<SpriteData>> Data
    {
        get
        {
            return sprite;
        }
        set
        {
            sprite = value;
        }
    }
    /*
    public void AddItem(string key, SpriteData data)
    {
        List<SpriteData> temp2 = sprite[key];
        temp2.Add(data);
        sprite[key] = temp2;
    }*/

    public void OverWrite(Dictionary<string, List<SpriteData>> DATA)
    {
        sprite = DATA;
    }

    public void AddKey(string key)
    {
        try
        {
            sprite.Add(key, new List<SpriteData>());
        }
        catch
        {
            //Debug.Log(key + " already exist in dictionery");
        }
    }

    public void AddItem(string key, string sprite_name, string path, SpriteGender sg)
    {
        SpriteData temp = new SpriteData();
        temp.id = id;
        temp.key = key;
        temp.name = sprite_name;
        temp.path = path;
        temp.sg = sg;
        List<SpriteData> temp2 = sprite[key];
        temp2.Add(temp);
        sprite[key] = temp2;
        id++;
    }
}

public class SpriteData
{
    public int id;
    public string key;
    public string name;
    public string path;
    public SpriteGender sg;
}

public enum SpriteGender
{
    none,
    male,
    female
}
