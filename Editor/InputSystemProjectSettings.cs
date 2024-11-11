using UnityEditor;
using UnityEngine;

static class GameInputProjectSettings
{
    public static InputSettings selectedInputSettings;

    [SettingsProvider]
    public static SettingsProvider CreateGameInputSettingsProvider()
    {
        var provider = new SettingsProvider("Project/GameInput", SettingsScope.Project)
        {
            label = "Input System",
            guiHandler = (searchContext) =>
            {
                selectedInputSettings = (InputSettings)EditorGUILayout.ObjectField("Input Settings", selectedInputSettings, typeof(InputSettings), false);
                GameInput.selectedSettings = selectedInputSettings;

                if (selectedInputSettings == null)
                {
                    if (GUILayout.Button("Create Input Settings"))
                    {
                        selectedInputSettings = ScriptableObject.CreateInstance<InputSettings>();
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
        string path = EditorPrefs.GetString("GameInputSettingsPath", "Assets/InputSettings.asset");
        selectedInputSettings = AssetDatabase.LoadAssetAtPath<InputSettings>(path);
        GameInput.main.settings = selectedInputSettings;
        GameInput.selectedSettings = selectedInputSettings;
    }

    public static void SaveSettings()
    {
        if (selectedInputSettings != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedInputSettings);
            EditorPrefs.SetString("GameInputSettingsPath", path);
            GameInput.main.settings = selectedInputSettings;
            GameInput.selectedSettings = selectedInputSettings;
        }
    }
}
