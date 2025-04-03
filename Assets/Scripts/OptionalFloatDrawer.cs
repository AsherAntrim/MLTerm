using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(OptionalFloat))]
public class OptionalFloatDrawer : PropertyDrawer {
    private const float toggleWidth = 20f;
    private const float spacing = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty useValueProp = property.FindPropertyRelative("useValue");
        SerializedProperty valueProp = property.FindPropertyRelative("value");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        Rect toggleRect = new Rect(position.x, position.y, toggleWidth, position.height);
        Rect floatRect = new Rect(position.x + toggleWidth + spacing, position.y,
                                  position.width - toggleWidth - spacing, position.height);

        useValueProp.boolValue = EditorGUI.Toggle(toggleRect, useValueProp.boolValue);

        EditorGUI.BeginDisabledGroup(!useValueProp.boolValue);
        EditorGUI.PropertyField(floatRect, valueProp, GUIContent.none);
        EditorGUI.EndDisabledGroup();
    }
}
