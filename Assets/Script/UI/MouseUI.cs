using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUI : MonoBehaviour
{
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }

    void OnGUI()
    {
        Vector2 mousePos = Event.current.mousePosition;
        GUIStyle dotStyle = new GUIStyle();
        dotStyle.normal.background = Texture2D.whiteTexture;
        GUI.Box(new Rect(mousePos.x - 2.5f, mousePos.y - 2.5f, 10, 10), GUIContent.none, dotStyle);
    }
}