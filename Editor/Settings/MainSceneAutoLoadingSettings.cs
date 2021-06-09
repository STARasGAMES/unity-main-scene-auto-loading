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
    public sealed class MainSceneAutoLoadingSettings : ScriptableObject
    {
        private const string DefaultAssetPath = "Assets/MainSceneAutoLoadingSettings.asset";
        private static string AssetPathKey => $"{Application.dataPath}_MainSceneAutoLoadingSettingsPath";

        public bool Enabled = true;

        [SerializeReference]
        internal IMainSceneProvider _mainSceneProvider = new FirstSceneInBuildSettings();

        [SerializeReference]
        internal IMainSceneLoadedHandler _mainSceneLoadedHandler = new LoadAllLoadedScenes();

        [SerializeReference]
        internal IPlaymodeExitedHandler _playmodeExitedHandler = new RestoreSceneManagerSetup();

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
            if (TryLoadAsset(out var settings))
            {
                return settings;
            }

            settings = ScriptableObject.CreateInstance<MainSceneAutoLoadingSettings>();
            settings.Enabled = true;
            settings._mainSceneProvider = new FirstSceneInBuildSettings();
            settings._mainSceneLoadedHandler = new LoadAllLoadedScenes();
            settings._playmodeExitedHandler = new RestoreSceneManagerSetup();
            AssetDatabase.CreateAsset(settings, DefaultAssetPath);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreate());
        }

        internal static bool TryLoadAsset(out MainSceneAutoLoadingSettings settings)
        {
            string assetPath = EditorPrefs.GetString(AssetPathKey, DefaultAssetPath);
            // try to load at the saved or default path
            settings = AssetDatabase.LoadAssetAtPath<MainSceneAutoLoadingSettings>(assetPath);
            if (settings != null)
                return true;

            // if no asset at path try to find it in project's assets
            var assetGuid = AssetDatabase.FindAssets($"t:{typeof(MainSceneAutoLoadingSettings)}").FirstOrDefault();
            if (string.IsNullOrEmpty(assetGuid))
                return false;
            assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            settings = AssetDatabase.LoadAssetAtPath<MainSceneAutoLoadingSettings>(assetPath);
            
            if (settings == null)
                return false;
            
            EditorPrefs.SetString(AssetPathKey, assetPath);
            return true;
        }
    }
}