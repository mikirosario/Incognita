using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public float radius = 0.5f;

    void OnDrawGizmos()
    {
        if (gameObject.activeInHierarchy)
        {
            Gizmos.color = gizmoColor;
            // Draw a circle
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
        }
    }
}
