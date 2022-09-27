using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SecondOrderDynamics))]
public class SecondOrderDynamicsEditor : PropertyDrawer 
{
    float minX, maxX, minY, maxY;
    float rangeX { get { return maxX - minX; } }
    private float rangeY { get { return maxY - minY; } }
    
    
    // rect related
    private int expanedHeightMod = 15;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // base.OnGUI(position, property, label);
        float singleLineHeight = EditorGUIUtility.singleLineHeight;
        // return;
        
        EditorGUI.BeginProperty(position, label, property);
        Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, singleLineHeight);
        
        property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);
        if (property.isExpanded)
        {
            
            int lines = 1;
            Rect fRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x, singleLineHeight);
            SerializedProperty fprop = property.FindPropertyRelative("F");
            EditorGUI.Slider(fRect, fprop, 0f, 10f);
        
            Rect zRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x, singleLineHeight);
            SerializedProperty zprop = property.FindPropertyRelative("Z");
            EditorGUI.Slider(zRect, zprop, 0f, 10f);
        
            Rect rRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x, singleLineHeight);
            SerializedProperty rprop = property.FindPropertyRelative("R");
            EditorGUI.Slider(rRect, rprop, -10f, 10f);
        
            // minX = 0f;
            // maxX = 10f;
            // minY = -10f;
            // maxY = 10f;
            float f, z, r;
            f = property.FindPropertyRelative("F").floatValue;
            z = property.FindPropertyRelative("Z").floatValue;
            r = property.FindPropertyRelative("R").floatValue;



            List<Vector3> points = GetSecondOrderDynamicsPoints(f, z, r);
            FindGraphBounds(points, out minX, out maxX, out minY, out maxY);

            Rect graphRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x,
                singleLineHeight * (expanedHeightMod - lines));
            //
            // EditorGUI.DrawRect(graphRect, new Color(0.2f, 0.2f, 0.2f));
            // DrawLine(graphRect, points, Color.red);

            ImprovedEditorGraph graph = new ImprovedEditorGraph(graphRect);
            graph.DrawNew(points);

        }
        
         
        
        /*EditorGUI.indentLevel = 0;
        int indent = EditorGUI.indentLevel;
        
        // some magic
        SerializedProperty minProp = property.FindPropertyRelative("F");
        SerializedProperty mirrirProp = property.FindPropertyRelative("Z");
        SerializedProperty maxProp = property.FindPropertyRelative("R");
        
        //draw!
        float f, z, r;
        f = property.FindPropertyRelative("F").floatValue;
        z = property.FindPropertyRelative("Z").floatValue;
        r = property.FindPropertyRelative("R").floatValue;
        
        Debug.Log($"F {f} - Z {z} - R {r}");
        
        f = 7f;
        
        Vector3 startPos = Vector3.zero;
        SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(f, z, r, startPos);
        DrawGraph(secondOrderDynamics);
        Debug.Log("test");
        
        EditorGUI.indentLevel = indent;*/
        
        EditorGUI.EndProperty();
    }

    List<Vector3> GetSecondOrderDynamicsPoints(float f, float z, float r)
    {
        SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(f, z, r, Vector3.zero);
        int steps = 150;
        float time = 5f;
        float timeStep = time / steps;
        Vector3 targetPos = Vector3.up;
        
        List<Vector3> results = new List<Vector3>();
        for (int i = 0; i < steps; i++) {
            Vector3 point = secondOrderDynamics.Update(timeStep, targetPos);
            // float distance = Vector3.Distance(targetPos, point);
            float distance = point.y;
            results.Add(new Vector3(i * timeStep, distance, 0f));
        }

        return results;
    }

    void FindGraphBounds(in List<Vector3> points, out float minX, out float maxX, out float minY, out float maxY)
    {
        minX = minY = Single.PositiveInfinity;
        maxX = maxY = Single.NegativeInfinity;
        foreach (var point in points)
        {
            // x axis
            if (point.x < minX)
                minX = point.x;
            if (point.x > maxX)
                maxX = point.x;
            
            // y axis
            if (point.y < minY)
                minY = point.y;
            if (point.y > maxY)
                maxY = point.y;
        }
    }
    void DrawLine(Rect rect, List<Vector3> _points, Color _color) {
        //converting to the correct space
        for (int i = 0; i < _points.Count; i++) {
            _points[i] = PointToGraph(_points[i], rect);
        }

        Handles.color = _color;
        Handles.DrawAAPolyLine(2.0f, _points.Count, _points.ToArray());
    }

    Vector3 PointToGraph(Vector3 _point, Rect rect) {
        _point.x = Mathf.Lerp(rect.xMin, rect.xMax,
            (_point.x - minX) / (rangeX)); // the t in this case is the basic lerp function solved for t
        _point.y = Mathf.Lerp(rect.yMax, rect.yMin,
            (_point.y - minY) / (rangeY)); // --||-- exept we invert the points so its right side up
        return new Vector3(_point.x, _point.y, 0f);
    }
    private void DrawGraph(SecondOrderDynamics secOrdDyn)
    {
        int steps = 150;
        float time = 5f;
        float timeStep = time / steps;
        Vector3 targetPos = Vector3.up;
        
        
        // Debug.Log("YES");
        // SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(secOrdDyn.F, secOrdDyn.Z, secOrdDyn.R, startPos);
        
        List<Vector3> results = new List<Vector3>();
        for (int i = 0; i < steps; i++) {
            Vector3 point = secOrdDyn.Update(timeStep, targetPos);
            // float distance = Vector3.Distance(targetPos, point);
            float distance = point.y;
            results.Add(new Vector3(i * timeStep, distance, 0f));
        }
        
        ImprovedEditorGraph graph = new ImprovedEditorGraph(0f, -1, 1f, 1f, "RopeBehaviourViz");
        graph.AddLine(results);
        graph.Draw(50, 300);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight * expanedHeightMod;
        }

        return EditorGUIUtility.singleLineHeight;
    }
}