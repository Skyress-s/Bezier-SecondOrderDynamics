using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Internal;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(SpecialAbility))]
public class SpecialAbilityPropertyDrawer : PropertyDrawer {

    private int GetEnumValueIndex(SpecialAbility.SpecialAbilityType sat) {
        return Array.IndexOf(Enum.GetValues(typeof(SpecialAbility.SpecialAbilityType)), sat);
    }

    private SpecialAbility.SpecialAbilityType GetEnumValueFromIndex(int index) {
        return (SpecialAbility.SpecialAbilityType)Enum.GetValues(typeof(SpecialAbility.SpecialAbilityType)).GetValue(index);
    }

    private void ShowTypeField(Rect position, SerializedProperty property) {
        int oldInt = property.enumValueIndex;
        SpecialAbility.SpecialAbilityType oldType = GetEnumValueFromIndex(property.enumValueIndex);
        SpecialAbility.SpecialAbilityType newType = oldType;
        float toggleWidth = position.width / 3f;
        
        Rect rectDash = new Rect(position.min.x, position.min.y, toggleWidth, position.height);
        if (EditorGUI.ToggleLeft(rectDash, "Dash", newType == SpecialAbility.SpecialAbilityType.Dash)) {
            newType = SpecialAbility.SpecialAbilityType.Dash;
        } else if (newType == SpecialAbility.SpecialAbilityType.Dash) {
            newType = SpecialAbility.SpecialAbilityType.None;
        }
        Rect rectBounce = new Rect(position.min.x + toggleWidth, position.min.y, toggleWidth, position.height);
        if (EditorGUI.ToggleLeft(rectBounce, "Bounce", newType == SpecialAbility.SpecialAbilityType.Bounce)) {
            newType = SpecialAbility.SpecialAbilityType.Bounce;
        } else if (newType == SpecialAbility.SpecialAbilityType.Bounce) {
            newType = SpecialAbility.SpecialAbilityType.None;
        }
        Rect rectInvis = new Rect(position.min.x + 2 * toggleWidth, position.min.y, toggleWidth, position.height);
        if (EditorGUI.ToggleLeft(rectInvis, "Invis", newType == SpecialAbility.SpecialAbilityType.Invisibility)) {
            newType = SpecialAbility.SpecialAbilityType.Invisibility;
        } else if (newType == SpecialAbility.SpecialAbilityType.Invisibility) {
            newType = SpecialAbility.SpecialAbilityType.None;
        }

        property.enumValueIndex = GetEnumValueIndex(newType);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        
        property.isExpanded = EditorGUI.Foldout(rectFoldout, property.isExpanded, label);
        
        if (property.isExpanded) {
            Rect rectType = new Rect(position.min.x + EditorGUIUtility.labelWidth, position.min.y + EditorGUIUtility.singleLineHeight, position.size.x - EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty propType = property.FindPropertyRelative("type");
            ShowTypeField(rectType, propType);
            var curProp = GetEnumValueFromIndex(propType.enumValueIndex);
            if (curProp == SpecialAbility.SpecialAbilityType.None) {
                // show no fields
            } else {
                Rect rectCooldown = new Rect(position.min.x, position.min.y + 2 * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(rectCooldown, property.FindPropertyRelative("cooldown"));
                if (curProp == SpecialAbility.SpecialAbilityType.Invisibility) {
                    Rect rectDuration = new Rect(position.min.x, position.min.y + 3 * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(rectDuration, property.FindPropertyRelative("duration"));
                } else {
                    Rect rectPower = new Rect(position.min.x, position.min.y + 3 * EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(rectPower, property.FindPropertyRelative("power"));
                }
            }
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        int totalLines = 1;

        if (property.isExpanded) {
            totalLines++; // for type field
            SerializedProperty propType = property.FindPropertyRelative("type");
            switch (GetEnumValueFromIndex(propType.enumValueIndex)) {
                case SpecialAbility.SpecialAbilityType.None:
                    break;
                case SpecialAbility.SpecialAbilityType.Dash:
                case SpecialAbility.SpecialAbilityType.Bounce:
                case SpecialAbility.SpecialAbilityType.Invisibility:
                    totalLines += 2;
                    break;
            }
        }

        return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
    }
}