using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SecondOrderDynamics))]
public class SecondOrderDynamicsEditor : PropertyDrawer 
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // base.OnGUI(position, property, label);

        
        
        EditorGUI.BeginProperty(position, label, property);
        
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Rect minRect = new Rect(position.x, position.y, position.width * 0.4f - 5, position.height);
        // Rect mirroredRect = new Rect(position.x + position.width * 0.4f, position.y, position.width * 0.2f, position.height);
        // Rect maxRect = new Rect(position.x + position.width * 0.6f + 5, position.y, position.width * 0.4f - 5, position.height);
        
        // some magic
        SerializedProperty minProp = property.FindPropertyRelative("F");
        SerializedProperty mirrirProp = property.FindPropertyRelative("Z");
        SerializedProperty maxProp = property.FindPropertyRelative("R");

        // EditorGUI.FloatField(mirroredRect, 5f);
        // EditorGUI.Slider(maxRect, 5f, 0f, 10f);
        
        //draw!

        // var targetObject = property.serializedObject.targetObject;
        // var targetObjectClassType = targetObject.GetType();
        // var secOrdDynn = targetObjectClassType.GetField(property.propertyPath);
        float f, z, r;
        f = property.FindPropertyRelative("F").floatValue;
        z = property.FindPropertyRelative("Z").floatValue;
        r = property.FindPropertyRelative("R").floatValue;
        
        Vector3 startPos = Vector3.zero;
        SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(f, z, r, startPos);
        DrawGraph(secondOrderDynamics);
        
        
        // EditorGUI.PropertyField(minRect, minProp, GUIContent.none);
        // EditorGUI.PropertyField(mirroredRect, mirrirProp, GUIContent.none);
        // EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);
        
        EditorGUI.indentLevel = indent;
        
        EditorGUI.EndProperty();
    }

    private void DrawGraph(SecondOrderDynamics secOrdDyn)
    {
        int steps = 150;
        float time = 5f;
        float timeStep = time / steps;
        Vector3 targetPos = Vector3.up;
        
        // SecondOrderDynamics secondOrderDynamics = new SecondOrderDynamics(secOrdDyn.F, secOrdDyn.Z, secOrdDyn.R, startPos);
        
        // Debug.Log("YES");
        
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

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * 2f;
    }
}
