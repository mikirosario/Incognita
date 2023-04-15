using UnityEngine;
public class RectTransformGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        if (gameObject.activeInHierarchy)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                Gizmos.color = gizmoColor;

                // Draw lines to connect the corners
                Gizmos.DrawLine(corners[0], corners[1]);
                Gizmos.DrawLine(corners[1], corners[2]);
                Gizmos.DrawLine(corners[2], corners[3]);
                Gizmos.DrawLine(corners[3], corners[0]);
            }
        }
    }
}
