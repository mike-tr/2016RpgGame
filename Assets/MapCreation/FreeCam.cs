using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour {

	public float dist = 200;
	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	Vector3 rot;
	// Update is called once per frame
	void Update () {
		rot = transform.eulerAngles;
		Vector3 r2 = rot;
		r2.x = 0;
		transform.eulerAngles = r2;
		if(Input.GetKey(KeyCode.A))
		{
			transform.position -= transform.right * speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.Q))
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.E))
		{
			transform.position -= transform.forward * speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.W))
		{
			transform.position += transform.up * speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.up * speed * Time.deltaTime;
		}
		transform.eulerAngles = rot;
	}
}
