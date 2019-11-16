using UnityEngine;
using System.Collections;

public class CCollision_S : MonoBehaviour
{

    private Vector3 ApplyForce = Vector3.zero;
    private Transform WorldView;
    private MoveCharacter hm;
    private Health player;
    private CharacterControl cc;
    public GetPath mv;
    // Use this for initialization
    int id;
    IEnumerator Start()
    {
        transform.name = "playerMe";
        hm = transform.parent.parent.GetComponent<MoveCharacter>();
        WorldView = transform.parent.parent;
        player = hm.transform.GetComponent<Health>();
        cc = hm.transform.GetComponent<CharacterControl>();

        while (!cc.Intialized)
            yield return new WaitForSeconds(Time.deltaTime);
        id = cc.player_id;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Attack")
        {
            
            Attack_data ad = coll.transform.GetComponent<Attack_data>();
            
            if (hm.transform == ad.caster.transform)
                return;
            player.ApplyDmg(ad.DMG(), ad.type);

            if (cc.target == null && mv.NotMoving)
                cc.target = ad.caster;
        }
        else if (coll.transform.tag == "SearchForPlayer")
        {
            GTarget gt = coll.transform.GetComponent<GTarget>();
            gt.SetTarget(hm.transform);
        }
    }

    
    short k = 3;
    short b = 0;
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Attack")
        {
            if (coll.transform.name.Contains("A" + id))
                return;
            else
            {
                b++;
                if (b > 25)
                {
                    b = 0;
                    if (Random.Range(0, 15) > 13)
                        return;
                }
                else
                    return;
            }
        }
        else if (coll.transform.tag == "character_box")
            return;
        else if (coll.transform.tag == "SearchForPlayer")
            return;

        Vector3 dirc = (coll.gameObject.transform.position - gameObject.transform.position).normalized;
        

        if (dirc.y < -0.61f)
        {
            hm.BlocckOrUnblockDir(dir.buttom, k);
        }
        else if (Mathf.Abs(dirc.x) > Mathf.Abs(dirc.y))
        {
            if (dirc.x > 0)
            {
                hm.BlocckOrUnblockDir(dir.right, k);
            }
            else
            {
                hm.BlocckOrUnblockDir(dir.left, k);
            }

        }
        else if (Mathf.Abs(dirc.x) < Mathf.Abs(dirc.y))
        {
            if (dirc.y > 0)
            {
                hm.BlocckOrUnblockDir(dir.top, k);
            }
            else
            {
                hm.BlocckOrUnblockDir(dir.buttom, k);
            }
        }
    }
}
