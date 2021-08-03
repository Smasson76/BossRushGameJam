using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor {
    private Spline spline;
    private const int stepsPerCurve = 10;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private int selectedIndex = -1;
    public bool showDirections;
    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };
    private void OnSceneGUI () {
        // The secelected spline
        spline = target as Spline;
		handleTransform = spline.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;;
        
        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.ControlPointCount; i +=3) {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Vector3 lineStart = spline.GetPoint(0f);
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        if (showDirections) {
            ShowDirections();
        }
    }

    private Vector3 ShowPoint(int index) {
		Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
		Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
		if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)) {
			selectedIndex = index;
			Repaint();
		}
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, handleRotation);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(spline, "Move Point");
				EditorUtility.SetDirty(spline);
				spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
			}
		}
		return point;
    }

    private void ShowDirections () {
        Vector3 point = spline.GetPoint(0);
        Handles.color = Color.red;
        Handles.DrawLine(point, point + spline.GetTangent(0));
        Handles.color = Color.green;
        Handles.DrawLine(point, point + spline.GetNormal(0));
        int steps = stepsPerCurve * spline.CurveCount;
        for (int i = 1; i <= steps; i++) {
            point = spline.GetPoint(i / (float)steps);
            Handles.color = Color.red;
            Handles.DrawLine(point, point + spline.GetTangent(i / (float)steps));
            Handles.color = Color.green;
            Handles.DrawLine(point, point + spline.GetNormal(i / (float)steps));
        }
    }

    public override void OnInspectorGUI () {
        bool showDirectionsOld = showDirections;
        showDirections = EditorGUILayout.Toggle("Show Curve Directions", showDirections);
        if (showDirectionsOld != showDirections) {
            SceneView.RepaintAll();
        }
		spline = target as Spline;
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount) {
			DrawSelectedPointInspector();
		}
		if (GUILayout.Button("Add Curve")) {
			Undo.RecordObject(spline, "Add Curve");
			spline.AddCurve();
			EditorUtility.SetDirty(spline);
		}
		if (GUILayout.Button("Reset")) {
			Undo.RecordObject(spline, "Reset");
			spline.Reset();
			EditorUtility.SetDirty(spline);
		}
	}

    private void DrawSelectedPointInspector() {
		GUILayout.Label("Selected Point");
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Move Point");
			EditorUtility.SetDirty(spline);
			spline.SetControlPoint(selectedIndex, point);
		}
        EditorGUI.BeginChangeCheck();
		BezierControlPointMode mode = (BezierControlPointMode) EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(spline, "Change Point Mode");
			spline.SetControlPointMode(selectedIndex, mode);
			EditorUtility.SetDirty(spline);
		}
	}
}
