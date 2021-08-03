using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAlongSpline : MonoBehaviour {
    public Spline spline;
    public Mesh crossSection;
    public int resolution;
    private Mesh mesh;
    private GameObject meshHolder;

    Vector3[] verts;

    public void BuildMesh() {
        OrientedPoint[] points = spline.GetOrientedPoints(resolution);
        
        Vector3[] crossSectionVerts = crossSection.vertices;
        int[] crossSectionTris = crossSection.triangles;

        int vertCount = crossSectionVerts.Length * resolution;
        verts = new Vector3[vertCount];
        int triCount = crossSectionTris.Length * resolution;
        int[] tris = new int[triCount];
        Debug.Log("Verts Count = " + vertCount);

        for (int i = 0; i < resolution; i++) {
            for (int j = 0; j < crossSectionVerts.Length; j++) {
                verts[i * crossSectionVerts.Length + j] = points[i].LocalToWorld(crossSectionVerts[j]);
                tris[i * crossSectionTris.Length + j] = crossSectionTris[j];
            }

            for (int j = 0; j < crossSectionTris.Length; j++) {
                int triIndex = crossSectionTris[j] + i * (crossSectionTris.Length - 2);
                Debug.Log("I = " + i + ", J = " + j + ", tri index = " + triIndex);
                tris[i * crossSectionTris.Length + j] = triIndex;
            }
        }

        mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;


        if (meshHolder == null) {
            meshHolder = new GameObject("Mesh Holder");
        }
        MeshFilter meshFilter = meshHolder.GetComponent<MeshFilter>();
        if (meshFilter == null) {
            meshFilter = meshHolder.AddComponent<MeshFilter>();
        }
        MeshRenderer meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            meshRenderer = meshHolder.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = new Material(Shader.Find("Diffuse"));
        
        meshFilter.mesh = mesh;
    }
    void OnDrawGizmosSelected() {
        for (int i = 0; i < verts.Length; i++) {
            Gizmos.DrawSphere(verts[i], 0.03f);
        }
    }
}
