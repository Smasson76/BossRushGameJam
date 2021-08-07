using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mesh2DTool))]
public class Mesh2DToolEditor : Editor {
    private Mesh2DTool mesh2DTool;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private int selectedIndex = -1;
    private static Color[] modeColors = {
        Color.cyan,
        Color.magenta
    };
    private void OnSceneGUI () {
        // The secelected spline
        mesh2DTool = target as Mesh2DTool;
		handleTransform = mesh2DTool.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;;

		Vector2[] metaVerts = mesh2DTool.GetMetaVerts();
		int[] lines = mesh2DTool.GetLines();
        
        for (int i = 0; i < metaVerts.Length; i++) {
            ShowPoint(i);
        }

        for (int i = 0; i < lines.Length; i += 2) {
            Vector3 p0 = handleTransform.TransformPoint(metaVerts[lines[i]]);
            Vector3 p1 = handleTransform.TransformPoint(metaVerts[lines[i + 1]]);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
        }
    }

    private Vector3 ShowPoint(int index) {
		Vector3 point = handleTransform.TransformPoint(mesh2DTool.GetMetaVerts()[index]);
		Handles.color = modeColors[(int)mesh2DTool.GetMetaVertModes()[index]];
		if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
			Repaint();
		}
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, handleRotation);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(mesh2DTool, "Move Point");
				EditorUtility.SetDirty(mesh2DTool);
			    mesh2DTool.SetMetaVert(index, handleTransform.InverseTransformPoint(point));
			}
		}
		return point;
    }

    public override void OnInspectorGUI () {
		DrawDefaultInspector();
		mesh2DTool = target as Mesh2DTool;
        if (selectedIndex >= 0 && selectedIndex < mesh2DTool.GetMetaVerts().Length) {
			DrawSelectedPointInspector();
		}
		if (GUILayout.Button("Load Mesh2D data")) {
			Undo.RecordObject(mesh2DTool, "BuildMesh");
			mesh2DTool.LoadMesh2D();
			EditorUtility.SetDirty(mesh2DTool);
		}
		if (GUILayout.Button("Write to Mesh2D data")) {
			Undo.RecordObject(mesh2DTool, "BuildMesh");
			mesh2DTool.BuildMesh2D();
			EditorUtility.SetDirty(mesh2DTool);
		}
		if (GUILayout.Button("Reset")) {
			Undo.RecordObject(mesh2DTool, "Reset");
			mesh2DTool.Reset();
			EditorUtility.SetDirty(mesh2DTool);
		}
	}

    private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", mesh2DTool.GetMetaVerts()[selectedIndex]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2DTool, "Move Point");
			EditorUtility.SetDirty(mesh2DTool);
			mesh2DTool.SetMetaVert(selectedIndex, point);
		}
		EditorGUI.BeginChangeCheck();
		Mesh2DVertMode mode = (Mesh2DVertMode) EditorGUILayout.EnumPopup("Mode", mesh2DTool.GetMetaVertModes()[selectedIndex]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2DTool, "Change Point Mode");
			mesh2DTool.SetMetaVertMode(selectedIndex, mode);
			EditorUtility.SetDirty(mesh2DTool);
		}
	}
}
