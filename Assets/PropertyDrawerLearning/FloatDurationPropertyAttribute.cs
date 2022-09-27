using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
///  https://nosuchstudio.medium.com/learn-unity-editor-scripting-property-drawers-part-2-6fe6097f1586
/// </summary>

public enum FloatDurationUnitsMode {
    Flexible,
    Seconds,
    Minutes,
    Hours
}
public class FloatDurationPropertyAttribute : PropertyAttribute {
    FloatDurationUnitsMode _unitsMode;
    public FloatDurationUnitsMode unitsMode {
        get { return _unitsMode; }
    }

    public FloatDurationPropertyAttribute(FloatDurationUnitsMode unitsMode = FloatDurationUnitsMode.Flexible) {
        _unitsMode = unitsMode;
    }
}

[CustomPropertyDrawer(typeof(FloatDurationPropertyAttribute))]
public class FloatDurationDrawer : PropertyDrawer {
    const float unitsLabelWidth = 80f;
    private FloatDurationUnitsMode uiUnitsMode;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var floatDurationAttr = attribute as FloatDurationPropertyAttribute;
        bool isFloatProperty = property.propertyType == SerializedPropertyType.Float;
        if (!isFloatProperty) {
            EditorGUI.LabelField(position, property.displayName, "FloatDurationPropertyAttribute cannot be used with non-float fields");
        } else {
            DrawFloatDurationField(position, property, label, floatDurationAttr.unitsMode);
        }
    }

    private float ConvertToUnits(float f, FloatDurationUnitsMode unitsMode) {
        switch (unitsMode) {
            case FloatDurationUnitsMode.Flexible:
                Debug.LogError("ConvertToUnits cannot be used with FloatDurationUnitsMode.Flexible.");
                return f;
            case FloatDurationUnitsMode.Minutes:
                return f / 60f;
            case FloatDurationUnitsMode.Hours:
                return f / 3600f;
            case FloatDurationUnitsMode.Seconds:
            default:
                return f;
        }
    }

    private float ConvertFromUnits(float f, FloatDurationUnitsMode unitsMode) {
        switch (unitsMode) {
            case FloatDurationUnitsMode.Flexible:
                Debug.LogError("ConvertToUnits cannot be used with FloatDurationUnitsMode.Flexible.");
                return f;
            case FloatDurationUnitsMode.Minutes:
                return f * 60f;
            case FloatDurationUnitsMode.Hours:
                return f * 3600f;
            case FloatDurationUnitsMode.Seconds:
            default:
                return f;
        }
    }

    private void DrawFloatDurationField(Rect position, SerializedProperty property, GUIContent label, FloatDurationUnitsMode unitsMode) {
        float curPropValue = property.floatValue;
        if (unitsMode == FloatDurationUnitsMode.Flexible) {
            if (uiUnitsMode == FloatDurationUnitsMode.Flexible) uiUnitsMode = FloatDurationUnitsMode.Seconds;
            var optionUnitsModes = new List<FloatDurationUnitsMode>() { FloatDurationUnitsMode.Seconds, FloatDurationUnitsMode.Minutes, FloatDurationUnitsMode.Hours };
            string[] displayOptions = optionUnitsModes.Select(um => um.ToString()).ToArray();
            int selectedIndex = optionUnitsModes.IndexOf(uiUnitsMode);
            float displayValue = ConvertToUnits(curPropValue, uiUnitsMode);
            Rect sliderPosition = new Rect(position.min.x, position.min.y, position.width - unitsLabelWidth, position.height);
            displayValue = EditorGUI.Slider(sliderPosition, label, displayValue, 0f, 100f);
            property.floatValue = ConvertFromUnits(displayValue, uiUnitsMode);

            Rect dropdownPosition = new Rect(position.min.x + position.width - unitsLabelWidth, position.y, unitsLabelWidth, position.height);
            selectedIndex = EditorGUI.Popup(dropdownPosition, selectedIndex, displayOptions);
            uiUnitsMode = optionUnitsModes[selectedIndex];
        } else {
            float displayValue = ConvertToUnits(curPropValue, unitsMode);
            Rect sliderPosition = new Rect(position.min.x, position.min.y, position.width - unitsLabelWidth, position.height);
            displayValue = EditorGUI.Slider(sliderPosition, label, displayValue, 0f, 100f);
            Rect labelPosition = new Rect(position.min.x + position.width - unitsLabelWidth, position.y, unitsLabelWidth, position.height);
            EditorGUI.LabelField(labelPosition, $"{unitsMode}");
            property.floatValue = ConvertFromUnits(displayValue, unitsMode);
        }
    }
}