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
	private List<int> selectedPointIndexes = new List<int>();
	private bool showNormals;
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


		if (selectedPointIndexes.Count > 1) {
			EditorGUI.BeginChangeCheck();
			Vector3 selectedTotal = Vector3.zero;
			int selectedCount = 0;
			foreach(int i in selectedPointIndexes) {
				Vector2 point2D = metaVerts[i];
				selectedTotal += new Vector3(point2D.x, point2D.y);
				selectedCount++;
			}
			Vector3 avgPos = selectedTotal / (float)selectedCount;
			Vector3 point = Handles.DoPositionHandle(avgPos, handleRotation);
			Vector3 pointDelta = point - avgPos;
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(mesh2DTool, "Move Point");
				EditorUtility.SetDirty(mesh2DTool);
				foreach(int i in selectedPointIndexes) {
					Vector2 point2D = metaVerts[i];
			    	mesh2DTool.SetMetaVert(i, handleTransform.InverseTransformPoint(new Vector3(point2D.x, point2D.y) + pointDelta));
				}
			}
		}

        for (int i = 0; i < lines.Length; i += 2) {
            Vector3 p0 = handleTransform.TransformPoint(metaVerts[lines[i]]);
            Vector3 p1 = handleTransform.TransformPoint(metaVerts[lines[i + 1]]);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
			if (showNormals) {
				Handles.color = Color.red;
				Vector2 lineCenter = (p0 + p1) / 2;
				Handles.DrawLine(lineCenter, lineCenter + mesh2DTool.GetLineNormal(i) * 0.3f);
			}
        }
    }

    private Vector3 ShowPoint(int index) {
		Vector3 point = handleTransform.TransformPoint(mesh2DTool.GetMetaVerts()[index]);
		Handles.color = modeColors[(int)mesh2DTool.GetMetaVertModes()[index]];
		if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap)) {
			if (!selectedPointIndexes.Contains(index)) {
				if (selectedPointIndexes.Count > 0 && Event.current.shift) {
					selectedPointIndexes.Add(index);
					Repaint();
				} else {
					SetSelection(index);
					Repaint();

				}
			} else {
				if (Event.current.shift) {
					Debug.Log("test");
					selectedPointIndexes.Remove(index);
					Repaint();
				} else {
					SetSelection(index);
					Repaint();
				}
			}
		}
		if (selectedPointIndexes.Count == 1 && selectedPointIndexes.Contains(index)) {
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
		GUILayout.Label("--------------------Mesh Data--------------------", EditorStyles.centeredGreyMiniLabel);
		DrawDefaultInspector();
		GUILayout.Space(20);

		GUILayout.Label("--------------------Selection options--------------------", EditorStyles.centeredGreyMiniLabel);
		mesh2DTool = target as Mesh2DTool;
		switch (selectedPointIndexes.Count) {
			case 0:
				DrawNoSelectionInspector();
				break;
			case 1:
				DrawSelectedPointInspector();
				break;
			case 2:
				DrawTwoSelectedPointInspector();
				break;
			default:
				break;
		}
		GUILayout.Space(20);

		GUILayout.Label("--------------------Mesh options--------------------", EditorStyles.centeredGreyMiniLabel);
		bool showDirectionsOld = showNormals;
		showNormals = EditorGUILayout.Toggle("Show Line Normals", showNormals);
        if (showDirectionsOld != showNormals) {
            SceneView.RepaintAll();
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
			selectedPointIndexes = new List<int>();
			EditorUtility.SetDirty(mesh2DTool);
		}
	}

	private void DrawNoSelectionInspector() {
		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button("Add Point")) {
			Undo.RecordObject(mesh2DTool, "Add point");
			selectedPointIndexes.Add(mesh2DTool.AddPoint(Vector3.zero));
			EditorUtility.SetDirty(mesh2DTool);
		}
	}

    private void DrawSelectedPointInspector() {
		int index = selectedPointIndexes[0];
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", mesh2DTool.GetMetaVerts()[index]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2DTool, "Move Point");
			mesh2DTool.SetMetaVert(index, point);
			EditorUtility.SetDirty(mesh2DTool);
		}
		EditorGUI.BeginChangeCheck();
		float val = EditorGUILayout.FloatField("U-Coord:",mesh2DTool.GetUs()[index]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2DTool, "Move Point");
			mesh2DTool.SetU(index, val);
			EditorUtility.SetDirty(mesh2DTool);
		}
		EditorGUI.BeginChangeCheck();
		Mesh2DVertMode mode = (Mesh2DVertMode) EditorGUILayout.EnumPopup("Mode", mesh2DTool.GetMetaVertModes()[index]);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(mesh2DTool, "Change Point Mode");
			mesh2DTool.SetMetaVertMode(index, mode);
			EditorUtility.SetDirty(mesh2DTool);
		}
		if (GUILayout.Button("Delete Point")) {
			Undo.RecordObject(mesh2DTool, "Delete Point");
			mesh2DTool.DeletePoint(index);
			selectedPointIndexes = new List<int>();
			EditorUtility.SetDirty(mesh2DTool);
		}
	}

	private void DrawTwoSelectedPointInspector() {
		int p0Index = selectedPointIndexes[0];
		int p1Index = selectedPointIndexes[1];
		int lineIndex = mesh2DTool.GetLineIndexFromPoints(p0Index, p1Index);
		if (lineIndex == -1) {
			if (GUILayout.Button("Add Line")) {
				Undo.RecordObject(mesh2DTool, "Add Line");
				mesh2DTool.AddLine(p0Index, p1Index);
				EditorUtility.SetDirty(mesh2DTool);
			}
		} else {
			if (GUILayout.Button("Flip Line Normal")) {
				Undo.RecordObject(mesh2DTool, "Flip Line Normal");
				mesh2DTool.FlipNormal(lineIndex);
				EditorUtility.SetDirty(mesh2DTool);
			}
			if (GUILayout.Button("Bisect Line")) {
				Undo.RecordObject(mesh2DTool, "Bisect Line");
				SetSelection(mesh2DTool.BisectLine(lineIndex));
				EditorUtility.SetDirty(mesh2DTool);
			}
			if (GUILayout.Button("Delete Line")) {
				Undo.RecordObject(mesh2DTool, "Delete Line");
				mesh2DTool.DeleteLine(lineIndex);
				EditorUtility.SetDirty(mesh2DTool);
			}
		}
	}

	private void SetSelection(int index) {
		selectedPointIndexes = new List<int>();
		selectedPointIndexes.Add(index);
	}
}
