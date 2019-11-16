using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TGMap))]
public class TileMapMouse : MonoBehaviour {

	// Use this for initialization
	TGMap _tileMap;
	Collider coll;
	Vector2 v = Vector2.one;
	Vector2 CurrentTileNode;
	IEnumerator Start () {
		coll = GetComponent<Collider>();
		_tileMap = GetComponent<TGMap>();

		while(Grid_creator.instance == null || !Grid_creator.instance.Loaded)
			yield return new WaitForSeconds(5 * Time.deltaTime);
		v *= Grid_creator.instance.node_radius;
	}
	
	// Update is called once per frame
	node n;
	TDTile t;
	void LateUpdate () {
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;

			if(coll.Raycast(ray, out hitInfo, Mathf.Infinity))
			{
				//n = Grid_creator.Global_Grid.NodeFromWorldPoint(hitInfo.point);
				//t = TGMap.Global_TGMap.TileFromWorldPoint(hitInfo.point);
				
				if(!OnKeyBoard_ui.Pause)
					GetMovPoint.Main_Character.SetMovePoint(new MovePos( true ,hitInfo.point));
			}
			
		}
	}
	void OnDrawGizmos()
    {
        if (t == null || n == null)
            return;

		Color c = (!n.walkable) ? new Color(1 , 0, 0, 0.5f) : new Color(0, 0 ,0, 0.5f);



		if(t.IsWater)
		{
			c.b = 1f - c.r * 0.8f;
		}
		else if(t.IsMountain && t.Height > 3)
		{
			c.r = 0.2f + c.r * 0.5f;
			c.g += 0.2f - c.r * 0.8f;
			c.b += 0.2f - c.r * 0.8f;
		}
		else
		{
			c.g = 1f - c.r * 0.8f;
		}
		
		Gizmos.color = c;
		Gizmos.DrawCube(n.pos, Vector3.one * (Grid_creator.instance.node_radius * 2));
    }

}
