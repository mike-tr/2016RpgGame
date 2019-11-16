using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(LoadExternal))]
public class LoadExternalAsSprite : MonoBehaviour
{
    static LoadExternalAsSprite ExternalLoad;

    public bool LoadAll = false;
    private Dictionary<int, Sprite[,]> AllSprites = new Dictionary<int, Sprite[,]>();
    public Dictionary<int, string> Id_ToPath = new Dictionary<int, string>();
    public Dictionary<int, Texture2D> allTextures = new Dictionary<int, Texture2D>();
    public bool Load2op = false;

    public bool LoadAllBodyParts = true;

    private int[] JumpId;

    public int ThreadCount_visual_editor = 1;
    /// ////////////////////////////
    /// 
    public int maxT = 5;
    private int trC = 1;
    public int ThreadCount
    {
        get
        {
            return trC;
        }
        set
        {
            trC = Mathf.Clamp(value, 1, maxT);
            for(int i = 0; i < JumpId.Length; i++)
            {
                if (i < trC)
                    JumpId[i] = trC;
                else
                    JumpId[i] = 0;
            }

        }
    }
    /// //////////////////////////////
    /// 
    public int LoadSpeed_visual_editor;
    /// /////////////////////////
    private int loadSpeed = 6;
    public int LoadSpeed
    {
        get
        {
            return loadSpeed;
        }
        set
        {
            loadSpeed = Mathf.Clamp(value, 1, 30);
        }
    }
    /// /////////////////////////

