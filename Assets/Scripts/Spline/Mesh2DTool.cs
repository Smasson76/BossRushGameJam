using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mesh2DTool : MonoBehaviour {
    public Mesh2D mesh2D;
    [SerializeField] private Vector2[] metaVerts;
    [SerializeField] private Mesh2DVertMode[] metaVertModes;
    [SerializeField] private float[] metaUs;
    [SerializeField] private int[] metaLines;

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
    public float[] GetUs() {
        return metaUs;
    }
    public void SetU(int index, float val) {
        metaUs[index] = val;
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
                for (int j = 0; j < metaLines.Length; j++) {
                    if (metaLines[j] == i) {
                        vertcount++;
                    }
                }
            } else {
                vertcount++;
            }
        }

        //Create the arrays for the new 2D mesh
        Vector2[] newVerts = new Vector2[vertcount];
        float[] newUs = new float[vertcount];
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
                newUs[newVertIndex] = metaUs[i];
                newVertIndex++;
            } else {
                for (int j = 0; j < metaLines.Length; j += 2) {
                    if (metaLines[j] == i) {
                        newVerts[newVertIndex] = metaVerts[i];
                        newNormals[newVertIndex] = GetLineNormal(j);
                        newUs[newVertIndex] = metaUs[i];
                        newLines[j] = newVertIndex;
                        newVertIndex++;
                    }
                    if (metaLines[j + 1] == i) {
                        newVerts[newVertIndex] = metaVerts[i];
                        newNormals[newVertIndex] = GetLineNormal(j);
                        newUs[newVertIndex] = metaUs[i];
                        newLines[j + 1] = newVertIndex;
                        newVertIndex++;
                    }
                }
            }

            mesh2D.verts = newVerts;
            mesh2D.normals = newNormals;
            mesh2D.us = newUs;
            mesh2D.lines = newLines;

            mesh2D.metaVerts = metaVerts;
            mesh2D.metaVertModes = metaVertModes;
            mesh2D.metaUs = metaUs;
            mesh2D.metaLines = metaLines;
        }
    }
        
    public Vector2 GetLineNormal(int lineStartIndex) {
        Vector2 alongLine = metaVerts[metaLines[lineStartIndex]] - metaVerts[metaLines[lineStartIndex + 1]];
        return new Vector2(-alongLine.y, alongLine.x).normalized;
    } 

    public void LoadMesh2D() {
        if (mesh2D == null) {
            Debug.LogError("No mesh assigned!");
            return;
        }
        metaVerts = mesh2D.metaVerts;
        metaVertModes = mesh2D.metaVertModes;
        metaUs = mesh2D.metaUs;
        metaLines = mesh2D.metaLines;
    }

    public int AddPoint(Vector2 location) {
        int newSize = metaVerts.Length + 1;
        Array.Resize(ref metaVerts, newSize);
        Array.Resize(ref metaVertModes, newSize);
        Array.Resize(ref metaUs, newSize);
        metaVerts[newSize - 1] = location;
        return newSize - 1;
    }

    public void AddLine(int p0index, int p1index) {
        Array.Resize(ref metaLines, metaLines.Length + 2);
        metaLines[metaLines.Length - 2] = p0index;
        metaLines[metaLines.Length - 1] = p1index;
    }

    public void FlipNormal(int lineIndex) {
        int oldLower = metaLines[lineIndex];
        metaLines[lineIndex] = metaLines[lineIndex + 1];
        metaLines[lineIndex + 1] = oldLower;
    }

    public int GetLineIndexFromPoints(int p0index, int p1index) {
        for (int i = 0; i < metaLines.Length; i += 2) {
            if (p0index ==  metaLines[i]) {
                if (p1index == metaLines[i + 1]) {
                    return i;
                }
            }
            if (p0index ==  metaLines[i + 1]) {
                if (p1index == metaLines[i]) {
                    return i;
                }
            }
        }
        return -1;
    }

    public void DeleteLine(int lineIndex) {
        for (int i = lineIndex; i < metaLines.Length - 2; i += 2) {
            metaLines[i] = metaLines[i + 2];
            metaLines[i + 1] = metaLines[i + 3];
        }
        Array.Resize(ref metaLines, metaLines.Length - 2);
    }

    public int BisectLine(int lineIndex) {
        int p0Index = metaLines[lineIndex];
        int p1Index = metaLines[lineIndex + 1];
        DeleteLine(lineIndex);
        int newPoint = AddPoint((metaVerts[p0Index] + metaVerts[p1Index]) / 2);
        AddLine(p0Index, newPoint);
        AddLine(newPoint, p1Index);
        return newPoint;
    }

    public void DeletePoint(int index) {
        // Remove the vertex from the list
        int newSize = metaVerts.Length - 1;
        for (int i = index; i < newSize; i++) {
            metaVerts[i] = metaVerts[i + 1];
            metaVertModes[i] = metaVertModes[i + 1];
            metaUs[i] = metaUs[i + 1];
        }
        Array.Resize(ref metaVerts, newSize);
        Array.Resize(ref metaVertModes, newSize);
        Array.Resize(ref metaUs, newSize);

        // Delete lines whihc were attatched to it
        for (int i = 0; i < metaLines.Length; i += 2) {
            if (metaLines[i] == index || metaLines[i + 1] == index) {
                DeleteLine(i);
            }
        }

        // Update other lines for changed indicies
        for (int i = 0; i < metaLines.Length; i += 1) {
            if (metaLines[i] > index) {
                metaLines[i] -= 1;
            }
        }
    }
}


public enum Mesh2DVertMode {
    smooth,
    hard
}

