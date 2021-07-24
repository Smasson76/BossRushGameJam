using UnityEngine;
public static class UtilityFunctions {
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Quaternion ShortestRotation(Quaternion a, Quaternion b) {
        if (Quaternion.Dot(a, b) < 0) {
            return a * Quaternion.Inverse(QuaternionMultiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }

    public static Quaternion QuaternionMultiply(Quaternion input, float scalar) {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    public static Vector3 VectorTo2D(Vector3 input) {
        return new Vector3(input.x, 0, input.z);
    }
}
