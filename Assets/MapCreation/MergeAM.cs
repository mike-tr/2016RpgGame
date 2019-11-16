using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeAM : MonoBehaviour {

	// Use this for initialization

	public bool run = false;
	IEnumerator Start () {
		while(!Grid_creator.instance.Loaded)
			yield return new WaitForSeconds(Time.deltaTime * 5);
		run = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!run)
			return;

		if(Input.GetMouseButtonDown(0))
		{
			node n = Grid_creator.instance.NodeFromWorldPoint(Vector3.zero);
			TDTile t = TGMap.instance.TileFromWorldPoint(Vector3.zero);
			print(n.GridX + " ," + n.GridY + " // " + t.x + " ," + t.y);
		}
	}
}
