using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_update : MonoBehaviour {
    public static Info_update instanse;

    public List<stats_info> si_update_list = new List<stats_info>();
    public ItemInfo UpdateItemInfo;

	// Use this for initialization
	void Start () {
        instanse = this;
        StartCoroutine(UpdateGUIInfo());	
	}
	
    IEnumerator UpdateGUIInfo()
    {
        while (true)
        {
            foreach (stats_info s in si_update_list)
                s.ReWriteStats();
            if(UpdateItemInfo != null)
                UpdateItemInfo.ReWriteStats();

            yield return new WaitForSeconds(0.5f);
        }
    }
}
