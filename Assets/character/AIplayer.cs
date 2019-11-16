using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveCharacter))]
[RequireComponent(typeof(CharacterControl))]
public class AIplayer : MonoBehaviour {
    static public List<Health> AI = new List<Health>();

    public Transform target
    {
        get
        {
            return cc.target;
        }
        set
        {
            cc.target = value;
        }
    }
    private Health target_c;

    private MoveCharacter mc;
    private CharacterControl cc;
    private AddItem item;
    public bool Aggressive = false;
    C_Stats stats;

    Vector2 Dist;

    private CharacterAnimation next_action
    {
        set
        {
            cc.SetNextAnim(value);
        }
    }


    private CharacterDirection direction
    {
        get { return cc.direction; }
        set { cc.direction = value; }
    }

    IEnumerator run;
    IEnumerator w8bruns;
    void Start()
    {
        Dist = Vector2.zero;
        mc = GetComponent<MoveCharacter>();
        cc = GetComponent<CharacterControl>();

        cc.IsMine = false;

        run = RunAway();
        //StartCoroutine(run);
        item = GetComponent<AddItem>();
        stats = item.stats;

        AI.Add(GetComponent<Health>());
    }

    public float dist_w = 10f;

    
    CharacterDirection[] dirt = new CharacterDirection[2];
    float dist2 = 0;
    void botWalk(Vector2 vel)
    {

        if (Mathf.Abs(vel.y) > 1.5f)
        {
            if (vel.y > 0)
            {
                dirt[0] = CharacterDirection.back;
            }
            else
            {
                dirt[0] = CharacterDirection.front;
            }
        }
        else vel.y = 0;
        if (Mathf.Abs(vel.x) > 1.5f)
        {
            if (vel.x > 0)
            {
                dirt[1] = CharacterDirection.right;
            }
            else
            {
                dirt[1] = CharacterDirection.left;
            }
        }
        else
            vel.x = 0;

        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y * 0.33f))
            direction = dirt[1];
        else
            direction = dirt[0];

        cc.Movement = vel;
        next_action = CharacterAnimation.Walk;
    }

    public bool running = false;
    void AiNa()
    {
        if (cc.action == CharacterAnimation.Hurt)
            return;

        float dist = Vector2.Distance(Dist, Vector2.zero);
        if (dist > dist_w - dist2)
        {
            dist2 = dist_w * 0.33f;

            if (cc.action != CharacterAnimation.idle && cc.action != CharacterAnimation.Walk)
                return;

            
            //botWalk(Dist);           
        }
        else
        {
            if (Mathf.Abs(Dist.y) > Mathf.Abs(Dist.x))
            {
                if (Dist.y > 0)
                    direction = CharacterDirection.back;
                else
                    direction = CharacterDirection.front;
            }
            else
            {
                if (Dist.x > 0)
                    direction = CharacterDirection.right;
                else
                    direction = CharacterDirection.left;
            }
            switch (cc.weapon)
            {
                case weapon_type.spear:
                case weapon_type.spear + 1:
                    next_action = CharacterAnimation.Thrust;
                    break;
                case weapon_type.sword:
                case weapon_type.sword + 1:
                    next_action = CharacterAnimation.Slash;
                    break;
                case weapon_type.wand:
                    next_action = CharacterAnimation.Slash;
                    break;
                case weapon_type.bow:
                    next_action = CharacterAnimation.Shoot;
                    break;
            }
        }
        /*
        else
        {
            next_action = CharacterAnimation.idle;
            Dist = Vector2.zero;
            dist2 = 0;
            //mc.Dist = Dist;  
        }*/
    }

    void GetTarget()
    {
        float dist = 150;
        target_c = null;
        foreach(Health cc_ai in AI)
        {
            if(!cc_ai.isDead && cc_ai.player_id != cc.player_id)
            {
                float temp = Vector2.Distance(cc_ai.transform.position, transform.position);
                if(temp < dist)
                {
                    dist = temp;
                    target_c = cc_ai;                   
                }
            }
        }
        if(target_c != null)
            target = target_c.transform;
    }

    // Update is called once per frame
    void Update () {
        if (cc.IsDead)
            return;

        if (target != null)
        {
            Dist = target.position - transform.position;
            if (!running)
            {             
                AiNa();
            }
            else
            {               
                botWalk(Dist * -1f);
            }
        }
        else
        {   
            if(Aggressive)
                GetTarget();
            return;
        }
        
        dist_w = stats.range;

    }

    public int runfail = 0;
    IEnumerator RunAway()
    {
        float temp = 0;
        float d2 = 1000;
        while (true)
        {
            if (target != null)
            {
                temp = Vector2.Distance(transform.position, target.position);
                if (!running)
                {
                    if (d2 < dist_w * 0.25 && runfail < 10)
                        running = true;
                }
                else
                {
                    if (d2 <= temp)
                        runfail += 4;
                    runfail++;
                    if (runfail > 10)
                    {
                        running = false;
                        yield return new WaitForSeconds(1.5f);
                        runfail = 0;
                    }
                }
                d2 = temp;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


}
