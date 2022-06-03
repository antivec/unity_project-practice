using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D m_mouseCursor;
    public Vector2 hotSpot_V;
    Vector2 m_hotSpot = Vector2.zero;
    public bool isMouseVisible = true;
    public bool isHotSpotOnTxtCenter = false;


    private void Start()
    {
        if(isMouseVisible)
        {
            ShowCursor();
        }
        else
            HideCursor();   
    }

    public void ShowCursor()
    {
        StartCoroutine("GenerateCursor");
    }
    public void HideCursor()
    {
        Cursor.visible = false;
        isMouseVisible = false;
        StopCoroutine("GenerateCursor");
    }
    IEnumerator GenerateCursor()
    {
        yield return new WaitForEndOfFrame();

        Cursor.visible = true;
        if(isHotSpotOnTxtCenter)
        {
            m_hotSpot.x = m_mouseCursor.width / 2;
            m_hotSpot.y = m_mouseCursor.height / 2;
        }
        else
        {
            m_hotSpot = hotSpot_V;
        }
        Cursor.SetCursor(m_mouseCursor, m_hotSpot, CursorMode.Auto);
        isMouseVisible = true;
    }
}