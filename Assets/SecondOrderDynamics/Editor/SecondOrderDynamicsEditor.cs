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

        Rect minRect = new Rect(position.x, position.y, position.width * 0.4f - 5, position.height);
        Rect mirroredRect = new Rect(position.x + position.width * 0.4f, position.y, position.width * 0.2f, position.height);
        Rect maxRect = new Rect(position.x + position.width * 0.6f + 5, position.y, position.width * 0.4f - 5, position.height);
        
        // some magic
        SerializedProperty minProp = property.FindPropertyRelative("F");
        SerializedProperty mirrirProp = property.FindPropertyRelative("Z");
        SerializedProperty maxProp = property.FindPropertyRelative("R");

        EditorGUI.FloatField(mirroredRect, 5f);
        EditorGUI.Slider(maxRect, 5f, 0f, 10f);
        
        
        
        //draw!
        
        // EditorGUI.PropertyField(minRect, minProp, GUIContent.none);
        // EditorGUI.PropertyField(mirroredRect, mirrirProp, GUIContent.none);
        // EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);
        
        EditorGUI.indentLevel = indent;
        
        EditorGUI.EndProperty();
        


    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * 2f;
    }
}
