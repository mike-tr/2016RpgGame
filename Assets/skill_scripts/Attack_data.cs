using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_data : MonoBehaviour {

    private float dmg = 100f;
    private float scatter = 5;

    private skill_type st;
    public damage_type type;

    public bool triggered = false;

    private Transform t;
    public Transform caster
    {
        get { return t; }
    }

    public void Init_Attack(skill_type _st, float _dmg, float _scatter, 
        float LifeSpan, Transform _caster,  Collider2D collider, damage_type dmg_type)
    {
        st = _st;
        dmg = _dmg;
        t = _caster;
        scatter = _scatter;
        if(st != skill_type.spell)
            StartCoroutine(Sleep_after(LifeSpan));
        type = dmg_type;


        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
    }

    public float DMG(bool a = false)
    {
        //if (!triggered && st == skill_type.spell)
          //  GetComponent<Spel1_script>().ZeroV(a);

        gameObject.layer = 9;
        Physics2D.IgnoreLayerCollision(9, 9, true);

        triggered = true;

        if (st == skill_type.arrow_shot)
        {
            StartCoroutine(Sleep_after(Time.deltaTime));
        }




        return Random.Range(dmg / scatter, dmg * 1.2f);

    }

    IEnumerator Sleep_after(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
	// Use this for initialization
}

public enum skill_type
{
    spell,
    attack,
    arrow_shot,
}

public enum weapon_type
{
    none = 0,
    spear = 1,
    sword = 3,
    oversize_spear = spear + 1,
    oversize_sword = sword + 1,
    wand,
    bow,
    
}