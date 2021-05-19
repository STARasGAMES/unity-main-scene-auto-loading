using System.Linq;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEditor;
using UnityEngine;

namespace SaG.MainSceneAutoLoading.MainSceneProviders
{
    public class FirstSceneInBuildSettingsMainSceneProvider : IMainSceneProvider
    {
        public SceneAsset Get()
        {
            var editorBuildSettingsScene = EditorBuildSettings.scenes.FirstOrDefault(x => x.enabled);
            if (editorBuildSettingsScene == null)
            {
                Debug.LogError("Cannot provide main scene, because there is no enabled scene in build settings.");
                return null;
            }

            var path = editorBuildSettingsScene.path;
            SceneAsset sceneAsset = SceneAssetUtility.ConvertPathToSceneAsset(path);
            return sceneAsset;
        }
    }
}