using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectTransformGizmo))]
public class RectTransformGizmoEditor : Editor
{
    void OnSceneGUI()
    {
        RectTransformGizmo gizmo = (RectTransformGizmo)target;
        RectTransform rectTransform = gizmo.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Handles.color = gizmo.gizmoColor;

            // Draw lines to connect the corners
            Handles.DrawLine(corners[0], corners[1]);
            Handles.DrawLine(corners[1], corners[2]);
            Handles.DrawLine(corners[2], corners[3]);
            Handles.DrawLine(corners[3], corners[0]);
        }
    }
}
