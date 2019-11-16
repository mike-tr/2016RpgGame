using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson<T> {
    public static T GetObjectFromPath(string path)
    {
        try
        {
            LoadData Read = new LoadData(path, true, null);


            //string jsonString = File.ReadAllText(path);
            string jsonString = Read.Load();
            //Debug.Log(jsonString);
            return JsonMapper.ToObject<T>(jsonString);
        }
        catch
        {
            Debug.Log("Cannot load json probably wrong format(wrong json)! " + path);
        }
        return default(T);
    }

    public static T GetObject(string json)
    {
        try
        {
            return JsonMapper.ToObject<T>(json);
        }
        catch
        {
            Debug.Log("Cannot load json probably wrong format(wrong json)!");
        }
        return default(T);
    }

}

public class WriteJson<T>
{
    public static JsonData GetJsonData(T obj)
    {
        return JsonMapper.ToJson(obj);
    }

    public static bool SaveJson(T obj, string path = null)
    {
        try {
            JsonData jData = JsonMapper.ToJson(obj);
            File.WriteAllText(path, jData.ToString());

            //LoadData Save = new LoadData(path, true, null);
            //Save.Save(jData.ToString());

        }
        catch
        {
            Debug.Log("ERROR OBJECT WONT CONVERT TO JSON");
            return false;
        }
        return true;
    }

}

public class LoadData
{
    //string message;

    string _FileLocation = Application.persistentDataPath;
    string _FileName = "kek.xml";
    private bool in2 = false;

    public LoadData(string fileName, bool full_path, string fileLocation)
    {
        _FileName = fileName;
        _FileLocation = fileLocation;

        in2 = full_path;

    }

    public void Save(string data)
    {
        FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);
        if (in2)
            t = new FileInfo(_FileName);
        if (!t.Exists)
        {
            var _writer = t.CreateText();
            _writer.Write(data);
            _writer.Close();
        }
        else
        {
            t.Delete();
            var _writer = t.CreateText();
            _writer.Write(data);
            _writer.Close();
        }
    }

    public string Load()
    {
        FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);
        if (in2)
            t = new FileInfo(_FileName);
        if (t.Exists)
        {
            var _reader = t.OpenText();           
            string send = _reader.ReadToEnd();
            _reader.Close();
            return send;
        }
        else
        {
            Debug.Log("SaveFile NotFound" + _FileLocation + "/" + _FileName);
        }
        return null;
    }
}
