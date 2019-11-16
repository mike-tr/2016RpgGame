using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject prefab;
	// Use this for initialization
    public void Spawn()
    {
        Transform child = Instantiate(prefab).transform;
        Vector3 p = Vector3.zero;
        p.x += Random.Range(-300, 300);
        p.y += Random.Range(-300, 300);
        child.position = p;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Spawn();
        }
    }
}
