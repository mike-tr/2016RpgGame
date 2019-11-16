using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LoadExternal : MonoBehaviour {

    public static LoadExternal main;
    public float scale = 1f;
    public static bool RunOnWindows = false;

    public GUIStyle style;

    //Data_sprites DataMale = new Data_sprites();
    //Data_sprites DataFemale = new Data_sprites();
    //Data_sprites DataBoth = new Data_sprites();
    Data_sprites DataSprites = new Data_sprites();
    void Start()
    {
        Application.runInBackground = true;
        RunOnWindows = IsWindowsPlatform();
        //DataBase.LoadSystem();
        reLoadData();
        main = this;
    }


    public void reLoadData()
    {
        DataBase.LoadSystem();
        DataBase.Sprite_DataBase = ReadJson<Data_sprites>.GetObjectFromPath(DataBase.SpriteDataPath);
    }

    private bool IsWindowsPlatform()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return true;
            default:
                return false;
        }
    }

    public void reWriteData()
    {
        ChatGUI.addLine("Searching Data Trying To Create Json");
        Stick(Application.persistentDataPath + InGameData.Sprite_path + "Sprites", 0);
        WriteJson<Data_sprites>.SaveJson(DataSprites, DataBase.SpriteDataPath);
        ChatGUI.addLine("Json Was Created! - GameReloaded");
        reLoadData();
    }

    /// <summary>
    /// /////////////////////////////////////
    /// </summary>
    int id = 0;
    void Stick(string path, int _id = 0)
    {
        id = _id;
        
        var files = Directory.GetFiles(path);

        
        //bool tt = false;
        foreach (var fileName in files)
        {
            if (fileName.Contains(".png") && !fileName.Contains(".meta"))
            {
                string pathSpl = fileName.Split(new string[] { InGameData.Sprite_path, ".png" }, StringSplitOptions.None)[1];

                string[] temp = fileName.Split(new string[] { "/", "\\", ".png" }, StringSplitOptions.None);
                string sprite_name = temp[temp.Length - 2];

                int ii = 0;
                foreach (string s in temp)
                {
                    ii++;
                    if (s.Contains("Sprite"))
                        break;
                }
                string FolderKey = temp[ii];


                if (FolderKey.Contains("sub_"))
                    FolderKey = temp[ii + 1];

                SpriteGender gender = SpriteGender.none;

                if (pathSpl.Contains("female"))
                {
                    gender = SpriteGender.female;
                }
                else if (pathSpl.Contains("male"))
                {
                    gender = SpriteGender.male;
                }

                //kek += gender + " // " + FolderKey + " // " + sprite_name + "\n";

                DataSprites.AddKey(FolderKey);
                DataSprites.AddItem(FolderKey, sprite_name, pathSpl, gender);
            }
        }

        var subDirs = Directory.GetDirectories(path);
        foreach (var subDir in subDirs)
        {

            Stick(subDir,id+1);
        }
    }  
}

