using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGround : MonoBehaviour {

	// Use this for initialization
	public Transform cam;
	private Vector3 pos
	{
		get
		{
			return cam.position / 2;
		}
	}
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(pos.x % 16, pos.y % 16, pos.z);
	}
}
