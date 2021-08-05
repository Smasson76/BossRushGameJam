using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAlongSpline : MonoBehaviour {
    public Spline spline;
    public Mesh2D crossSection;
    public int resolution;
    private Mesh mesh;
    private GameObject meshHolder;

    public void BuildMesh() {
        OrientedPoint[] points = spline.GetOrientedPoints(resolution);
        
        int segments = resolution - 1;
        int sectionVertCount = crossSection.verts.Length;
        int vertCount = resolution * sectionVertCount;
        int tricount = resolution * crossSection.lines.Length;

        Vector3[] verts = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] tris = new int[tricount * 3];

        for (int i = 0; i < resolution; i++) {
            int offset = i * sectionVertCount;
            for (int j = 0; j < sectionVertCount; j++) {
                int id = offset + j;
                verts[id] = points[i].LocalToWorld(new Vector3(crossSection.verts[j].x, crossSection.verts[j].y, 0));
                normals[id] = points[i].LocalToWorldDirection(new Vector3(crossSection.normals[j].x, crossSection.normals[j].y, 0));
                uvs[id] = new Vector2(crossSection.us[j], i / (float)resolution);
            }
        }
        int triIndex = 0;
        for (int i = 0; i < segments; i++) {
            int offset = i * sectionVertCount;
            for (int j = 0; j < crossSection.lines.Length; j += 2) {
                int a = offset + crossSection.lines[j] + sectionVertCount;
                int b = offset + crossSection.lines[j];
                int c = offset + crossSection.lines[j + 1];
                int d = offset + crossSection.lines[j + 1] + sectionVertCount;
                tris[triIndex] = a; triIndex++;
                tris[triIndex] = b; triIndex++;
                tris[triIndex] = c; triIndex++;
                tris[triIndex] = c; triIndex++;
                tris[triIndex] = d; triIndex++;
                tris[triIndex] = a; triIndex++;
            } 
        }

        mesh = new Mesh();
        mesh.vertices = verts;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = tris;


        if (meshHolder == null) {
            if (transform.Find("Mesh") == null) {
                meshHolder = new GameObject("Mesh");
                meshHolder.transform.SetParent(this.transform);
            } else {
                meshHolder = transform.Find("Mesh").gameObject;
            }
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
