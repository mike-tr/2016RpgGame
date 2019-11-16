using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {

    public Slider HealthBar;
    public Text CH; // current hp text
    public Text TH; // total hp text
    private bool UpdateText;
    CanvasGroup cg;
    // Use this for initialization
    void Start() {
        if (CH == null || TH == null)
        {
            UpdateText = false;
            cg = GetComponent<CanvasGroup>();
        }
        else
            UpdateText = true;
        
    }

    // Update is called once per frame
    public int TotalHealth
    {
        set
        {
            HealthBar.maxValue = value;
            if (UpdateText)
                TH.text = value.ToString();
        }
    }

    public int CurrentHealth
    {
        set
        {
            HealthBar.value = value;
            if(UpdateText)
                CH.text = value.ToString();
        }
    }

    public void IsDead(bool state)
    {
        if (UpdateText)
            return;
        if (state)
            cg.alpha = 0;
        else
            cg.alpha = 1;
    }
}
