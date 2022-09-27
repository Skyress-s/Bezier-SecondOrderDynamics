using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(FakeRope))]
public class FakeRopeEditor : Editor 
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
    }
    
    
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(FakeRope fakeRope, GizmoType gizmoType)
    {
        
        
        
        if (fakeRope.t1 == null  || fakeRope.t3 == null) 
            return;
        Vector3 targetPosition = fakeRope.GetMiddlePosition();
        Vector3 dynamicPosition = fakeRope.dynamicmiMiddlePosition;
        if (!Application.isPlaying)
            dynamicPosition = targetPosition;
        
        //draw middle spheres
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(dynamicPosition, 0.05f);
        
        //draw lines
        Handles.color = Color.red;
        Handles.DrawPolyLine(targetPosition, Bezier.PointLinear(fakeRope.t1.position, fakeRope.t3.position, 0.5f));
        Handles.color = Color.gray;
        Handles.DrawPolyLine(targetPosition, dynamicPosition);
        
        //draw handles
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fakeRope.t1.position, 0.1f);
        Gizmos.DrawSphere(fakeRope.t3.position, 0.1f);


        int segments = 11;
        List<Vector3> linePositions = new List<Vector3>();
        for (int i = 0; i < segments; i++) {
            linePositions.Add(Bezier.PointQuadratic(fakeRope.t1.position, fakeRope.GetMiddlePosition(), fakeRope.t3.position,
                (float)i / (float)(segments - 1)));
        }
        
        Handles.DrawPolyLine(linePositions.ToArray());
            
        
    }

}
