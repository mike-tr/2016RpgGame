using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;

/*
public class UI_Buttons : MonoBehaviour
{

    Dictionary<UiType, Dictionary<string, SmartButton>> KeyButtons = new Dictionary<UiType, Dictionary<string, SmartButton>>();
    private GameObject UIButton;

    public LoadExternal LE;
    public AddSpriteN MyCharacter;

    RectTransform Rtp;
    //float scale_by = 12.3f;

    private float scale_by = 14.57f;
    private bool Hided = true;

    // Use this for initialization
    void Start()
    {
        UIButton = (GameObject)Resources.Load("Other/prefabs/UiButton");

        Rtp = transform.parent.GetComponent<RectTransform>();

        Vector2 temp = Rtp.offsetMin;
        temp.y *= scale_by;
        Rtp.offsetMin = temp;

        ReloadButtons();
    }

    void ReloadButtons()
    {
        //Dictionary<string, SmartButton> sm = KeyButtons.Values[0];
        foreach (Dictionary<string, SmartButton> sm in KeyButtons.Values)
            foreach (string s in sm.Keys)
                sm[s].DeleteAll();

        KeyButtons = new Dictionary<UiType, Dictionary<string, SmartButton>>();
        KeyButtons.Add(UiType.None, new Dictionary<string, SmartButton>());
        KeyButtons.Add(UiType.Male, new Dictionary<string, SmartButton>());
        KeyButtons.Add(UiType.MandF, new Dictionary<string, SmartButton>());
        KeyButtons.Add(UiType.Female, new Dictionary<string, SmartButton>());

        AddButton_Parent("Item_List", -1, new Color(0, 0.5f, 0.2f, 0.5f), UiType.None);
        try
        {
            ///////////////////////////// Start Male
            AddButton_ParentAndChild("Male", "Item_List", -2, new Color(0.45f, 0.75f, 0.5f, 0.5f), UiType.Male, UiType.None);
            foreach (string s in DataBase.Sprite_DataBase[SpriteGender.male].Data.Keys)
            {
                AddButton_ParentAndChild(s, "Male", -2, new Color(0.25f, 0.75f, 0.5f, 0.5f), UiType.Male, UiType.Male);
                //AddButton_IsChild("Remove", "Remove", s, new Color(0.75f, 0.6f, 0.6f, 0.5f));

                foreach (SpriteData sd in DataBase.Sprite_DataBase[SpriteGender.male].Data[s])
                {
                    AddButton_IsChild(sd.name, sd.path, sd.key, sd.id, new Color(0.0f, 1.0f, 0.5f, 0.5f), UiType.Male);
                }

                AddButton_IsChild("Remove", "Remove", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.Male);
                AddButton_IsChild("Hide", "Hide", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.Male);
            }
            //////////////////////// Start No-Gender
            AddButton_ParentAndChild("Both", "Item_List", -2, new Color(0.45f, 0.75f, 0.5f, 0.5f), UiType.MandF, UiType.None);
            foreach (string s in DataBase.Sprite_DataBase[SpriteGender.both].Data.Keys)
            {
                AddButton_ParentAndChild(s, "Both", -2, new Color(0.25f, 0.75f, 0.5f, 0.5f), UiType.MandF, UiType.MandF);
                //AddButton_IsChild("Remove", "Remove", s, new Color(0.75f, 0.6f, 0.6f, 0.5f));

                foreach (SpriteData sd in DataBase.Sprite_DataBase[SpriteGender.both].Data[s])
                {
                    AddButton_IsChild(sd.name, sd.path, sd.key, sd.id, new Color(0.0f, 1.0f, 0.5f, 0.5f), UiType.MandF);
                }

                AddButton_IsChild("Remove", "Remove", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.MandF);
                AddButton_IsChild("Hide", "Hide", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.MandF);
            }
            ////////////////////
            AddButton_ParentAndChild("Female", "Item_List", -2, new Color(0.45f, 0.75f, 0.5f, 0.5f), UiType.Female, UiType.None);
            foreach (string s in DataBase.Sprite_DataBase[SpriteGender.female].Data.Keys)
            {
                AddButton_ParentAndChild(s, "Female", -2, new Color(0.25f, 0.75f, 0.5f, 0.5f), UiType.Female, UiType.Female);
                //AddButton_IsChild("Remove", "Remove", s, new Color(0.75f, 0.6f, 0.6f, 0.5f));

                foreach (SpriteData sd in DataBase.Sprite_DataBase[SpriteGender.female].Data[s])
                {
                    AddButton_IsChild(sd.name, sd.path, sd.key, sd.id, new Color(0.0f, 1.0f, 0.5f, 0.5f), UiType.Female);
                }

                AddButton_IsChild("Remove", "Remove", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.Female);
                AddButton_IsChild("Hide", "Hide", s, -2, new Color(0.75f, 0.6f, 0.6f, 0.5f), UiType.Female);
            }

        }
        catch { }

        AddButton_Parent("esc", -3, new Color(0.3f, 0.4f, 0.5f, 0.5f), UiType.None, "options", false);

        AddButton_ParentAndChild("RJR", "esc", -4, new Color(0, 0.5f, 0.2f, 0.5f), UiType.None, UiType.None, false, "ReWrite JSON & Reload");
        AddButton_ParentAndChild("RJ", "esc", -4, new Color(0, 0.5f, 0.2f, 0.5f), UiType.None, UiType.None, false, "Reload Json");
    }

    public void AddButton_IsChild(string name, string path, string key, int id, Color color, UiType sg, string ConstText = null)
    {
        Transform child = Instantiate(UIButton).transform;
        Text text = child.GetChild(0).GetComponent<Text>();
        child.name = name;

        text.text = (ConstText != null) ? ConstText : name;
        child.SetParent(transform);

        child.localPosition = Vector3.zero;
        child.localScale = new Vector3(1, 1, 1);

        Button b = child.GetComponent<Button>();
        OnButtonClick temp = new OnButtonClick(path, key, id, sg, this);
        b.onClick.AddListener(temp.CallFuntion);


        KeyButtons[sg][key].AddChild(child);

        Image image = child.GetComponent<Image>();
        image.color = color;


        child.gameObject.SetActive(false);

    }

    public void AddButton_ParentAndChild(string s, string parent_key, int id, Color color, UiType sg, UiType parentG, bool AddShow = true, string ConstText = null)
    {
        Transform child = Instantiate(UIButton).transform;
        Text text = child.GetChild(0).GetComponent<Text>();
        child.name = s;
        //child.GetChild(0).name = sd.key;
        text.text = (ConstText != null) ? ConstText : s;
        child.SetParent(transform);

        child.localPosition = Vector3.zero;
        child.localScale = new Vector3(1, 1, 1);

        Button b = child.GetComponent<Button>();
        OnButtonClick temp = new OnButtonClick(s, s, id, sg, this);
        b.onClick.AddListener(temp.CallFuntion);

        SmartButton Ked = new SmartButton(text, s, child);
        Ked.HideText = (ConstText != null) ? ConstText : s;
        Ked.ShowText = (ConstText != null) ? ConstText : s;

        if (AddShow)
        {
            text.text += " - show";
            Ked.HideText += " - hide";
            Ked.ShowText += " - show";
        }
        KeyButtons[sg].Add(s, Ked);

        Image image = child.GetComponent<Image>();
        image.color = color;

        KeyButtons[parentG][parent_key].AddChild(child, Ked);
        child.gameObject.SetActive(false);
    }

    public void AddButton_Parent(string s, int id, Color color, UiType sg, string ConstText = null, bool AddShow = true)
    {

        Transform child = Instantiate(UIButton).transform;
        Text text = child.GetChild(0).GetComponent<Text>();
        child.name = s;
        //child.GetChild(0).name = sd.key;
        text.text = (ConstText != null) ? ConstText : s;

        child.SetParent(transform);

        child.localPosition = Vector3.zero;
        child.localScale = new Vector3(1, 1, 1);

        Button b = child.GetComponent<Button>();
        OnButtonClick temp = new OnButtonClick(s, s, id, sg, this);
        b.onClick.AddListener(temp.CallFuntion);

        SmartButton Ked = new SmartButton(text, s, child);
        Ked.HideText = (ConstText != null) ? ConstText : s;
        Ked.ShowText = (ConstText != null) ? ConstText : s;

        if (AddShow)
        {
            text.text += " - show";
            Ked.HideText += " - hide";
            Ked.ShowText += " - show";
        }

        KeyButtons[sg].Add(s, Ked);

        Image image = child.GetComponent<Image>();
        image.color = color;
    }

    public void ExcuteCode(SpriteGender sg, string key, int id)
    {
        MyCharacter.AddOrReplaceSprite(sg, key, id);
    }

    public void RemoveItem(string key)
    {
        MyCharacter.AddEmptyOrRemoveSprite(key);
    }

    public void OpenMore(string key, UiType sg)
    {
        bool hide = KeyButtons[sg][key].Hide;
        KeyButtons[sg][key].setActive(hide);

        switch (key)
        {
            case "RJR":
                LE.reWriteData();
                LE.reLoadData();
                ReloadButtons();
                break;
            case "RJ":
                LE.reLoadData();
                ReloadButtons();
                break;
            case "Item_List":
            case "esc":
                //increase the size of the box
                Hide_itOrShow = 2;
                break;


        }
    }


    public void HideTree(string key, UiType sg)
    {
        KeyButtons[sg][key].setActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }

    int Hide_itOrShow = 0;
    void LateUpdate()
    {
        if (Hide_itOrShow == 1)
        {
            Vector2 temp = Rtp.offsetMin;
            if (KeyButtons[UiType.None]["esc"].Hide && KeyButtons[UiType.None]["Item_List"].Hide)
            {
                temp.y *= scale_by;
                Hided = true;
            }
            else if (Hided)
            {
                Hided = false;
                temp.y /= scale_by;
            }
            Rtp.offsetMin = temp;
        }
        if (Hide_itOrShow > 0)
        {
            Hide_itOrShow--;
        }
    }

}

public class OnButtonClick
{
    string path;
    string key;
    UiType sg;
    int id;
    UI_Buttons parent;

    public OnButtonClick(string _path, string _key, int _id, UiType _sg, UI_Buttons _parent)
    {
        path = _path;
        key = _key;
        id = _id;
        parent = _parent;
        sg = _sg;
    }

    public void CallFuntion()
    {
        if (path == "Remove")
        {
            parent.RemoveItem(key);
        }
        else if (path == "Hide")
        {
            parent.HideTree(key, sg);
        }
        else if (path != key)
            parent.ExcuteCode(path, key, id);
        else
            parent.OpenMore(key, sg);
    }
}
*/
public class SmartButton
{
    List<BT> childs = new List<BT>();
    string key;
    public Text text;
    public bool Hide = true;

