using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControl))]
public class MoveCharacter : MonoBehaviour {

    public AddSpriteN player;
    private CharacterControl hm;
    private float speed
    {
        get { return hm.walk_speed.speed_t * 0.8f; }
    } 

    public void BlocckOrUnblockDir(dir dr, int d)
    {
        if (dr == dir.buttom)
        {
            bButton = d;
            transform.position += Vector3.up * 33 * speed * Time.deltaTime;
        }
        else if (dr == dir.left)
        {
            bLeft = d;
            transform.position += Vector3.right * 25 * speed * Time.deltaTime;
        }
        else if (dr == dir.right)
        {
            bRight = d;
            transform.position -= Vector3.right * 25 * speed * Time.deltaTime;
        }
        else if (dr == dir.top)
        {
            bTop = d;
            transform.position -= Vector3.up * 33 * speed * Time.deltaTime;
        }
    }

    public int bRight = 0;
    public int bLeft = 0;
    public int bTop = 0;
    public int bButton = 0;

    public bool blockR
    {
        get
        {
            return bRight > 0;
        }
    }
    public bool blockL
    {
        get
        {
            return bLeft > 0;
        }
    }
    public bool blockB
    {
        get
        {
            return bButton > 0;
        }
    }
    public bool blockT
    {
        get
        {
            return bTop > 0;
        }
    }

    // Use this for initialization
    void Start()
    {
        hm = GetComponent<CharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {      
        Move();
    }

    private float slow_r = 0.66f;
    Vector2 conv(WalkDirections wd)
    {
        switch(wd)
        {
            case WalkDirections.up:
                return Vector2.up;
            case WalkDirections.down:
                return Vector2.down;
            case WalkDirections.left:
                return Vector2.left;
            case WalkDirections.right:
                return Vector2.right;
            case WalkDirections.right_down:
                return Vector2.right * slow_r + Vector2.down * slow_r;
            case WalkDirections.right_up:
                return Vector2.right * slow_r + Vector2.up * slow_r;
            case WalkDirections.left_up:
                return Vector2.left * slow_r + Vector2.up * slow_r;
            case WalkDirections.left_down:
                return Vector2.left * slow_r + Vector2.down * slow_r;
        }
        return Vector2.zero;
    }

    void Move()
    {
        // Move object across XY plane
        if (hm.Movement != Vector2.zero)
        {
            Vector2 Movement = hm.Movement;
            
            if (blockB || blockT)
            {

                Movement.y = 0;
            }

            if (blockR)
            {
                if (Movement.x > 0)
                    Movement.x = 0;
            }
            else if (blockL)
            {
                if (Movement.x < 0)
                    Movement.x = 0;
            }
            Vector3 angels = transform.eulerAngles;
            transform.eulerAngles = Vector3.zero;
            transform.Translate(Movement.x * speed, Movement.y * speed, 0);
            transform.eulerAngles = angels;
            hm.Movement = Vector2.zero;
            hm.SetNextAnim(CharacterAnimation.Walk);
        }
        else if (hm.action == CharacterAnimation.Walk)
            hm.SetNextAnim(CharacterAnimation.idle);

    }

    void LateUpdate()
    {
        if (bButton > 0)
            bButton--;
        if (bTop > 0)
            bTop--;
        if (bRight > 0)
            bRight--;
        if (bLeft > 0)
            bLeft--;
    }  
}


