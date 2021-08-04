using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAlongSpline : MonoBehaviour {
    public Spline spline;
    public Mesh crossSection;
    public int resolution;
    private Mesh mesh;
    private GameObject meshHolder;

    public void BuildMesh() {
        OrientedPoint[] points = spline.GetOrientedPoints(resolution);
        
        Vector3[] crossSectionVerts = crossSection.vertices;
        Vector3[] crossSectionNormals = crossSection.normals;
        int[] crossSectionTris = crossSection.triangles;

        int vertCount = crossSectionVerts.Length * resolution;
        Vector3[] verts = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        int triCount = crossSectionTris.Length * resolution;
        int[] tris = new int[triCount];

        for (int i = 0; i < resolution; i++) {
            for (int j = 0; j < crossSectionVerts.Length; j++) {
                verts[i * crossSectionVerts.Length + j] = points[i].LocalToWorld(crossSectionVerts[j]);
                normals[i * crossSectionNormals.Length + j] = points[i].LocalToWorldDirection(crossSectionNormals[j]);
            }

            for (int j = 0; j < crossSectionTris.Length; j++) {
                int triIndex = crossSectionTris[j] + i * (crossSectionTris.Length - 2);
                tris[i * crossSectionTris.Length + j] = triIndex;
            }
        }

        mesh = new Mesh();
        mesh.vertices = verts;
        mesh.normals = normals;
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
}
