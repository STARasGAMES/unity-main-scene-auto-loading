using SaG.MainSceneAutoLoading.MainSceneLoadedHandlers;
using SaG.MainSceneAutoLoading.MainSceneProviders;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.Settings
{
    public class MainSceneAutoLoadingSettings : ScriptableObject
    {
        public const string AssetPath = "Assets/MainSceneAutoLoadingSettings.asset";

        public bool Enabled = true;

        public SceneAsset MainScene = default;

        internal IMainSceneProvider GetMainSceneProvider()
        {
            if (MainScene != null)
            {
                return new ConcreteMainSceneProvider(MainScene);
            }

            return new FirstSceneInBuildSettingsMainSceneProvider();
        }

        internal IMainSceneLoadedHandler GetLoadMainSceneHandler()
        {
            return new LoadActiveScene();
        }

        internal static MainSceneAutoLoadingSettings GetOrCreate()
        {
            var settings = AssetDatabase.LoadAssetAtPath<MainSceneAutoLoadingSettings>(AssetPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<MainSceneAutoLoadingSettings>();
                settings.Enabled = true;
                settings.MainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
                AssetDatabase.CreateAsset(settings, AssetPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreate());
        }
    }
}