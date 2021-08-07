using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Mesh2D", menuName = "mesh2D")]
public class Mesh2D : ScriptableObject {
    public Vector2[] verts;
    public Vector2[] normals;
    public float[] us;
    public int[] lines;

    public Vector2[] metaVerts;
    public int[] metaLines;
    public Mesh2DVertMode[] metaVertModes;

    public void Reset () {
        verts = new Vector2[] {
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(-1,1),
            new Vector2(1,1),
            new Vector2(1,1),
            new Vector2(1,-1),
            new Vector2(1,-1),
            new Vector2(-1,-1),
        };
        normals = new Vector2[] {
            new Vector2(-1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(0, -1),
        };
        us = new float[] {
            0,
            0.25f,
            0.25f,
            0.5f,
            0.5f,
            0.75f,
            0.75f,
            1,

        };
        lines = new int[] {
            1, 0,
            3, 2,
            5, 4,
            7, 6,
        };

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
}
