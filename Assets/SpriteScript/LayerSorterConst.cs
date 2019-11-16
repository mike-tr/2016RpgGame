using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorterConst : MonoBehaviour {

    SpriteRenderer sr;
    public bool ForceLayer = true;
    CalcLayer layerc;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        if(ForceLayer)
            sr.sortingLayerName = "all";
        layerc = new CalcLayer(transform.eulerAngles.z);
	}
	
	// Update is called once per frame
	void Update () {
        sr.sortingOrder = layerc.GetSortingLayer(transform.position);   
    }
}

public class CalcLayer
{
    float divx;
    float divy;
    public CalcLayer(float rot_z)
    {
        divx = rot_z / 180;
        divy = 1 - divx;
    }

    float dvx = 0;
    float dvy = 0;

    public int GetSortingLayer(Vector3 pos)
    {
        TDTile t = TGMap.instance.TileFromWorldPoint(pos);
        return 32767 - Mathf.Abs((t.x * -20) + (t.y * 30 * TGMap.instance.size_x)); 
    }
}
