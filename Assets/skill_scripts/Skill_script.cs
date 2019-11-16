using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_script : MonoBehaviour {

    private float power = 50;
    private float scatter = 5;

    private Transform t;
    private damage_type damage_type;

	// Use this for initialization
	void Start () {
        Attack_data dt = gameObject.AddComponent<Attack_data>();
        
        if (t == null)
            t = this.transform;
       
    }
	
    public void InitSpell(float _power, float _scatter, Transform caster, damage_type type)
    {
        power = _power;
        damage_type = type;
        t = caster;
        scatter = _scatter;
    }

	// Update is called once per frame
}
