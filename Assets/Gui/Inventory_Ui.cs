using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Ui : MonoBehaviour {
    GameObject UIButton;
    public INV_handle add_t;
    void Start()
    {
        UIButton = (GameObject)Resources.Load("Other/prefabs/UiSlot");
        ITEMS_load.Get.LoadItems(transform, UIButton, add_t);     
        Destroy(this);   
    }


}
