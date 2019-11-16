using System.Collections;
using UnityEngine;

public class OnKeyBoard_ui : MonoBehaviour {
    public static OnKeyBoard_ui KeyboardC;
    public static bool Pause
    {
        get
        {
            return ActiveObjects != 0;
        }
    }
    public static bool TakeItem = false;
    private static int ActiveObjects = 0;
    public int ACTO = 0;
    public GameObject[] UI_elements;
    public KeyCode[] UI_callKey;
	
    IEnumerator Start()
    {
        KeyboardC = this;
        foreach(GameObject g in UI_elements)
        {
            g.SetActive(true);
        }
        yield return new WaitForSeconds(Time.deltaTime);
        foreach(GameObject g in UI_elements)
        {
            g.SetActive(false);
        }
        ActiveObjects = 0;
    }

	// Update is called once per frame
    
    public bool ALIN = false;
    public void ALINSet()
    {
        StopCoroutine(SetFalse());
        StartCoroutine(SetFalse());
        ALIN = !ALIN;
    }

    public void UnStuck()
    {

    }
	void Update () {
        int i = 0;
        
		foreach(KeyCode key in UI_callKey)
        {
            if(Input.GetKeyDown(key))
            {
                SetObjectActive(UI_elements[i], !UI_elements[i].activeSelf);
                
            }
            i++;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            TakeItemUI();
        }else if(Input.GetKeyDown(KeyCode.E))
        {
            SetObjectActive(UI_elements[0], !UI_elements[0].activeSelf);
            SetObjectActive(UI_elements[1], UI_elements[0].activeSelf && ALIN);
        }
        else if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            ALIN = !ALIN;
        }
	}

    public void TakeItemUI()
    {
        StartCoroutine(Take());
    }

    IEnumerator Take()
    {
        TakeItem = true;
        yield return null;
        TakeItem = false;
    }
    public void SetActive(int i)
    {
        StopCoroutine(SetFalse());
        StartCoroutine(SetFalse());
        SetObjectActive(UI_elements[i], !UI_elements[i].activeSelf);
    }

    public void SetAcElem1()
    {
        SetObjectActive(UI_elements[1], UI_elements[0].activeSelf && ALIN);
    }

    IEnumerator SetFalse()
    {
        ActiveObjects++;
        yield return null;
        ActiveObjects--;
    }
    void SetObjectActive(GameObject Object,bool active)
    {
        if(Object.activeSelf == active)
            return;
        Object.SetActive(active);

        if(active)
            ActiveObjects++;
        else
            ActiveObjects--;
        
        ACTO = ActiveObjects;
    }
}
