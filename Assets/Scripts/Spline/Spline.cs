using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spline : MonoBehaviour {
    [SerializeField] private Vector3[] points;
    [SerializeField] private BezierControlPointMode[] modes;

    public void Reset () {
        points = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(10, 0, 0),
            new Vector3(0, 0, 10),
            new Vector3(10, 0, 10),
        };
        modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
    }

    public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;
        Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(points.Length - 4);
    }

    public Vector3 GetPoint (float t) {
        int i = SplineToCurveT(ref t);
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}

    public Vector3 GetTangent (float t) {
        int i = SplineToCurveT(ref t);
		return Bezier.GetTangent(points[i], points[i + 1], points[i + 2], points[i + 3], t);
	}

    public Vector3 GetNormal (float t) {
        int i = SplineToCurveT(ref t);
		return Bezier.GetNormal(points[i], points[i + 1], points[i + 2], points[i + 3], t);
	}

    public Quaternion GetOrientation (float t) {
        Vector3 tangent = GetTangent(t);
        Vector3 normal = GetNormal(t);
        return Quaternion.LookRotation(tangent, normal);
    }

    public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}

    private int SplineToCurveT(ref float t) {
        int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
        return i;
    }

    // Public access to the points
    public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
        if (index % 3 == 0) {
			Vector3 delta = point - points[index];
			if (index > 0) {
				points[index - 1] += delta;
			}
			if (index + 1 < points.Length) {
				points[index + 1] += delta;
			}
		}
		points[index] = point;
        EnforceMode(index);
	}

    //Managing the control point modes
    public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		modes[(index + 1) / 3] = mode;
	}

    private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1) {
			return;
		}
        int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			enforcedIndex = middleIndex + 1;
		}
		else {
			fixedIndex = middleIndex + 1;
			enforcedIndex = middleIndex - 1;
		}
		Vector3 middle = points[middleIndex];
		Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
		}
		points[enforcedIndex] = middle + enforcedTangent;
	}

    public OrientedPoint[] GetOrientedPoints(int count) {
        OrientedPoint[] arr = new OrientedPoint[count];
        for (int i = 0; i < count; i++) {
            float t = i / (float)(count - 1);
            Vector3 pos = this.transform.InverseTransformPoint(GetPoint(t));
            Quaternion rot = GetOrientation(t);
            arr[i] = new OrientedPoint(pos, rot);
        }
        return arr;
    }
}

// Enum for the mode of a control point
public enum BezierControlPointMode {
    Free,
    Aligned,
    Mirrored
}

// Getting values on a curve:
public static class Bezier {
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		float omt = 1 -t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return  p0 * (omt2 * omt) +
                p1 * (3 * omt2 * t) +
                p2 * (3 * omt * t2) +
                p3 * (t2 * t);
	}

    public static Vector3 GetTangent (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		float omt = 1 - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =   p0 * (-omt2) +
                            p1 * (3 * omt2  - 2 * omt) +
                            p2 * (-3 * t2 + 2 * t) +
                            p3 * (t2);
        return tangent.normalized;
	}

    public static Vector3 GetNormal(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        Vector3 tangent = GetTangent(p0, p1, p2, p3, t);
        Vector3 binormal = Vector3.Cross(Vector3.up, tangent).normalized;
        return Vector3.Cross(tangent, binormal).normalized;
    }
}

public class OrientedPoint {
    public Vector3 position;
    public Quaternion rotation;
    public OrientedPoint (Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }

    public Vector3 LocalToWorld (Vector3 point) {
        return position + rotation * point;
    }

    public Vector3 WorldToLocal (Vector3 point) {
        return Quaternion.Inverse(rotation) * (position - position);
    }

    public Vector3 LocalToWorldDirection (Vector3 dir) {
        return rotation * dir;
    }
}