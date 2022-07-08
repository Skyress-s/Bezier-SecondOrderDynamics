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
        int steps = 150;
        float time = 2f;
        float timeStep = time / steps;
        Vector3 targetPos = Vector3.up;
        Vector3 startPos = Vector3.zero;
        
        FakeRope fakeRope = (FakeRope)target;
        SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(fakeRope.f, fakeRope.z, fakeRope.r, startPos);

        List<Vector3> results = new List<Vector3>();
        for (int i = 0; i < steps; i++) {
            Vector3 point = secondOrderDynamics.Update(timeStep, targetPos);
            // float distance = Vector3.Distance(targetPos, point);
            float distance = point.y;
            results.Add(new Vector3(i * timeStep, distance, 0f));
        }

        float minn = -10;
        ImprovedEditorGraph graph = new ImprovedEditorGraph(0f, -1, 2, 3, "Herro");
        graph.Draw(50, 300);
        graph.DrawLine(results, Color.red);
        
        
        //creating own graph
        


        
    }
    
    
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(FakeRope fakeRope, GizmoType gizmoType)
    {
        if (fakeRope.t1 == null  || fakeRope.t3 == null) 
            return;
        Vector3 middlePos = fakeRope.GetMiddlePosition();    
        
        Gizmos.color = Color.cyan;
        Vector3 targetPosition = Bezier.PointQuadratic(fakeRope.t1.position, middlePos,
            fakeRope.t3.position, fakeRope.t);
        Gizmos.DrawSphere(targetPosition, 0.2f);
        Gizmos.color = Color.red;
        Vector3 dynamicPosition =Bezier.PointQuadratic(fakeRope.t1.position, fakeRope.middlePosition, fakeRope.t3.position, 0.5f);
        Gizmos.DrawSphere(dynamicPosition, 0.2f);
        
        Handles.color = Color.green;
        Handles.DrawPolyLine(targetPosition, dynamicPosition);
        Handles.color = Color.red;
        Handles.DrawPolyLine(targetPosition, middlePos);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fakeRope.t1.position, 0.1f);
        Gizmos.DrawSphere(fakeRope.GetMiddlePosition(), 0.2f);
        Gizmos.DrawSphere(fakeRope.t3.position, 0.3f);


        int segments = 10;
        List<Vector3> linePositions = new List<Vector3>();
        for (int i = 0; i < segments; i++) {
            linePositions.Add(Bezier.PointQuadratic(fakeRope.t1.position, fakeRope.GetMiddlePosition(), fakeRope.t3.position,
                (float)i / (float)(segments - 1)));
        }
        
        Handles.DrawPolyLine(linePositions.ToArray());
            
        
    }

}
