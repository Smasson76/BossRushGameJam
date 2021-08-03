using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshAlongSpline))]
public class MeshAlongSplineEditor : Editor {
    private MeshAlongSpline meshAlongSpline;
    public override void OnInspectorGUI () {
        meshAlongSpline = target as MeshAlongSpline;
        DrawDefaultInspector();
		if (GUILayout.Button("Build Mesh")) {
            Debug.Log("Building Mesh");
            meshAlongSpline.BuildMesh();
		}
    }
}
