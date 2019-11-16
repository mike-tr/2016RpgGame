using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(TGMap))]
public class TGMapInspector : Editor {

	public override void  OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Regenerate"))
		{
			TGMap tileMap = (TGMap)target;
			tileMap.BuildMesh();			
		}
	}
}
