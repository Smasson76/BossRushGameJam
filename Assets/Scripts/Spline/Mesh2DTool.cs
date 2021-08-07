using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh2DTool : MonoBehaviour {
    public Mesh2D mesh2D;
    private Vector2[] metaVerts;
    private Mesh2DVertMode[] metaVertModes;
    private float[] us;
    private int[] metaLines;

    public void Reset() {
        metaVerts = new Vector2[] {
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(1,1),
            new Vector2(1,-1)
        };
        metaVertModes = new Mesh2DVertMode[] {
            Mesh2DVertMode.hard,
            Mesh2DVertMode.hard,
            Mesh2DVertMode.hard,
            Mesh2DVertMode.hard,
        };
        metaLines = new int[] {
            1, 0,
            2, 1,
            3, 2,
            0, 3,
        };
    }

    public Vector2[] GetMetaVerts() {
        return metaVerts;
    }
    public void SetMetaVert(int i, Vector2 pos) {
        metaVerts[i] = pos;
    }
    public void SetMetaVertMode(int i, Mesh2DVertMode mode) {
        metaVertModes[i] = mode;
    }
    public Mesh2DVertMode[] GetMetaVertModes() {
        return metaVertModes;
    }
    public int[] GetLines() {
        return metaLines;
    }

    public void BuildMesh2D() {
        // Count the number of verts in the 2D mesh, based on the number of hard and smooth metaverts
        int vertcount = 0;
        for (int i = 0; i < metaVertModes.Length; i++) {
            if (metaVertModes[i] == Mesh2DVertMode.hard) {
                vertcount += 2;
            } else {
                vertcount += 1;
            }
        }

        //Create the arrays for the new 2D mesh
        Vector2[] newVerts = new Vector2[vertcount];
        Vector2[] newNormals = new Vector2[vertcount];
        int[] newLines = new int[metaLines.Length];

        //Fill the arrays, splitting the metaverts when they're marked as hard edges
        int newVertIndex = 0;
        for (int i = 0; i < metaVerts.Length; i++) {
            if (metaVertModes[i] == Mesh2DVertMode.smooth) {
                newVerts[newVertIndex] = metaVerts[i];
                int normalInfluenceCount = 0;
                Vector2 normalInfluenceTotal = Vector2.zero;
                for (int j = 0; j < metaLines.Length; j += 2) {
                    if (metaLines[j] == i) {
                        newLines[j] = newVertIndex;
                        normalInfluenceCount++;
                        normalInfluenceTotal += GetLineNormal(j);
                    }
                    if (metaLines[j + 1] == i) {
                        newLines[j + 1] = newVertIndex;
                        normalInfluenceCount++;
                        normalInfluenceTotal += GetLineNormal(j);
                    }
                }
                newNormals[newVertIndex] = (normalInfluenceTotal / (float)normalInfluenceCount).normalized;
                newVertIndex++;
            } else {
                for (int j = 0; j < metaLines.Length; j += 2) {
                    if (metaLines[j] == i) {
                        newVerts[newVertIndex] = metaVerts[i];
                        newNormals[newVertIndex] = GetLineNormal(j);
                        newLines[j] = newVertIndex;
                        newVertIndex++;
                    }
                    if (metaLines[j + 1] == i) {
                        newVerts[newVertIndex] = metaVerts[i];
                        newNormals[newVertIndex] = GetLineNormal(j);
                        newLines[j + 1] = newVertIndex;
                        newVertIndex++;
                    }
                }
            }

            mesh2D.verts = newVerts;
            mesh2D.normals = newNormals;
            mesh2D.lines = newLines;

            mesh2D.metaVerts = metaVerts;
            mesh2D.metaLines = metaLines;
            mesh2D.metaVertModes = metaVertModes;
        }
    }
        
    public Vector2 GetLineNormal (int lineStartIndex) {
        Vector2 alongLine = metaVerts[metaLines[lineStartIndex]] - metaVerts[metaLines[lineStartIndex + 1]];
        return new Vector2(-alongLine.y, alongLine.x).normalized;
    } 

    public void LoadMesh2D () {
        if (mesh2D == null) {
            Debug.LogError("No mesh assigned!");
            return;
        }
        metaVerts = mesh2D.metaVerts;
        metaLines = mesh2D.metaLines;
        metaVertModes = mesh2D.metaVertModes;
    }
}


public enum Mesh2DVertMode {
    smooth,
    hard
}

