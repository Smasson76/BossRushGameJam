using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mesh2D))]
public class Mesh2DToolEditor : Editor {
    private Mesh2D mesh2D;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private int selectedIndex = -1;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };
    private void OnSceneGUI () {
        // The secelected spline
        mesh2D = target as Mesh2D;
		handleTransform = mesh2D.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;;
        
        for (int i = 0; i < mesh2D.verts.Length; i++) {
            ShowPoint(i);
        }

        for (int i = 0; i < mesh2D.lines.Length; i += 2) {
            Vector3 p0 = handleTransform.TransformPoint(mesh2D.verts[mesh2D.lines[i]]);
            Vector3 p1 = handleTransform.TransformPoint(mesh2D.verts[mesh2D.lines[i + 1]]);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
        }
    }

    private Vector3 ShowPoint(int index) {
		Vector3 point = handleTransform.TransformPoint(mesh2D.verts[index]);
		Handles.color = Color.green;
		if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
			Repaint();
		}
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, handleRotation);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(mesh2D, "Move Point");
				EditorUtility.SetDirty(mesh2D);
			    mesh2D.verts[index] = handleTransform.InverseTransformPoint(point);
			}
		}
		return point;
    }

    public override void OnInspectorGUI () {
		mesh2D = target as Mesh2D;
        if (selectedIndex >= 0 && selectedIndex < mesh2D.verts.Length) {
			DrawSelectedPointInspector();
		}
		if (GUILayout.Button("Reset")) {
			Undo.RecordObject(mesh2D, "Reset");
			mesh2D.Reset();
			EditorUtility.SetDirty(mesh2D);
		}
	}

    private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", mesh2D.verts[selectedIndex]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2D, "Move Point");
			EditorUtility.SetDirty(mesh2D);
			mesh2D.verts[selectedIndex] = point;
		}
	}
}
