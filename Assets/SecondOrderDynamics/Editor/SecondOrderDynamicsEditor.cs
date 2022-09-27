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
            SerializedProperty fprop = property.FindPropertyRelative("f");
            EditorGUI.Slider(fRect, fprop, 0f, 10f);
        
            Rect zRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x, singleLineHeight);
            SerializedProperty zprop = property.FindPropertyRelative("z");
            EditorGUI.Slider(zRect, zprop, 0f, 10f);
        
            Rect rRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x, singleLineHeight);
            SerializedProperty rprop = property.FindPropertyRelative("r");
            EditorGUI.Slider(rRect, rprop, -10f, 10f);
        
            // minX = 0f;
            // maxX = 10f;
            // minY = -10f;
            // maxY = 10f;
            float f, z, r;
            f = property.FindPropertyRelative("f").floatValue;
            z = property.FindPropertyRelative("z").floatValue;
            r = property.FindPropertyRelative("r").floatValue;
            
            


            List<Vector3> points = GetSecondOrderDynamicsPoints(f, z, r);

            Rect graphRect = new Rect(position.min.x, position.min.y + lines++ * singleLineHeight, position.size.x,
                singleLineHeight * (expanedHeightMod - lines));

            ImprovedEditorGraph graph = new ImprovedEditorGraph(graphRect);
            graph.DrawNew(points);
                
            
            
        }


        // ensures its updated when changing GUI
        SecondOrderDynamics secondOrderDynamics= fieldInfo.GetValue(property.serializedObject.targetObject) as SecondOrderDynamics;
        secondOrderDynamics.UpdateVariables();
        
         
        
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
        SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(f, z, r);
        secondOrderDynamics.Init(Vector3.zero);
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

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight * expanedHeightMod;
        }

        return EditorGUIUtility.singleLineHeight;
    }
}