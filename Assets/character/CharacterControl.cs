using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    private static int PLC = 0;
    private static int NEXT_PLAYER_ID
    {
        get { PLC++; return PLC - 1; }
    }
    private static Transform Skill_Dump;

    public bool Intialized = false;

    private int ID;
    public int player_id
    {
        get { return ID; }
    }

    private Dictionary<skill_type, List<Transform>> skill_rec = new Dictionary<skill_type, List<Transform>>();

    public AddSpriteN asn;
    public Collider2D MyCollider;
    private C_Stats stats;


    public Transform target
    {
        get
        {

            if (TargetDead)
                t = null;
            return t;
        }
        set
        {
            t = value;
            hpt = t.GetComponent<Health>();
        }
    }

    public void ResetTarget()
    {
        t = null;
        hpt = null;
    }
    public Transform t;
    public Health hpt;

    private bool TargetDead
    {
        get
        {
            if (hpt == null)
                return true;
            if (!hpt.isDead)
                return false;
            hpt = null;
            return true;
        }
    }

    public float spell_range
    {
        get { return stats.spell_range;  }
    }
    public SpriteSpeed cast_speed
    {
        get { return stats.cast_speed; }
    }
    public SpriteSpeed Attack_speed
    {
        get { return stats.Attack_speed; }
    }
    public SpriteSpeed Shooting_speed
    {
        get { return stats.Shooting_speed; }
    }
    public SpriteSpeed walk_speed
    {
        get { return stats.walk_speed; }
    }
    public float physical_dmg
    {
        get { return stats.physical_dmg; }
    }
    public float magic_dmg
    {
        get { return stats.magic_dmg; }
    }




    public List<skill_type> Attacks = new List<skill_type>();

    public weapon_type weapon;

    public CharacterAnimation EDITOR_setAnim = CharacterAnimation.idle;

    public CharacterAnimation action
    {
        get{ return asn.action;}
    }
    public void SetNextAnim(CharacterAnimation next_anim)
    {
        asn.action = next_anim;
        Kp = true;
    }

    public bool Kp = false;

    public CharacterDirection direction;
    public Vector2 Movement;

    public bool IsDead
    {
        get { return hp.isDead; }
    }

    public GameObject skill1;
    public GameObject physic_attack;
    public GameObject arrow;
    public Health hp;
    // Use this for initialization

    public GetPath path;

    private Transform ATBS;
    void Start () {

        foreach (skill_type stype in EnumCheck<skill_type>.GetEnumValues())
            skill_rec.Add(stype, new List<Transform>());

        if (Skill_Dump == null)
        {
            Skill_Dump = new GameObject().transform;
            Skill_Dump.name = "skill_recycle_bin";
            Skill_Dump.position = Vector2.zero;
        }

        hp = GetComponent<Health>();
        StartReady = true;
        skill1 = Resources.Load<GameObject>("Character/Skills/prefabs/skill_1");
        physic_attack = Resources.Load<GameObject>("Character/Skills/prefabs/attack_box");
        arrow = Resources.Load<GameObject>("Character/skills/prefabs/arrow");
        StartCoroutine(SpawnSpell());
        stats = GetComponent<AddItem>().stats;
        if (IsMine)
            InGameData.add_item = GetComponent<AddItem>();

        ID = NEXT_PLAYER_ID;

        ATBS = new GameObject().transform;
        ATBS.SetParent(this.transform);
        ATBS.name = "AttackBoxHolder";
        ATBS.localEulerAngles = Vector3.zero;
        ATBS.localPosition = Vector3.zero;

        Intialized = true;
    }

    bool temp_mine = false;

    void CastSpell()
    {
        Transform child = null;
        foreach (Transform t in skill_rec[skill_type.spell])
        {
            if (!t.gameObject.activeSelf)
            {
                child = t;
                child.gameObject.SetActive(true);
                break;
            }
        }
        if (child == null)
        {
            child = Instantiate(skill1).transform;
            skill_rec[skill_type.spell].Add(child);
        }
        child.name = skill_type.spell + "_" + ID.ToString();
        Attack_data ss = child.GetComponent<Attack_data>();
        ss.Init_Attack(skill_type.spell, magic_dmg, 10, 0, transform, MyCollider, damage_type.physical);
        Vector3 dir = CharacterAnimations.GetDirection(direction);
        child.localPosition = transform.position + dir * 20 - transform.up * 5;

        Spel1_script ssc = child.GetComponent<Spel1_script>();
        Vector3 rot = child.transform.eulerAngles;
        float r = Random.Range(0, 15) + Random.Range(0, 15);

        if (target == null)
        {
            switch (direction)
            {
                case CharacterDirection.back:
                    rot.z = -90 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.front:
                    rot.z = 90 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.right:
                    rot.z = 180 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.left:
                    rot.z = (Random.Range(-r, r));
                    break;
            }
            rot.z += transform.eulerAngles.z;
        }
        else
        {
            rot.z = (AngleBetweenVector2(transform.position, target.position)) - 180 + (Random.Range(-r, r));
        }
        child.transform.eulerAngles = rot;
        ssc.speed = stats.range / 15;

        ssc.ReLoadSpell();
        child.SetParent(Skill_Dump);
    }

    public float min_r = 10f;
    public float max_r = 20f;
    void Attack()
    {
        if(weapon == weapon_type.wand)
            return;

        Transform child = null;
        foreach(Transform Attack_re in skill_rec[skill_type.attack])
        {
            if(!Attack_re.gameObject.activeSelf)
            {
                child = Attack_re;
                child.gameObject.SetActive(true);
                break;            
            }
        }
        if (child == null)
        {
            child = Instantiate(physic_attack).transform;
            skill_rec[skill_type.attack].Add(child);  
            child.name = skill_type.attack + "_A" + ID.ToString();    
            child.SetParent(ATBS);      
            child.transform.localEulerAngles = Vector3.zero;
        }

        Attack_data ss = child.GetComponent<Attack_data>();
        ss.Init_Attack(skill_type.attack, physical_dmg, 10, 1, transform, MyCollider, damage_type.physical);
        Vector3 dir = CharacterAnimations.GetDirection(direction);

        Vector3 rot = Vector3.zero;
        if (target == null)
            switch (direction)
            {
                case CharacterDirection.back:
                    rot.z = 90 + transform.eulerAngles.z;
                    break;
                case CharacterDirection.front:
                    rot.z = -90 + transform.eulerAngles.z;
                    break;
                case CharacterDirection.right:
                    rot.z = 0 + transform.eulerAngles.z;
                    break;
                case CharacterDirection.left:
                    rot.z = 180 + transform.eulerAngles.z;
                    break;
            }
        else
        {
            rot.z = (AngleBetweenVector2(transform.position, target.position));
        }
        ATBS.eulerAngles = rot;

        float sc = stats.range / 15;
        child.localScale = new Vector2(sc, 0.7f);
        child.localPosition = new Vector3(2 + sc * 6f, 0, 0);
        
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    void arrow_shot()
    {
        Transform child = null;
        foreach (Transform t in skill_rec[skill_type.arrow_shot])
        {
            if (!t.gameObject.activeSelf)
            {
                child = t;
                child.gameObject.SetActive(true);
                break;
            }
        }
        if (child == null)
        {
            child = Instantiate(arrow).transform;
            skill_rec[skill_type.arrow_shot].Add(child);
        }

        Attack_data ss = child.GetComponent<Attack_data>();
        ss.Init_Attack(skill_type.arrow_shot, physical_dmg, 10, stats.range, transform, MyCollider, damage_type.physical);
        Vector3 dir = CharacterAnimations.GetDirection(direction);
        child.localPosition = dir * 4 - transform.up * 2 + transform.position;

        arrow_script ars = child.GetComponent<arrow_script>();
        ars.speed = 10 * (stats.strength * 0.1f);


        Vector3 rot = child.transform.eulerAngles;
        float r = Random.Range(0, 15) + Random.Range(0,15);

        if(target == null)
        {
            switch (direction)
            {
                case CharacterDirection.back:
                    rot.z = -90 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.front:
                    rot.z = 90 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.right:
                    rot.z = 180 + (Random.Range(-r, r));
                    break;
                case CharacterDirection.left:
                    rot.z = (Random.Range(-r, r));
                    break;
                

            }
            rot.z += transform.eulerAngles.z;
        }
        else
        {
            rot.z =  (AngleBetweenVector2(transform.position, target.position)) - 180 + (Random.Range(-r, r));
        }
        child.transform.localEulerAngles = rot;
        child.SetParent(Skill_Dump);
    }

    private Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    IEnumerator SpawnSpell()
    {
        while (true)
        {
            for (int i = 0; i < Attacks.Count; i++)
            {
                SetNextAnim(CharacterAnimation.idle);
                if (Attacks[i] == skill_type.spell)
                    CastSpell();
                else if (Attacks[i] == skill_type.attack)
                    Attack();
                else if (Attacks[i] == skill_type.arrow_shot)
                    arrow_shot();

                Attacks.RemoveAt(i);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

	// Update is called once per frame
	void Update () {
        

        if (!Application.isEditor)
            return;

        if (EDITOR_setAnim != CharacterAnimation.idle)
        {
            SetNextAnim(EDITOR_setAnim);
            EDITOR_setAnim = CharacterAnimation.idle;
        }

        if(mine != temp_mine)
        {
            try
            {
            GetComponent<HandleInput>().enabled = IsMine;
            GetComponent<HandleInput>().cam.SetActive(IsMine);
            }
            catch{}
            temp_mine = mine;
        }
    }

    public bool mine = false;
    private bool StartReady = false;
    public bool IsMine
    {
        get
        {
            return mine;
        }
        set
        {
            if (!StartReady)
                mine = value;
        }
    }
}
