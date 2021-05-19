using UnityEditor;

namespace SaG.MainSceneAutoLoading.Utilities
{
    public static class SceneAssetUtility
    {
        public static string ConvertSceneAssetToString(SceneAsset sceneAsset) =>
            AssetDatabase.GetAssetOrScenePath(sceneAsset);

        public static SceneAsset ConvertPathToSceneAsset(string scenePath) =>
            AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
    }
}