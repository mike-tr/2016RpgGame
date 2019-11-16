using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	// Use this for initialization

	public Transform FollowTarget;
	Vector3 targetPos;
	public float lerp_speed = 5;
	public float yd = 100;
	public float DistZ = 200;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = FollowTarget.position;
		targetPos.z -= DistZ;
		targetPos.y -= yd;
		transform.position = Vector3.Lerp(transform.position, targetPos, lerp_speed * Time.deltaTime);
	}
}
