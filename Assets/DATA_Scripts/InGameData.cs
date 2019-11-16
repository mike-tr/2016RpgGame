using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Linq;
using UnityEngine.UI;

public class InGameData
{
    public static List<Transform> ReUseBox = new List<Transform>();
    public static AddItem add_item;

    //static public float GameScale = 1f;
    static public float GameScale
    {
        get
        {
            float scale = Screen.width / 1200;
            return scale;
        }
    }

    static public float GameBlock = 64f;
    static public bool WriteJson = false;
    static public string Sprite_path = "/Textures/";
    static public bool IsAndroid = true;

    static public float ResWoriginal = 1280;
    static public float ResHoriginal = 720;


    public static SpriteGender GetSpriteGender(string path)
    {
        SpriteGender sg = SpriteGender.none;
        if (path.Contains("female"))
            sg = SpriteGender.female;
        else if (path.Contains("male"))
            sg = SpriteGender.male;
        return sg;         
    }

    public static int random_int(int min, int max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }

    public static float rand(float min, float max)
    {
        return Random.Range(min, max);
    }
}


 