using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoUtils
{
    public static void DrawRect(Rect bounds)
    {
        //left line
        Gizmos.DrawLine(new Vector2(bounds.x, bounds.y), new Vector2(bounds.x, bounds.y + bounds.height));
        //bottom line
        Gizmos.DrawLine(new Vector2(bounds.x, bounds.y), new Vector2(bounds.x + bounds.width, bounds.y));
        //right line
        Gizmos.DrawLine(new Vector2(bounds.x + bounds.width, bounds.y), new Vector2(bounds.x + bounds.width, bounds.y + bounds.height));
        //top line
        Gizmos.DrawLine(new Vector2(bounds.x, bounds.y + bounds.height), new Vector2(bounds.x + bounds.width, bounds.y + bounds.height));
    }
}