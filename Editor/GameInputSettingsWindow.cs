using UnityEditor;
using UnityEngine;

public class GameInputSettingsWindow : EditorWindow
{
    private GameInputSettings inputSettings;

    [MenuItem("Window/Input Settings")]
    public static void ShowWindow()
    {
        GetWindow<GameInputSettingsWindow>("Input Settings");
    }

    private void OnGUI()
    {
        GUILayout.Label("Input Settings", EditorStyles.boldLabel);

        inputSettings = GameInputSystemProjectSettings.selectedInputSettings;
        inputSettings = (GameInputSettings)EditorGUILayout.ObjectField("Input Settings", inputSettings, typeof(GameInputSettings), false);

        if (inputSettings != null)
        {
            SerializedObject serializedObject = new SerializedObject(inputSettings);
            SerializedProperty inputsSettingsProperty = serializedObject.FindProperty("m_Inputs");
            EditorGUILayout.PropertyField(inputsSettingsProperty, true);

            serializedObject.ApplyModifiedProperties();
        }

        GameInput.main.settings = inputSettings;

        GameInputSystemProjectSettings.selectedInputSettings = inputSettings;
    }
}
