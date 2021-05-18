using System.Security.Permissions;
using UnityEditor;

namespace SaG.MainSceneAutoLoading
{
    public static class SceneAssetHelper
    {
        public static string ConvertSceneAssetToString(SceneAsset sceneAsset) =>
            AssetDatabase.GetAssetOrScenePath(sceneAsset);

        public static SceneAsset ConvertPathToSceneAsset(string scenePath) =>
            AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
    }
}