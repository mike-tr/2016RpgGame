using UnityEngine;
using System.Collections;

public class HitBoxScriptC : MonoBehaviour
{

    private PlayerHitBox ph;
    public dir dirc;

    private Vector3 forceDir;
    void Start()
    {
        ph = transform.parent.GetComponent<PlayerHitBox>();

        switch(dirc)
        {
            case dir.buttom:
                forceDir = new Vector3(0,-1);
                break;
            case dir.left:
                forceDir = new Vector3(-1, 0);
                break;
            case dir.right:
                forceDir = new Vector3(1, 0);
                break;
            case dir.top:
                forceDir = new Vector3(0, 1);
                break;

            case dir.right_top:
                forceDir = new Vector3(0.5f, 0.5f);
                break;
            case dir.right_buttom:
                forceDir = new Vector3(0.5f, -0.5f);
                break;
            case dir.left_top:
                forceDir = new Vector3(-0.5f, 0.5f);
                break;
            case dir.left_buttom:
                forceDir = new Vector3(-0.5f, -0.5f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        ph.NewColl(coll.name, dirc);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        ph.CollExit(other.name, dirc);
    }
}
