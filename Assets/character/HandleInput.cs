using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MoveCharacter))]
[RequireComponent(typeof(CharacterControl))]
public class HandleInput: MonoBehaviour {

    Vector2 Movement_OnTouch;
    public AddSpriteN player;
    private MoveCharacter mc;
    private CharacterControl cc;

    public GameObject cam;

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


    // Use this for initialization
    void Start()
    {
        Movement_OnTouch = Vector2.zero;
        mc = GetComponent<MoveCharacter>();
        cc = GetComponent<CharacterControl>();

        cam.SetActive(cc.IsMine);
        this.enabled = cc.IsMine;
        
        //gameObject.SetActive(mc.IsMine);
        //this.enabled = mc.IsMine;
    }

    void Awake()
    {
        if (cc == null)
            return;
        cam.SetActive(cc.IsMine);
        this.enabled = cc.IsMine;    
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //if (!mc.IsMine)
          //  return;

        if (Application.platform != RuntimePlatform.Android)
            Keyboard();
        else
            InputMobile();
    }

    void InputMobile()
    {
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary))
        {
            // Get movement of the finger since last frame
            Movement_OnTouch += Input.GetTouch(0).deltaPosition;
            var dist = Vector2.Distance(Movement_OnTouch, Vector2.zero);
            if (dist > 10f)
                Movement_OnTouch *= (10f / dist);
            // Move object across XY plane
            //mc.Movement = Movement_OnTouch;    
            if(Mathf.Abs(Movement_OnTouch.x) < 0.5f)
            {
                Movement_OnTouch.x = 0;
            }
            if (Mathf.Abs(Movement_OnTouch.y) > 0.5f)
            {
                Movement_OnTouch.x = 0;
            }
            cc.Movement = Movement_OnTouch;
            cc.SetNextAnim(CharacterAnimation.Walk);

        }
        else if (Input.touchCount > 0)
        {

        }
        else
        {
            next_action = CharacterAnimation.SpellCast;
            Movement_OnTouch = Vector2.zero;
            //mc.Movement = Movement_OnTouch;  
        }
    }

    void Keyboard()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            switch(cc.weapon)
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
        else if (Input.GetKey(KeyCode.R))
        {
            next_action = CharacterAnimation.SpellCast;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = CharacterDirection.right;
            Vector2 m = Vector2.right;
            next_action = CharacterAnimation.Walk;
            if (Input.GetKey(KeyCode.W))
            {
                m += Vector2.up;
                m *= 0.67f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m += Vector2.down;
                m *= 0.67f;
            }
            cc.Movement = m;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = CharacterDirection.left;
            Vector2 m = Vector2.left;
            next_action = CharacterAnimation.Walk;
            if (Input.GetKey(KeyCode.W))
            {
                m += Vector2.up;
                m *= 0.67f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m += Vector2.down;
                m *= 0.67f;
            }
            cc.Movement = m;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = CharacterDirection.back;
            cc.Movement = Vector2.up;
            next_action = CharacterAnimation.Walk;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = CharacterDirection.front;
            cc.Movement = Vector2.down;
            next_action = CharacterAnimation.Walk;
        }
    }
    


}
