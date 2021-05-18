using System.Collections.Generic;
using System.IO;
using SaG.MainSceneAutoLoading.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SaG.MainSceneAutoLoading
{
    public class MainSceneAutoLoadingSettingsProvider : SettingsProvider
    {
        private SerializedObject _serializedObject;
        
        public MainSceneAutoLoadingSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _serializedObject = MainSceneAutoLoadingSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            _serializedObject.Update();

            EditorGUILayout.PropertyField(
                _serializedObject.FindProperty(nameof(MainSceneAutoLoadingSettings.Enabled)));
            
            EditorGUILayout.PropertyField(
                _serializedObject.FindProperty(nameof(MainSceneAutoLoadingSettings.MainScene)));
            
            EditorGUILayout.PropertyField(
                _serializedObject.FindProperty(nameof(MainSceneAutoLoadingSettings.LoadAllLoadedScenes)));
            
            EditorGUILayout.PropertyField(
                _serializedObject.FindProperty(nameof(MainSceneAutoLoadingSettings.RestoreHierarchyState)));
            
            _serializedObject.ApplyModifiedProperties();
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
                var provider = new MainSceneAutoLoadingSettingsProvider("Project/Main Scene Auto Loader", SettingsScope.Project);
 
                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromSerializedObject(MainSceneAutoLoadingSettings.GetSerializedSettings());
                return provider;
            }
 
            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }
    }
}