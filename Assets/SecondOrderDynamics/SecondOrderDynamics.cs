//refrence https://www.youtube.com/watch?v=KPoeNZZ6H4s&t=238s
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class SecondOrderDynamics {
    #region MyRegion
    
    private Vector3 xp; // previous input
    private Vector3 y, yd; //state variables
    private float _w, _z, _d, k1, k2, k3;

    public float F, Z, R;
    
    public SecondOrderDynamics(float f, float z, float r, Vector3 x0) {
        //compute constants
        _w = 2f * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        k1 = z / (Mathf.PI * f);
        k2 = 1f / (_w * _w);
        k3 = r * z / _w;
        
        //init variables
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }

    public void UpdateVariables(float f, float z, float r) {
        //compute constants
        _w = 2f * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        k1 = z / (Mathf.PI * f);
        k2 = 1f / (_w * _w);
        k3 = r * z / _w;
    }

    public Vector3 Update(float T, Vector3 x, Vector3? xd = null) {
        if (xd == null) { // estimate target velocity
            xd = (x - xp) / T;
            xp = x;
            // Debug.Log("is null!");
        }

        float k1_stable, k2_stable;
        if (_w * T < _z) { // clamp k2 to guarantee stability without jitter
            k1_stable = k1;
            k2_stable = Mathf.Max(k2, T * T / 2f + T * k1 / 2f, T * k1);
        }
        else { // use pole matching when the system is very fast
            float t1 = Mathf.Exp(-_z * _w * T);
            float alpha = 2f * t1 * (_z <= 1f ? Mathf.Cos(T * _d) : MathF.Cosh(T * _d));
            float beta = t1 * t1;
            float t2 = T / (1f + beta - alpha);
            k1_stable = (1 - beta) * t2;
            k2_stable = T * t2;
        }

        y = y + T * yd; //integrate position by velocity
        yd = (Vector3)(yd + T * (x + k3 * xd - y - k1 * yd) / k2_stable);
        return y;
    }
    #endregion


    
    
}
