using System;
using System.Collections.Generic;
using System.Linq;
using SaG.MainSceneAutoLoading.MainSceneLoadedHandlers;
using SaG.MainSceneAutoLoading.MainSceneProviders;
using SaG.MainSceneAutoLoading.PlaymodeExitedHandlers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SaG.MainSceneAutoLoading.Settings
{
    public class MainSceneAutoLoadingSettings : ScriptableObject
    {
        public const string AssetPath = "Assets/MainSceneAutoLoadingSettings.asset";

        public bool Enabled = true;

        public SceneAsset MainScene = default;

        public bool LoadAllLoadedScenes = false;

        public bool RestoreHierarchyState = true;

        public HandlerSource MainSceneLoadedHandlerSource;

        public Object ExplicitMainSceneLoadedHandler;

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
            switch (MainSceneLoadedHandlerSource)
            {
                case HandlerSource.Default:
                    if (LoadAllLoadedScenes)
                        return new LoadAllLoadedScenes();
                    return new LoadActiveScene();
                case HandlerSource.FindObjectsOfType:
                    var handlers = new List<IMainSceneLoadedHandler>(FindInterfacesOfType<IMainSceneLoadedHandler>());
                    return new MainSceneLoadedHandlers.Composite(handlers);
                case HandlerSource.ExplicitObjectReference:
                    return (IMainSceneLoadedHandler) ExplicitMainSceneLoadedHandler;
                default:
                    throw new ArgumentOutOfRangeException(nameof(MainSceneLoadedHandlerSource));
            }
        }

        internal IPlaymodeExitedHandler GetPlaymodeExitedHandler()
        {
            return new RestoreSceneManagerSetup();
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
        
        private static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreate());
        }

        public enum HandlerSource
        {
            Default,
            FindObjectsOfType,
            ExplicitObjectReference
        }
    }
}