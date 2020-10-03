using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEditor : MonoBehaviour
{
    public Texture2D cursorTexture;
    private Vector2 cursorOffset;

    private void Start()
    {
        SetCursor();
    }

    private void SetCursor()
    {
        cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }
}
