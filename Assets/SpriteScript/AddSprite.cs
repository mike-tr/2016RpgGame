using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AddSprite : MonoBehaviour {

    Dictionary<string, Transform> sprites = new Dictionary<string, Transform>();

    public GameObject imageContainer;

    public string[] DrawOrder;

    public CharacterDirection direction;
    public CharacterAnimation action;

    private CharacterAnimation LastAnim;

    private int MaxRow = 0;
    public int ImageIndex = 0;
    public int SpriteColumn = 0;

    public float speed = 8;
    // Use this for initialization
    void Start()
    {
        imageContainer = (GameObject)Resources.Load("ImageContainer");
        StartCoroutine(ChangeImage());
        MaxRow = CharacterAnimations.GetAnimationLenght(action);

        foreach(string s in DrawOrder)
        {
            AddEmptyOrRemoveSprite(s);
        }
    }

    public void AddOrReplaceSprite(string path, string key)
    {
        try
        {
            RawImage tex = sprites[key].GetComponent<RawImage>();             
            //tex.texture = LoadExternal.LoadTextureToFile(InGameData.Sprite_path + path + ".png");
            tex.color = new Color(1,1,1,1);
        }
        catch
        {
            Transform child = Instantiate(imageContainer).transform;

            child.name = key;
            RawImage tex = child.GetComponent<RawImage>();
            //tex.texture = LoadExternal.LoadTextureToFile(InGameData.Sprite_path + path + ".png");
            child.SetParent(transform);

            child.localPosition = Vector3.zero;
            child.localScale = new Vector3(1, 1, 1);
            sprites.Add(key, child);
        }
    }

    public void AddEmptyOrRemoveSprite(string key)
    {
        try
        {
            RawImage tex = sprites[key].GetComponent<RawImage>();
            tex.color = new Color(1, 1, 1, 0);
        }
        catch
        {
            Transform child = Instantiate(imageContainer).transform;

            child.name = key;
            RawImage tex = child.GetComponent<RawImage>();
            tex.color = new Color(1,1,1,0);
            if(key == "body")
            {
                //tex.texture = LoadExternal.LoadTextureToFile(InGameData.Sprite_path + DataBase.Sprite_DataBase.Data["body"][0].path + ".png");
                tex.color = new Color(1, 1, 1, 1);
            }
            child.SetParent(transform);

            child.localPosition = Vector3.zero;
            child.localScale = new Vector3(1, 1, 1);
            sprites.Add(key, child);
        }
    }

    public void CheckAnimation()
    {
        if (LastAnim != action)
        {
            ImageIndex = 0;
            LastAnim = action;
            MaxRow = CharacterAnimations.GetAnimationLenght(action);
        }

        SpriteColumn = ((int)action * 4) + (int)direction;
        if (SpriteColumn >= CharacterAnimations.MaxColumn - 1)
            SpriteColumn = CharacterAnimations.MaxColumn - 1;

        transform.localPosition = new Vector3(-ImageIndex * 64 - 32, SpriteColumn * 64 + 32,0);
    }

    // Update is called once per frame
    void LateUpdate () {
        CheckAnimation();
	}


    IEnumerator ChangeImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / speed);
            ImageIndex++;
            if (ImageIndex >= MaxRow)
                ImageIndex = 0;
            //(call change image)
        }
    }


}
