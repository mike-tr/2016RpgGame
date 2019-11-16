using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_script : MonoBehaviour {

    private float sp;

    public float speed
    {
        set
        {
            sp = Random.Range(value * 0.5f, value * 1.5f);
            if (sp > 300f)
                sp = 300f;
            else if (sp < 25f)
                sp = 25f;
        }
    }
   
	// Update is called once per frame
	void Update () {
        transform.position -= transform.right * sp * Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == name || coll.gameObject.name.Contains("attack"))
            return;
        
        
        StartCoroutine(Sleep_after(Time.deltaTime));
        
    }

    IEnumerator Sleep_after(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
