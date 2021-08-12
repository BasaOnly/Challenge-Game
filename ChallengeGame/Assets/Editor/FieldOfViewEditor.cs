using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBehaviour))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyBehaviour patrol = (EnemyBehaviour) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(patrol.transform.position, Vector3.up, Vector3.forward, 360, patrol.radius);
        //left side
        Vector3 viewAngle01 = DirectionFromAngle(patrol.transform.eulerAngles.y, -patrol.angle / 2);
        //right side
        Vector3 viewAngle02 = DirectionFromAngle(patrol.transform.eulerAngles.y, patrol.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(patrol.transform.position, patrol.transform.position + viewAngle01 * patrol.radius);
        Handles.DrawLine(patrol.transform.position, patrol.transform.position + viewAngle02 * patrol.radius);

        if (patrol.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(patrol.transform.position, patrol.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
