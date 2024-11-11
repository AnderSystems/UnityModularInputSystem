using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

static class GameInputSystemProjectSettings
{
    public static GameInputSettings selectedInputSettings;

    [SettingsProvider]
    public static SettingsProvider CreateInputSystemSettingsProvider()
    {
        var provider = new SettingsProvider("Project/InputSystem", SettingsScope.Project)
        {
            label = "Input System",
            guiHandler = (searchContext) =>
            {
                selectedInputSettings = (GameInputSettings)EditorGUILayout.ObjectField("Input Settings", selectedInputSettings, typeof(GameInputSettings), false);
                GameInput.selectedSettings = selectedInputSettings;

                if (selectedInputSettings == null)
                {
                    if (GUILayout.Button("Create Input Settings"))
                    {
                        selectedInputSettings = ScriptableObject.CreateInstance<GameInputSettings>();
                        AssetDatabase.CreateAsset(selectedInputSettings, "Assets/InputSettings.asset");
                        AssetDatabase.SaveAssets();
                    }
                }
                else
                {
                    var serializedObject = new SerializedObject(selectedInputSettings);
                    var keysProperty = serializedObject.FindProperty("m_Inputs");

                    EditorGUILayout.PropertyField(keysProperty, true);

                    serializedObject.ApplyModifiedProperties();
                }

                if (GUI.changed)
                {
                    SaveSettings();
                }
            }
        };
        LoadSettings();
        return provider;
    }

    public static void LoadSettings()
    {
        string path = EditorPrefs.GetString("InputSystemSettingsPath", "Assets/InputSettings.asset");
        selectedInputSettings = AssetDatabase.LoadAssetAtPath<GameInputSettings>(path);
        GameInput.main.settings = selectedInputSettings;
        GameInput.selectedSettings = selectedInputSettings;
    }

    public static void SaveSettings()
    {
        if (selectedInputSettings != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedInputSettings);
            EditorPrefs.SetString("InputSystemSettingsPath", path);
            GameInput.main.settings = selectedInputSettings;
            GameInput.selectedSettings = selectedInputSettings;
        }
    }
}
