using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spline : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)]
    private float lerp = 0f;
    
    [SerializeField]
    private Transform p1;
    [SerializeField]
    private Transform p2;
    [SerializeField]
    private Transform p3;
    [SerializeField]
    private Transform p4;
    [SerializeField]
    private Transform pointVisualizer;

    private void Update() {
        //position
        Vector3 A = GetPoint(p1.position, p2.position, lerp);
        Vector3 B = GetPoint(p2.position, p3.position, lerp);
        Vector3 C = GetPoint(p3.position, p4.position, lerp);
        
        Vector3 D = GetPoint(A, B, lerp);
        Vector3 E = GetPoint(B, C, lerp);
        
        Vector3 F = GetPoint(D, E, lerp);
        // pointVisualizer.position = F;
        
        //postion bersteain

        pointVisualizer.position = P(lerp);
        
        //rotaiton

        // Quaternion r1 = Quaternion.LookRotation(p2.position - p1.position, p1.up);
        // Quaternion r2 = Quaternion.LookRotation(p4.position - p3.position, p4.up);
        //
        // Quaternion newRot = Quaternion.Slerp(r1, r2, lerp);

        Vector3 d1 = p1.forward;
        Vector3 d2 = p4.forward;
        
        Vector3 lerpRight = Vector3.Slerp(d1, d2, lerp);

        Vector3 up = Vector3.Cross(lerpRight, PDerived(lerp));
        
        Debug.DrawLine(pointVisualizer.position, pointVisualizer.position + PDerived(lerp).normalized * 4f);
        Debug.DrawLine(pointVisualizer.position, pointVisualizer.position + up * 10f);

        pointVisualizer.rotation = Quaternion.LookRotation(PDerived(lerp), up);
        
        
        
        
        
        //debug spline
        Debug.DrawLine(p1.position, p2.position, Color.cyan);
        Debug.DrawLine(p2.position, p3.position, Color.cyan);
        Debug.DrawLine(p3.position, p4.position, Color.cyan);
        
        Debug.DrawLine(A, B, Color.blue);
        Debug.DrawLine(B, C, Color.blue);
        
        Debug.DrawLine(D, E, Color.red);

    }

    Vector3 P(float t) {
        return p1.position * (-t * t * t + 3f * t * t - 3f * t + 1f) +
               p2.position * (3f * t * t * t - 6f * t * t + 3f * t) +
               p3.position * (-3f * t * t * t + 3f * t * t) +
               p4.position * (t * t * t);
    }

    Vector3 PDerived(float t) {
        return p1.position * (-3f * t * t + 6f * t - 3) +
               p2.position * (9f * t * t - 12f * t + 3f) +
               p3.position * (-9f * t * t + 6f * t) +
               p4.position * (3f * t * t);
    }
    
    
    private Vector3 GetPoint(Vector3 v1, Vector3 v2, float t) {
        return (1f - t) * v1 + t * v2;
    }

    private Vector3 GetRotation(Vector3 r1, Vector3 r2, float t) {
        return (1f - t) * r1 + t * r2;
    }
    
    
}
