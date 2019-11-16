using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDist : MonoBehaviour {

	// Use this for initialization
	Vector3 newPos;

	void Start()
	{
		print("owner name " + transform.name);
	}
	bool GetDist(Vector3 dir, Vector3 ds)
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position + ds, dir, out hit))
		{
            newPos.z += hit.distance + ds.z;
			return true;
		}
		return false;
	}
	void Update () {
		newPos = transform.position;
		if(!GetDist(Vector3.forward, Vector3.zero))
			GetDist(Vector3.forward, -Vector3.forward * 1000); 
		transform.position = Vector3.Lerp(transform.position, newPos, 5 * Time.deltaTime);
	}
}
