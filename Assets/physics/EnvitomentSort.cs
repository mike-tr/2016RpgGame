using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvitomentSort : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Dictionary<string, int> names = new Dictionary<string, int>();

        foreach(Transform t in transform)
        {
            try
            {
                names.Add(t.name,1);
            }
            catch
            {
                names[t.name] += 1; 
            }
            t.name = t.name + "_" + names[t.name];
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
