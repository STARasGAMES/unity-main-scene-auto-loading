using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace SaG.MainSceneAutoLoading.Settings
{
    public class MainSceneAutoLoadingSettingsProvider : SettingsProvider
    {
        private SerializedObject _serializedObject;
        private Editor _editor;

        public MainSceneAutoLoadingSettingsProvider(string path, SettingsScope scopes,
            IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _editor = Editor.CreateEditor(MainSceneAutoLoadingSettings.GetOrCreate());
            _serializedObject = MainSceneAutoLoadingSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField($"CUSTOM 1 {EditorGUIUtility.labelWidth} {EditorGUIUtility.fieldWidth}");
            _editor.OnInspectorGUI();
        }

        public static bool IsSettingsAvailable()
        {
            return File.Exists(MainSceneAutoLoadingSettings.AssetPath);
        }

        [SettingsProvider]
        public static SettingsProvider Create()
        {
            if (IsSettingsAvailable())
            {
                var provider =
                    new MainSceneAutoLoadingSettingsProvider("Project/Main Scene Auto Loader", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords =
                    GetSearchKeywordsFromSerializedObject(MainSceneAutoLoadingSettings.GetSerializedSettings());
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }
    }
}