    public Sprite[,] GetSprite(int id)
    {
        try
        {
            return AllSprites[id];
        }
        catch
        {
            foreach(ExternalS_loadData el in Sprite_ld)
            {
                if (el.id == id)
                {
                    return null;
                }
            }
            LoadSprites(id);
            return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        print("ExternalLoad - " + (ExternalLoad == this));
        ExternalLoad = this;

        print("ExternalLoad - " + (ExternalLoad == this));
        if (LoadAll)
            LoadAllSprites(SpriteGender.male);
        if (LoadAllBodyParts)
            LoadBodyParts(SpriteGender.male);
        DataBase.LoadedSpritesDataBase = this;

        CreateIDtP();


        
        JumpId = new int[maxT];
        for (int i = 0; i < maxT; i++)
        {
            StartCoroutine(LoadSprites_delayed(i));
            JumpId[i] = 0;
        }
        print(ThreadCount_visual_editor);
        ThreadCount = 1;
        
        //StartCoroutine(LoadSprites_delayed(1));
        //StartCoroutine(LoadSprites_delayed(2));
        //StartCoroutine(LoadSprites_delayed(3));
        //StartCoroutine(LoadSprites_delayed(4));

        StartCoroutine(DelOrder());



    }

    static public Texture2D GetTexture(int id)
    {
        Texture2D source;
        if(ExternalLoad.allTextures.TryGetValue(id,out source))
        {
            source = null;
            return ExternalLoad.allTextures[id];
        }
        string file = (LoadExternal.RunOnWindows) ? "file:///" : "file://";
        WWW w = new WWW(file + Application.persistentDataPath + ExternalLoad.Id_ToPath[id]);
        source = new Texture2D(2048, 2048, TextureFormat.DXT1Crunched, false);
        w.LoadImageIntoTexture(source);
        source.Compress(false);
        ExternalLoad.allTextures.Add(id, source);
        source = null;
        return ExternalLoad.allTextures[id];
    }

    public static void ReloadDataBase()
    {

        ExternalLoad.CreateIDtP();
    }

    void CreateIDtP()
    {
        Id_ToPath = new Dictionary<int, string>();
        foreach (List<SpriteData> sdl in DataBase.Sprite_DataBase.Data.Values)
            foreach (SpriteData sd in sdl)
            {                    
                Id_ToPath.Add(sd.id, InGameData.Sprite_path + sd.path + ".png");
            }
        //yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        

        if (!Application.isEditor)
            return;

        if (LoadSpeed != LoadSpeed_visual_editor)
        {
            LoadSpeed = LoadSpeed_visual_editor;
            LoadSpeed_visual_editor = LoadSpeed;
        }

        if(ThreadCount_visual_editor != ThreadCount)
        {
            ThreadCount = ThreadCount_visual_editor;
            ThreadCount_visual_editor = ThreadCount;
        }
    }

    private void LoadAllSprites(SpriteGender sg)
    {
        foreach (List<SpriteData> sdL in DataBase.Sprite_DataBase.Data.Values)
        {
            foreach (SpriteData sd in sdL)
            {
                if (sd.sg == sg)
                {
                    Sprite_ld.Add(new ExternalS_loadData(sd.id, InGameData.Sprite_path + sd.path + ".png"));
                }
                   

            }
        }
        DontDestroy.first.LoadAll();
    }

    private void LoadBodyParts(SpriteGender sg)
    {
        foreach (List<SpriteData> sdL in DataBase.Sprite_DataBase.Data.Values)
        {
            foreach (SpriteData sd in sdL)
            {
                if (sd.sg == sg && sd.path.Contains("body"))
                {
                    Sprite_ld.Add(new ExternalS_loadData(sd.id, InGameData.Sprite_path + sd.path + ".png"));
                }
            }
        }
        DontDestroy.first.LoadAll();
    }

    List<ExternalS_loadData> Sprite_ld = new List<ExternalS_loadData>();
    private bool Transparent(int xs, int ys, Texture2D tex)
    {
        for (int x = 0; x < 64; x++)
            for (int y = 0; y < 64; y++)
            {
                if (tex.GetPixel(xs + x, ys + y).a != 0)
                {
                    return false;
                }
            }
        return true;
    }

    private IEnumerator LoadSprites_delayed(int id)
    {
        //StartCoroutine(delay_c());      
        float time = Time.time;
        int k = 0;
        while (true)
        {  
            if(JumpId[id] > 0)
            {
                while (Sprite_ld.Count > id)
                {
                    ExternalS_loadData ed = Sprite_ld[id];
                    if (ed.empty)
                    {
                        if (Sprite_ld.Count > (id + ThreadCount))
                            ed = Sprite_ld[id + ThreadCount];
                        else
                            break;
                    }
                    ed.empty = true;


                    Sprite[,] nSprite = new Sprite[13, 21];

                    if (!ed.path.Contains("oversize"))
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            for (int j = 0; j < 21; j++, k++)
                            {
                                
                                if(Transparent(i * 64, j * 64, GetTexture(ed.id)))
                                {
                                    nSprite[i, j] = null;
                                    continue;
                                }

                                Sprite newSprite = Sprite.Create(GetTexture(ed.id), new Rect(i * 64, j * 64, 64, 64), new Vector2(0.5f, 0.5f));
                                nSprite[i, j] = newSprite;

                                if (k % LoadSpeed == 0)
                                    yield return null;
                            }
                        }
                    }
                    else
                    {
						int im = ed.path.Contains("spear") ? 8 : 6;
						for (int i = 0; i < 13; i++)
						{
							for (int j = 0; j < 21; j++)
							{
								if(i <  im)
                                    if (ed.path.Contains ("spear")) {
                                        if (j > 12 && j < 17) {
                                            Sprite newSprite = Sprite.Create(GetTexture(ed.id), new Rect(i * 192, (j - 13)  * 192, 192, 192), new Vector2(0.5f, 0.5f));
                                            nSprite [i, j] = newSprite;
                                            k++;
                                            continue;
                                        }
                                    } else {
                                        if (j > 4 && j < 9) {
                                            Sprite newSprite = Sprite.Create(GetTexture(ed.id), new Rect(i * 192, (j - 5) * 192, 192, 192), new Vector2(0.5f, 0.5f));
                                            nSprite [i, j] = newSprite;
                                            k++;
                                            continue;
                                        }
                                    }
								nSprite [i, j] = null;


							}
						}							                                              
                    }

                    try
                    {
                        AllSprites.Add(ed.id, nSprite);
                    }
                    catch
                    {
                        //print("ERROR~~!!");
                    }


                    if (k % LoadSpeed == 0)
                        yield return null;
                }
            }else
            {
                //print("Load Thread id = " + id + " Sleeping");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator DelOrder()
    {
        while(true)
        {
            lock(Sprite_ld)
            {
            for (int i = 0; i < Sprite_ld.Count; i++)
                if (Sprite_ld[i].empty)
                    Sprite_ld.RemoveAt(i);
            }
            if (Sprite_ld.Count > 0)
                print(Time.timeSinceLevelLoad + " , Still Loading Objects - " + Sprite_ld.Count);
            else
                DontDestroy.first.Loading = false;
            yield return new WaitForSeconds(1f);

        }
    }

    private void LoadSprites(int id)
    {
        string path = Id_ToPath[id];
        if (Load2op)
        {

            Sprite_ld.Add(new ExternalS_loadData(id, path));
            return;
        }
    }
}

public class ExternalS_loadData
{
    public int id;
    public string path;
    public bool empty = false;

    public ExternalS_loadData(int _id, string _path)
    {
        id = _id;
        path = _path;
        empty = false;
    }
}