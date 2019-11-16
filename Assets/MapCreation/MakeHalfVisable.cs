using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MakeHalfVisable : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        Transform tp = new GameObject().transform;
        tp.SetParent(this.transform);
        tp.localPosition = Vector3.zero;
        tp.localScale = Vector3.one;
        tp.localEulerAngles = Vector3.zero;

        SpriteRenderer sr = tp.gameObject.AddComponent<SpriteRenderer>();
        SpriteRenderer my = GetComponent<SpriteRenderer>();

        sr.sortingLayerName = "all";

        sr.sprite = my.sprite;
        sr.color = new Color(my.color.r, my.color.g, my.color.b, 0.5f);
	}
	
}
