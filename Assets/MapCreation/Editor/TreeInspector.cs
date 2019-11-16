using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(GenerateTree))]
public class TreeInspector : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate"))
        {
            GenerateTree Gent = (GenerateTree)target;
            Gent.BuildMesh();
        }
    }
}
