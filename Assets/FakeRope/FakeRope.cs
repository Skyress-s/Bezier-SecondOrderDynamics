using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRope : MonoBehaviour {
    public Transform t1;
    public Transform t2;
    public Transform t3;
    public Transform coobe;
    public bool bVelZero = false;
    [HideInInspector] public SecondOrderDynamics secondOrderDynamics;
    
    
    public Vector3 middlePosition;
    
    [Range(0, 1)]
    public float t;

    [Header("SecondOrderDynamics")] 
    [Range(0,10)] public float f = 1f;
    [Range(0,5)]public float z = 1f;
    [Range(-6,6)]public float r = 1f;
    
    
    //config
    [Header("Config")] public float targetLength = 4f;

    public LineRenderer lr;


    private Vector3 targetLastFrame;
    private void Start() {
        middlePosition = GetMiddlePosition();
        targetLastFrame = middlePosition;
        secondOrderDynamics = new SecondOrderDynamics(1.4f, 0.15f, -2.0f, transform.position);
    }

    private void Update() {
        // middlePosition = Vector3.Lerp(middlePosition, GetMiddlePosition(), 5f * Time.fixedDeltaTime);


        //update the dynamincs
        float stiffness = f;
        if (targetLength < Vector3.Distance(t1.position, t3.position)) {
            stiffness *= 4f;
        }
        secondOrderDynamics.UpdateVariables(stiffness, z, r);

        //velocity
        Vector3? vel = GetMiddlePosition() - targetLastFrame;
        targetLastFrame = GetMiddlePosition();
        
        vel = vel / Time.deltaTime;
        // Debug.Log(vel);
        if (bVelZero) {
            // bVelZero = false;
            vel = null;
        }
        middlePosition = secondOrderDynamics.Update(Time.deltaTime, GetMiddlePosition(), vel);

        // //moves coobe
        // coobe.position = middlePosition;
        
        //draws
        for (int i = 0; i < lr.positionCount; i++) {
            lr.SetPosition(i, Bezier.PointQuadratic(t1.position, middlePosition, t3.position, (float)i / (float)(lr.positionCount - 1)));
        }
        
    }

    public Vector3 GetMiddlePosition() {
        float realLength = Vector3.Distance(t1.position, t3.position); 
        if ( realLength > targetLength) {
            return Bezier.PointLinear(t1.position, t3.position, 0.5f);
        }

        float difference = targetLength - realLength;
        return Bezier.PointLinear(t1.position, t3.position, 0.5f) + Physics.gravity.normalized * difference;
    }
    
    
    
    
}
