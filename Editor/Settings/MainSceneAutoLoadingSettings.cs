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

        [SerializeReference]
        internal IMainSceneProvider _mainSceneProvider = new FirstSceneInBuildSettings();

        [SerializeReference]
        internal IMainSceneLoadedHandler _mainSceneLoadedHandler = new LoadAllLoadedScenes();

        [SerializeReference]
        internal IPlaymodeExitedHandler _playmodeExitedHandler = new RestoreSceneManagerSetup();

        public bool RestoreHierarchyState = true;

        internal IMainSceneProvider GetMainSceneProvider()
        {
            return _mainSceneProvider;
        }

        internal IMainSceneLoadedHandler GetLoadMainSceneHandler()
        {
            return _mainSceneLoadedHandler;
        }

        internal IPlaymodeExitedHandler GetPlaymodeExitedHandler()
        {
            return _playmodeExitedHandler;
        }

        internal static MainSceneAutoLoadingSettings GetOrCreate()
        {
            var settings = AssetDatabase.LoadAssetAtPath<MainSceneAutoLoadingSettings>(AssetPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<MainSceneAutoLoadingSettings>();
                settings.Enabled = true;
                settings._mainSceneProvider = new FirstSceneInBuildSettings();
                settings._mainSceneLoadedHandler = new LoadAllLoadedScenes();
                settings._playmodeExitedHandler = new RestoreSceneManagerSetup();
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