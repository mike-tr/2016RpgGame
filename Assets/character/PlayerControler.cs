using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {

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

    private MoveCharacter mc;
    private CharacterControl cc;
    private AddItem item;
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

    void Start()
    {
        Dist = Vector2.zero;
        mc = GetComponent<MoveCharacter>();
        cc = GetComponent<CharacterControl>();

        //StartCoroutine(run);
        item = GetComponent<AddItem>();
        stats = item.stats;
        AIplayer.AI.Add(GetComponent<Health>());
    }

    public float dist_w = 10f;
    float dist2 = 0;
    void AiNa()
    {
        if (cc.action == CharacterAnimation.Hurt)
            return;

        float dist = Vector2.Distance(Dist, Vector2.zero);
        if (dist > dist_w - dist2)
        {
            dist2 = dist_w * 0.33f;      
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
    }



    // Update is called once per frame
    void Update () {
        if (cc.IsDead)
            return;

		if(Input.GetKey(KeyCode.Space))
		{
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

        if (target != null)
        {
            Dist = target.position - transform.position;           
			AiNa();
        }
        else
        {   
            return;
        }
        
        dist_w = stats.range;

    }

}
