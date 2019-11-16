using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamScript : MonoBehaviour {


    public float speed = 5;
	// Update is called once per frame
	void Update () {

        Vector3 rot = transform.eulerAngles;
        Vector3 rotn = rot;
        rotn.x = 0;
        transform.eulerAngles = rotn;
		if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        transform.eulerAngles = rot;
	}
}
