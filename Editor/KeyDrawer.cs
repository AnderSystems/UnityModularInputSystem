using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InputSystem.key))]
public class KeyDrawer : PropertyDrawer
{
    private static string currentBindingProperty = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Set label width
        EditorGUIUtility.labelWidth = 70;
        EditorGUI.BeginProperty(position, label, property);

        // Set the position to draw fields
        Rect keyCodeRect = new Rect(position.x, position.y, position.width / 2 - 20, EditorGUIUtility.singleLineHeight);
        //Rect mouseButtonRect = new Rect(position.x + position.width / 4 - 20, position.y, position.width / 4 - 20, EditorGUIUtility.singleLineHeight);

        Rect buttonRect = new Rect(position.x + position.width / 2 - 1, position.y, (position.width / 2) - 26, EditorGUIUtility.singleLineHeight);
        Rect cancelButtonRect = new Rect(position.x + position.width - 28, position.y, 20, EditorGUIUtility.singleLineHeight);

        // Get the unique property path to use as a key
        string propertyPath = property.propertyPath;

        // Draw the fields
        EditorGUI.PropertyField(keyCodeRect, property.FindPropertyRelative("keyCode"), new GUIContent("Key"));
        //EditorGUI.PropertyField(mouseButtonRect, property.FindPropertyRelative("mouseButton"), new GUIContent("Mouse"));

        // Draw Bind Button
        bool isBinding = currentBindingProperty == propertyPath;
        if (isBinding)
        {
            if (Event.current.type == EventType.KeyDown)
            {
                property.FindPropertyRelative("keyCode").intValue = (int)Event.current.keyCode;
                currentBindingProperty = null;
                Event.current.Use();
            }
            else
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 0)
                    {
                        property.FindPropertyRelative("keyCode").intValue = (int)KeyCode.Mouse0;
                        currentBindingProperty = null;
                        Event.current.Use();
                    }

                    if (Event.current.button == 1)
                    {
                        property.FindPropertyRelative("keyCode").intValue = (int)KeyCode.Mouse1;
                        currentBindingProperty = null;
                        Event.current.Use();
                    }

                    if (Event.current.button == 2)
                    {
                        property.FindPropertyRelative("keyCode").intValue = (int)KeyCode.Mouse2;
                        currentBindingProperty = null;
                        Event.current.Use();
                    }
                }
            }
            isBinding = GUI.Toggle(buttonRect, isBinding, "Press key to bind", GUI.skin.button);
        }
        else
        {
            if (GUI.Button(buttonRect, "Bind"))
            {
                currentBindingProperty = propertyPath;
            }
        }

        // Draw Cancel/Clear Button
        if (isBinding)
        {
            if (GUI.Button(cancelButtonRect, "x"))
            {
                currentBindingProperty = null;
            }
        }
        else
        {
            if (GUI.Button(cancelButtonRect, "x"))
            {
                if (EditorUtility.DisplayDialog("Clear Key", "Are you sure you want to clear the key?", "Yes", "No"))
                {
                    property.FindPropertyRelative("keyCode").intValue = (int)KeyCode.None;
                }
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 1;
    }
}