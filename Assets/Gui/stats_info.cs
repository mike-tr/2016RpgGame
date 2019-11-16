using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stats_info : MonoBehaviour {
    static public C_Stats stats;
    public Text pname;
    public Text strength;
    public Text agility;
    public Text intelligence;
    public Text range;
    public Text mobiity;
    public Text endurance;

    public Text Health;
    public Text physical_dmg;
    public Text health_regen;
    public Text magic_dmg;
    public Text evasion;
    public Text armor;

    // Use this for initialization
    IEnumerator Start () {
        while (Info_update.instanse == null)
            yield return new WaitForSeconds(Time.deltaTime);
        Info_update.instanse.si_update_list.Add(this);
	}


    public void ReWriteStats()
    {
        if(stats == null)
            return;
        strength.text = (stats.strength).ToString("F0");
        agility.text = (stats.agility).ToString("F0");
        intelligence.text = (stats.intelligence).ToString("F0");
        range.text = (stats.range).ToString("F1");
        mobiity.text = (stats.mobility).ToString("F0");
        endurance.text = (stats.endurance).ToString("F0");
        Health.text = (stats.health).ToString("F1");
        physical_dmg.text = (stats.physical_dmg).ToString("F0");
        magic_dmg.text = (stats.magic_dmg).ToString("F0");
        health_regen.text = (stats.health_regen).ToString("F1");
        evasion.text = (stats.evasion).ToString("F1");
        armor.text = (stats.armor).ToString("F1");
        //pname.text = (CharacterControl).ToString("F2");
    }
}
