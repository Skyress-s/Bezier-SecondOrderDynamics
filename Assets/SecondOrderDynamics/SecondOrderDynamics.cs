//refrence https://www.youtube.com/watch?v=KPoeNZZ6H4s&t=238s
using System;
using UnityEngine;

[Serializable]
public class SecondOrderDynamics {
    #region properties
    
    [SerializeField] private float f, z, r;
    
    private Vector3 xp; // previous input
    private Vector3 y, yd; //state variables
    private float _w, _z, _d, k1, k2, k3;

    #endregion

    #region fields

    

    public float F
    {
        get { return f; }
        set {
            f = value;
            UpdateVariables(); 
        }
    }
    public float Z
    {
        get { return z; }
        set {
            z = value;
            UpdateVariables(); 
        }
    }
    public float R
    {
        get { return r; }
        set {
            r = value;
            UpdateVariables(); 
        }
    }
        
    #endregion
        
    public SecondOrderDynamics(float f, float z, float r)
    {
        this.f = f;
        this.z = z;
        this.r = r;
    }

    // use BEFORE for call of Update()
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x0">start position</param>
    public void Init(Vector3 x0)
    {
        UpdateVariables(); 
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }
    
    public void UpdateVariables() {
        // //compute constants
        // _w = 2f * Mathf.PI * f;
        // _z = z;
        // _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        // k1 = z / (Mathf.PI * f);
        // k2 = 1f / (_w * _w);
        //compute constants
        _w = 2f * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        k1 = z / (Mathf.PI * f);
        k2 = 1f / (_w * _w);
        k3 = r * z / _w;       // k3 = r * z / _w;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="T"></param>
    /// <param name="x">target postion</param>
    /// <param name="xd">target velocity</param>
    /// <returns></returns>
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


    
    
}