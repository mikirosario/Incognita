using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPointGizmo))]
public class SpawnPointGizmoEditor : Editor
{
    void OnSceneGUI()
    {
        SpawnPointGizmo gizmo = (SpawnPointGizmo)target;
        Transform spawnPoint = gizmo.transform;

        Handles.color = gizmo.gizmoColor;

        // Draw a circle
        Handles.DrawWireDisc(spawnPoint.position, Vector3.forward, gizmo.radius);

        // Alternatively, draw a square
        // Vector3 halfSize = new Vector3(gizmo.radius, gizmo.radius, 0);
        // Handles.DrawWireCube(spawnPoint.position, halfSize * 2);
    }
}
