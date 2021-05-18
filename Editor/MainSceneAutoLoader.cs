using System;
using System.Linq;
using SaG.MainSceneAutoLoading.MainSceneProviders;
using SaG.MainSceneAutoLoading.Settings;
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
            Debug.Log("Initialize");
            if (!Settings.Enabled)
            {
                SetPlayModeStartScene(null);
                return;
            }

            var scene = Settings.GetMainSceneProvider().Get();
            SetPlayModeStartScene(scene);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Test()
        {
            Debug.Log($"RuntimeInitializeOnLoadMethod {EditorSceneManager.GetActiveScene().path}");
            if (CurrentArgs != null)
            {
                Debug.Log($"SceneSetups: {string.Join(", ",CurrentArgs.SceneSetups.Select(s => s.path))}");
                Settings.GetLoadMainSceneHandler().OnMainSceneLoaded(CurrentArgs);
            }
        }
        
        [InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            Debug.Log("OnEnterPlayMode");
            CurrentArgs = null;
            if (!Settings.Enabled)
            {
                SetPlayModeStartScene(null);
                return;
            }

            var mainScene = Settings.GetMainSceneProvider().Get();
            var currentLoadedScene = SceneAssetHelper.ConvertPathToSceneAsset(EditorSceneManager.GetActiveScene().path);

            if (mainScene == currentLoadedScene)
            {
                return;
            }

            if (SetPlayModeStartScene(mainScene))
            {
                Debug.Log("Play mode start scene was changed. Restarting playmode to apply changes...");
                EditorApplication.ExitPlaymode();
                EditorApplication.delayCall += Hack;

                // for some reason unity needs one more 'tick' to apply changes to EditorSceneManager.playModeStartScene
                void Hack()
                {
                    EditorApplication.delayCall += EditorApplication.EnterPlaymode;
                }

                return;
            }

            var loadedScenes = EditorSceneManager.GetSceneManagerSetup();
            CurrentArgs = new LoadMainSceneArgs(loadedScenes);
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