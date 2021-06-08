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
            EditorGUILayout.Space();
            _editor.OnInspectorGUI();
        }

        public static bool IsSettingsAvailable()
        {
            return MainSceneAutoLoadingSettings.TryLoadAsset(out var _);
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