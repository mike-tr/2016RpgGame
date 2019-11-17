using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



[System.Serializable]
public class ScaleElement
{
    public RectTransform Element;
    [HideInInspector]
    public Vector2 StartPos;
    public Vector2 OffsetPerScale;
    public float downScale = 1f;

}
public class UIMenu : MonoBehaviour {

    public static UIMenu UiMenu;
    public ScaleElement[] UIElements;
    public float Scale
    {
        get
        {
            return transform.localScale.x;
        }
        set
        {        
            transform.localScale = new Vector3(value, value, 1);
            foreach(ScaleElement se in UIElements)
            {
                if(value < 1) {
                    se.Element.position = se.StartPos - se.OffsetPerScale * (1 - value) * se.downScale;
                    se.Element.localScale = transform.localScale;
                } else {
                    se.Element.position = se.StartPos - se.OffsetPerScale * (1 - value);
                    se.Element.localScale = transform.localScale;
                }
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGame()
    {
        if(LoadExternal.main == null)
        {
            ChatGUI.addLine("ERROR : LoadExternal was not loaded!");
            return;
        }
        ChatGUI.addLine("researching textures..." +
            "/nAnd resaving data.");
        LoadExternal.main.reWriteData();
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        LoadExternal.main.reLoadData();
        ChatGUI.addLine("Reloading Game in...");
        yield return new WaitForSeconds(1f);
        LoadExternalAsSprite.ReloadDataBase();
        for (int i = 10; i > 0; i--)
        {
            ChatGUI.addLine(i.ToString() + "...");
            yield return new WaitForSeconds(0.25f);
        }
        SceneManager.LoadScene(1);
    }


	// Use this for initialization
	void Start () {
        UiMenu = this;

        foreach (ScaleElement se in UIElements) {
            se.StartPos = se.Element.position;
            se.Element.localScale = transform.localScale;
        }

        Scale = 1.0f;
	}
}
