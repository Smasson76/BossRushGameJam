using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void OnMouseEnter() {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
