using UnityEditor;
using UnityEngine;

public class InputSettingsWindow : EditorWindow
{
    private InputSettings inputSettings;

    [MenuItem("Window/Input Settings")]
    public static void ShowWindow()
    {
        GetWindow<InputSettingsWindow>("Input Settings");
    }

    private void OnGUI()
    {
        GUILayout.Label("Input Settings", EditorStyles.boldLabel);

        inputSettings = InputSystemProjectSettings.selectedInputSettings;
        inputSettings = (InputSettings)EditorGUILayout.ObjectField("Input Settings", inputSettings, typeof(InputSettings), false);

        if (inputSettings != null)
        {
            SerializedObject serializedObject = new SerializedObject(inputSettings);
            SerializedProperty inputsSettingsProperty = serializedObject.FindProperty("m_Inputs");
            EditorGUILayout.PropertyField(inputsSettingsProperty, true);

            serializedObject.ApplyModifiedProperties();
        }

        InputSystem.main.settings = inputSettings;

        InputSystemProjectSettings.selectedInputSettings = inputSettings;
    }
}
