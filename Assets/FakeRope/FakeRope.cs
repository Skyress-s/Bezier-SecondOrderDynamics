using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRope : MonoBehaviour {
    //public state
    [Header("state")]
    public Transform t1;
    public Transform t2;
    public Transform t3;
    
    
    
    [Header("SecondOrderDynamics")] 
    public SecondOrderDynamics secondOrderDynamics;
    
    
    //config
    [Header("Config")] public float targetLength = 4f;
    public bool bVelZero = false;
    
    [Header("Refrences")][Tooltip("will only calculate if a LineRenderer is selecter")]
    public LineRenderer lr;

    //private state
    [HideInInspector] public Vector3 dynamicmiMiddlePosition;
    private Vector3 targetLastFrame;
    private void Start() {
        dynamicmiMiddlePosition = GetMiddlePosition();
        targetLastFrame = dynamicmiMiddlePosition;
        secondOrderDynamics.Init(transform.position);
        startF = secondOrderDynamics.F;
    }

    private float startF;
    private void Update() {


        //update the dynamincs
        float stiffness = startF;
        if (targetLength < Vector3.Distance(t1.position, t3.position)) {
            stiffness *= 4f;
        }

        secondOrderDynamics.F = stiffness;
        //velocity
        Vector3? vel = GetMiddlePosition() - targetLastFrame;
        targetLastFrame = GetMiddlePosition();
        
        vel = vel / Time.deltaTime;
        // Debug.Log(vel);
        if (bVelZero) {
            // bVelZero = false;
            vel = null;
        }
        dynamicmiMiddlePosition = secondOrderDynamics.Update(Time.deltaTime, GetMiddlePosition(), vel);

        // //moves coobe
        // coobe.position = middlePosition;
        
        //draws
        for (int i = 0; i < lr.positionCount; i++) {
            lr.SetPosition(i, Bezier.PointQuadratic(t1.position, dynamicmiMiddlePosition, t3.position, (float)i / (float)(lr.positionCount - 1)));
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
