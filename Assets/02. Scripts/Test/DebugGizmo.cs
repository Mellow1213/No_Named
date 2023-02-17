using UnityEngine;
using UnityEditor;

public class DebugGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.white;  // 기즈모 색상
    [Range(0.1f, 10.0f)] public float gizmoSize = 0.5f;
    [Range(0.1f, 2.0f)] public float labelSize = 0.1f;
    public bool showLabel = true;
    public string gizmoName = "My Gizmo";
    public bool drawGizmoAsWire = false;

    private void OnDrawGizmos()
    {
        Handles.color = gizmoColor;
        Gizmos.color = gizmoColor;
        
        if (drawGizmoAsWire)
        {
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, gizmoSize);
        }

        if (showLabel)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = (int)(labelSize * 100);
            Handles.Label(transform.position + Vector3.up * (gizmoSize + 0.1f), gizmoName, labelStyle);
        }
    }
}