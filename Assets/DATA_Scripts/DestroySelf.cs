using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DestroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Application.isPlaying)
		{
			Destroy(this.gameObject);
		}
		else
		{
			DestroyImmediate(this.gameObject);
		}
    }
}
