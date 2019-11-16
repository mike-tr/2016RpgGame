using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHitBox : MonoBehaviour {

    private Vector3 ApplyForce = Vector3.zero;
    private Transform WorldView;
    private MoveCharacter hm;
  
	// Use this for initialization
	void Start () {

        transform.name = "playerMe";
        hm = transform.parent.GetComponent<MoveCharacter>();
        WorldView = hm.transform.parent;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        WorldView.position += ApplyForce;
        ApplyForce *= 0.95f;
        if (Vector3.Distance(ApplyForce, Vector3.zero) < 0.1f)
            ApplyForce = Vector3.zero;

    }

    public void NewColl(string collName, dir CollDir, Vector3 AddForce = default(Vector3))
    {
        try
        {
            hm.BlocckOrUnblockDir(CollDir,1);
            ApplyForce += AddForce;      
        }
        catch
        {

        }      
    }

    public void CollExit(string collName, dir CollDir)
    {
        try
        {
            hm.BlocckOrUnblockDir(CollDir, -1);
        }
        catch
        {

        }
    }

}

public enum dir
{
    right,
    left,
    top,
    buttom,
    right_top,
    left_top,
    right_buttom,
    left_buttom,
    None
}