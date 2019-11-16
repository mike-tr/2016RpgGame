using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_dir : MonoBehaviour {

    private CharacterControl cc;
    private CharacterDirection cd;

    private C_Stats stats;
	// Use this for initialization
	void Start () {
        cc = transform.parent.GetComponent<CharacterControl>();
        stats = transform.parent.GetComponent<AddItem>().stats;
	}
	
	// Update is called once per frame
	void Update () {
		if(cd != cc.direction)
        {
            cd = cc.direction;
            Vector3 dir = CharacterAnimations.GetDirection(cd);

            //transform.eulerAngles = dir * 90;
            transform.localPosition = dir * 2 + dir * stats.range * 6 - 2 * Vector3.up;
        }
	}
}
