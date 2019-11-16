using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControl))]
public class Health : MonoBehaviour {

    public int player_id
    {
        get { return cc.player_id; }
    }
    public float Movement
    {
        get { return cc.walk_speed.speed_t; }
    }

    private float healthT;

    public float Current_health;
    private float hp;

    public float DealDmgtest = 0;
    public float health
    {
        get
        {
            return hp;
        }
        set
        {
            Current_health = value;
            hp = value;
            HealthBar.CurrentHealth = Mathf.RoundToInt(value);
            if(isDead)
                Kill();
        }
    }

    public void GotHeal(float heal)
    {
        health = Mathf.Clamp(health + heal, 0, healthT); 
    }
    public float armor;
    public float health_regen;

    private CharacterControl cc;
    private C_Stats stats;
    private AddItem Items;

    public bool KillPlayer;
    public bool revive_player;
    public bool isDead
    {
        get{
            return health <= 0;
        }
    }

    public HealthBarUI HealthBar;
    public CapsuleCollider2D coll;
	// Use this for initialization
	void Start () {
        Items = GetComponent<AddItem>();
        cc = GetComponent<CharacterControl>();
        health = healthT;
        stats = Items.stats;

        StartCoroutine(HealthRegen());
	}
	
	// Update is called once per frame
    
    public void HealthChange(float hp)
    {
        float dmg = healthT - health;
        healthT = hp;
        HealthBar.TotalHealth = Mathf.RoundToInt(healthT);
        health = healthT - dmg;
    }

    void Update()
    {
        if (armor != stats.armor)
            armor = stats.armor;
        
        if(Application.isEditor)
        {
            if(revive_player && isDead)
            {
                Revive();
                revive_player = false;
            }
            else if(KillPlayer && !isDead)
            {
                health = 0;
                KillPlayer = false;
            }
            else if(DealDmgtest > 0)
            {
                health -= DealDmgtest;
                DealDmgtest = 0;
            }
        }
    }

    public void ApplyDmg(float dmg, damage_type type)
    {
        float dm = dmg * (25 / stats.armor);
        if (dm < dmg * 0.05f)
            dm = dmg * 0.05f;
        health -= dm;
    }

    void Kill()
    {
        coll.enabled = false;
        
        Items.IsDead();
        cc.path.StopPath();
        cc.hpt = null;
        cc.SetNextAnim(CharacterAnimation.Hurt);
        HealthBar.IsDead(isDead);
    }

    void Revive()
    {
        coll.enabled = true;
        health = healthT;
        HealthBar.IsDead(false);
        cc.SetNextAnim(CharacterAnimation.idle);
    }

    IEnumerator HealthRegen()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if(isDead)
                continue;
            health_regen = stats.health_regen;

            GotHeal(stats.health_regen / 25);
        }
    }

}

public enum damage_type
{
    physical,
    megical,
}