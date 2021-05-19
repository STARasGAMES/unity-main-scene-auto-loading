using System;
using System.Linq;
using SaG.MainSceneAutoLoading.Settings;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SaG.MainSceneAutoLoading
{
    public static class MainSceneAutoLoader
    {
        public static LoadMainSceneArgs CurrentArgs { get; private set; }

        public static MainSceneAutoLoadingSettings Settings => MainSceneAutoLoadingSettings.GetOrCreate();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (!Settings.Enabled)
            {
                SetPlayModeStartScene(null);
                return;
            }

            var scene = Settings.GetMainSceneProvider().Get();
            SetPlayModeStartScene(scene);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnPlaymodeExited();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnEnteringPlayMode();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private static void OnEnteringPlayMode()
        {
            CurrentArgs = null;
            if (!Settings.Enabled)
            {
                SetPlayModeStartScene(null);
                return;
            }

            var mainScene = Settings.GetMainSceneProvider().Get();
            var currentLoadedScene =
                SceneAssetUtility.ConvertPathToSceneAsset(EditorSceneManager.GetActiveScene().path);

            if (mainScene == currentLoadedScene)
            {
                return;
            }

            SetPlayModeStartScene(mainScene);

            var loadedScenes = EditorSceneManager.GetSceneManagerSetup();
            var selectedGameObjects = Selection.gameObjects;
            GlobalObjectId[] selectedGameObjectsIds = new GlobalObjectId[selectedGameObjects.Length];
            GlobalObjectId.GetGlobalObjectIdsSlow(selectedGameObjects, selectedGameObjectsIds);
           
            var unfoldedObjects = SceneHierarchyUtility.GetExpandedGameObjects()
                .Select(go => GlobalObjectId.GetGlobalObjectIdSlow(go)).ToArray();
            var expandedScenes = SceneHierarchyUtility.GetExpandedSceneNames();
            CurrentArgs = new LoadMainSceneArgs(loadedScenes, selectedGameObjectsIds, unfoldedObjects, expandedScenes,
                Settings.RestoreHierarchyState);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnMainSceneLoaded()
        {
            if (CurrentArgs != null)
            {
                Settings.GetLoadMainSceneHandler().OnMainSceneLoaded(CurrentArgs);
            }
        }

        private static void OnPlaymodeExited()
        {
            if (CurrentArgs != null)
                Settings.GetPlaymodeExitedHandler().OnPlaymodeExited(CurrentArgs);
        }

        /// <summary>
        /// Returns true if playModeStartScene was changed, otherwise, false.
        /// </summary>
        /// <param name="sceneAsset">Scene asset</param>
        /// <returns>true if playModeStartScene was changed, otherwise, false</returns>
        public static bool SetPlayModeStartScene(SceneAsset sceneAsset)
        {
            if (EditorSceneManager.playModeStartScene != sceneAsset)
            {
                EditorSceneManager.playModeStartScene = sceneAsset;
                return true;
            }

            return false;
        }

        [MenuItem("Tools/MainSceneAutoLoading/Reset")]
        public static void Reset()
        {
            EditorSceneManager.playModeStartScene = null;
        }
    }
}