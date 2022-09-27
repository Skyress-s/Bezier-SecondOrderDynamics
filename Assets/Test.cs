using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform positionFollow;
    public SecondOrderDynamics secondOrderDynamicsPos;
    public SecondOrderDynamics secondOrderDynamicsRot;


    private Vector3 posLast;
    private Vector3 rotLast;
    private void Start()
    {
        posLast = transform.position;
        secondOrderDynamicsPos.Init(positionFollow.position);

        rotLast = transform.eulerAngles;
        secondOrderDynamicsRot.Init(positionFollow.rotation.eulerAngles);

    }

    private void Update()
    {
        Vector3 vel = (transform.position - posLast) / Time.deltaTime;
        positionFollow.position = secondOrderDynamicsPos.Update(Time.deltaTime, transform.position);
        posLast = transform.position;


        Vector3 rotVel = (transform.eulerAngles - rotLast) / Time.deltaTime;
        positionFollow.eulerAngles = secondOrderDynamicsRot.Update(Time.deltaTime, transform.eulerAngles);
        rotLast = positionFollow.eulerAngles;

    }
}
