using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum UiType
{
    None,
    Male,
    Female,
    MandF
}

public class GameOptions
{
    
    public static string Button_clicked;
    private static int Items = -1;
    private static int Reset = 0;
    

    public float SpriteRowPixel = 64;
    public float SpriteColumnPixel = 64;

    public static void AddButtonItem()
    {
        Items += 1;
    }

    public static bool ResDone;
    public static bool Res_button
    {
        get
        {
            if (Button_clicked != "")
            {
                if (Reset > Items)
                {
                    Reset = 0;
                    Button_clicked = "";
                    ResDone = true;
                    return true;
                }
                Reset += 1;
                return false;
            }
            return true;
        }
    }
}


class DataBase
{
    public static LoadExternalAsSprite LoadedSpritesDataBase;
    public static Data_sprites Sprite_DataBase = new Data_sprites();

    private static void Loads()
    {

    }

    //public static Data_sprites SpriteMale = new Data_sprites();
    //public static Data_sprites SpriteFemale = new Data_sprites();
    //public static Data_sprites SpriteBoth = new Data_sprites();
    private static bool SysLoad = false;

    //public static string PathMale = "/Male_Sprites.xml";
    //public static string PathFemale = "/Female_Sprites.xml";
    //public static string PathNone = "/None_Sprite.xml";
    public static string SpriteDataPath = "/Sprites.xml";

    public static void LoadSystem()
    {
        if (SysLoad)
            return;

        //foreach (SpriteGender sg in Enum.GetValues(typeof(SpriteGender)))
          //  Sprite_DataBase.Add(sg, new Data_sprites());

        if (Application.platform == RuntimePlatform.Android)
        {
            InGameData.IsAndroid = true;
        }

        if (!InGameData.IsAndroid)
        {
            //DataBase.PathFemale = Application.dataPath + "/Resources" + DataBase.PathFemale;
            //DataBase.PathMale = Application.dataPath + "/Resources" + DataBase.PathMale;
            //DataBase.PathNone = Application.dataPath + "/Resources" + DataBase.PathNone;
            DataBase.SpriteDataPath = Application.dataPath + "/Resources" + DataBase.SpriteDataPath;
        }
        else
        {
            //DataBase.PathFemale = Application.persistentDataPath + DataBase.PathFemale;
            //DataBase.PathMale = Application.persistentDataPath + DataBase.PathMale;
            //DataBase.PathNone = Application.persistentDataPath + DataBase.PathNone;
            DataBase.SpriteDataPath = Application.persistentDataPath + DataBase.SpriteDataPath;
        }

        if (Application.isEditor && !InGameData.IsAndroid)
        {
            InGameData.Sprite_path = "/Editor" + InGameData.Sprite_path;
        }

        SysLoad = true;
    }

    public static void AddSpritePath(SpriteGender sg, string path, string file_name, string key)
    {
        AddKeys(sg, key);
        //Debug.Log(file_name);

        foreach (SpriteData s in Sprite_DataBase.Data[key])
            if (s.path == path)
                return;
        Sprite_DataBase.AddItem(key, file_name, path, sg);
    }

    public static void AddKeys(SpriteGender sg, string name)
    {
        if (!Sprite_DataBase.Data.ContainsKey(name))
        {
            Sprite_DataBase.AddKey(name);
        }
    }
}