    public string HideText;
    public string ShowText;

    Transform transform;

    public List<BT> GetChilds()
    {
        return childs;
    }

    public void AddChild(Transform child, SmartButton sb = null)
    {
        BT temp = new BT();
        temp.child_transform = child;
        temp.child = sb;

        childs.Add(temp);

    }

    public void DeleteAll()
    {
        foreach (BT t in childs)
        {
            setActive(true);

            if (t.child != null)
                t.child.DeleteAll();
            t.child_transform.gameObject.AddComponent<DestroySelf>();

        }
        transform.gameObject.AddComponent<DestroySelf>();
    }

    public void setActive(bool active)
    {
        Hide = !active;

        if (active)
        {
            foreach (BT t in childs)
            {
                t.child_transform.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (BT t in childs)
            {
                t.child_transform.gameObject.SetActive(false);
                if (t.child != null)
                    t.child.setActive(false);
            }
        }

        if (active)
        {

            text.text = key + " - Hide";
            if (HideText != null)
            {
                text.text = HideText;
            }
        }
        else
        {
            text.text = key + " - Show";
            if (ShowText != null)
            {
                text.text = ShowText;
            }
        }
    }



    public SmartButton(Text _text, string _key, Transform _transform)
    {
        text = _text;
        key = _key;
        transform = _transform;
    }
}


public class BT
{
    public Transform child_transform;
    public SmartButton child;
}