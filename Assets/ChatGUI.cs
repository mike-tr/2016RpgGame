using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChatGUI : MonoBehaviour
    , IPointerEnterHandler
     , IPointerExitHandler
{

	private static ChatGUI instance;
    public Transform Messages;
    public Scrollbar slider;

    static string log;
    static public int FontSize = 24;
    public float TimeToHide = 1f;
	public static void addLine(string value)
	{
        log += "<size=" + FontSize + ">" + value + "</size>\n\n";
        if (instance != null)
        {
            instance.text.text = log;
            instance.slider.value = 0;

            instance.Hide = false;
        }
	}
    // Use this for initialization
    CanvasGroup Group;
	Text text;
    void Start()
    {
        Group = GetComponent<CanvasGroup>();
        instance = this;
        text = Messages.GetComponent<Text>();
        instance.text.text = log;
        instance.slider.value = 0;
        leftToHide = TimeToHide;
        StartCoroutine(HideChat());
    }

    IEnumerator HideChat()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (!Hide)
            {
                if (ov == false)
                {
                    leftToHide -= 0.05f;
                    if (leftToHide <= 0)
                        Hide = true;
                }
            }
            else
            {
                if (Group.alpha > 0.35f)
                {
                    Group.alpha -= 0.10f;
                }
            }
            
        }
    }

    bool hd = false;
    float leftToHide = 2f;
    bool ov = false;
    bool Hide
    {
        get
        {
            return hd;
        }
        set
        {
            Group.alpha = 1f;
            hd = value;
            leftToHide = TimeToHide;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Hide = false;
        ov = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ov = false;
    }


}
