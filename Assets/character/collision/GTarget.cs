using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTarget : MonoBehaviour {

	// Use this for initialization
	private CharacterControl cc;
	void Start () {
		cc = transform.parent.parent.GetComponent<CharacterControl>();
		gameObject.SetActive(false);
	}
	public void SetTarget(Transform t)
	{
		if(t == cc.transform)
			return;
		cc.target = t;

        print("got target!");
	}
}
