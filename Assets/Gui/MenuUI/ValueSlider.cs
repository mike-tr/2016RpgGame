using UnityEngine;
using UnityEngine.UI;

public class ValueSlider : MonoBehaviour {

    // Use this for initialization
    public Text value;
    public Slider slider;
    public void OnValueChangeS()
    {
        this.value.text = slider.value.ToString("F1") + "f";
        UIMenu.UiMenu.scale = slider.value;
    }

}
