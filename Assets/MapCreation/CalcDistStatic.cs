using UnityEngine;

[ExecuteInEditMode]
public class CalcDistStatic : MonoBehaviour {

	// Use this for initialization
	Vector3 newPos;
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
	void Start () {
		newPos = transform.position;
		if(!GetDist(Vector3.forward, Vector3.forward * 10))
			GetDist(Vector3.forward, -Vector3.forward * 1000); 
		transform.position = newPos;
		
		if(Application.isPlaying)
			Destroy(this);
	}
}
