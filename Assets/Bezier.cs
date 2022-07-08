using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{

    /// <summary>
    /// gets the point by positions
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 PointCubic(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t) {
        return p1 * (-t * t * t + 3f * t * t - 3f * t + 1f) +
               p2 * (3f * t * t * t - 6f * t * t + 3f * t) +
               p3 * (-3f * t * t * t + 3f * t * t) +
               p4 * (t * t * t);
    }

    public static Vector3 PointQuadratic(Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        // return p1 * (1 - 2f * t * t) +
        //        p2 * (t - t * t) +
        //        p3 * (t * t); // mathematical mistake somewhere
        Vector3 A = PointLinear(p1, p2, t);
        Vector3 B = PointLinear(p2, p3, t);
        return PointLinear(A, B, t);
    }

    public static Vector3 PointLinear(Vector3 p1, Vector3 p2, float t) {
        return (1f - t) * p1 + t * p2;
    }
}
