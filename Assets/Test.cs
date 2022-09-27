using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform positionFollow;
    public Transform rotationFollow;
    public SecondOrderDynamics secondOrderDynamicsPos;
    public SecondOrderDynamics secondOrderDynamicsRot;
    // public float fooAfter;
    public float fooAFter;

    public SecondOrderDynamicsF SecondOrderDynamicsF;


    private Vector3 posLast;

    private void Start()
    {
        posLast = transform.position;
        secondOrderDynamicsPos.Init(positionFollow.position);
    }

    private void Update()
    {
        Vector3 vel = (transform.position - posLast) / Time.deltaTime;
        positionFollow.position = secondOrderDynamicsPos.Update(Time.deltaTime, transform.position);
        posLast = transform.position;
        secondOrderDynamicsPos.UpdateVariables();

        

    }
}
