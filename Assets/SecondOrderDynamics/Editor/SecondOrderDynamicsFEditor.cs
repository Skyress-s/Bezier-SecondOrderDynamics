using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SecondOrderDynamicsF))]
public class SecondOrderDynamicsFEditor : PropertyDrawer 
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

            Rect graphRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x,
                singleLineHeight * (expanedHeightMod - lines));

            ImprovedEditorGraph graph = new ImprovedEditorGraph(graphRect);
            graph.DrawNew(points);

        }
        
        EditorGUI.EndProperty();
    }

    List<Vector3> GetSecondOrderDynamicsPoints(float f, float z, float r)
    {
        SecondOrderDynamicsF secondOrderDynamics = new SecondOrderDynamicsF(f, z, r, 0f);
        int steps = 150;
        float time = 5f;
        float timeStep = time / steps;
        float targetPos = 1;
        
        List<Vector3> results = new List<Vector3>();
        for (int i = 0; i < steps; i++) {
            float point = secondOrderDynamics.Update(timeStep, targetPos);
            // float distance = Vector3.Distance(targetPos, point);
            float distance = point;
            results.Add(new Vector3(i * timeStep, distance, 0f));
        }

        return results;
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
